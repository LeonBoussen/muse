using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UI : MonoBehaviour
{
    public GameObject[] PauseButtons;
    public GameObject[] DeathButtons;
    public GameObject[] WinButtons;
    public GameObject outline;

    private Animator anim;
    public float slowDownDuration;
    public GameObject DeathUI;
    public GameObject PauseUI;
    public GameObject WinUI;

    public GameObject DeathBackground;
    public GameObject PauseBackground;
    private int selectedItem;

    private bool IsPaused = false;
    private bool isCoolDown = false;
    private bool dpad_used = false;

    private PlayerMovement player_movement;
    private PlayerHealthScript player_health;
    private PlayerAttack player_attack;
    private WinScript winscript;
    private SoundManagerScript soundmanager;
    // Start is called before the first frame update
    void Start()
    {
        selectedItem = 0;
        soundmanager = FindObjectOfType<SoundManagerScript>();
        winscript = FindObjectOfType<WinScript>();
        player_movement = FindObjectOfType<PlayerMovement>();
        player_health = FindObjectOfType<PlayerHealthScript>();
        player_attack = FindObjectOfType<PlayerAttack>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var keypadY = Input.GetAxis("Keypad_Y");
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButton("Options"))
        {
            if (!isCoolDown && !player_health.hasdied && !winscript.has_won)
            {
                if (IsPaused)
                {
                    StartCoroutine(Unpause());
                }
                else
                {
                    StartCoroutine(Pause());
                }
                StartCoroutine(CoolDown());
            }
        }
        if (!dpad_used)
        {
            if (keypadY > 0.8f || keypadY < -0.8f)
            {
                if (PauseUI.activeInHierarchy && IsPaused)
                {
                    StartCoroutine(DPad_Cooldown(keypadY, .25f, 0));
                    dpad_used = true;
                }
                else if (DeathUI.activeInHierarchy && player_health.hasdied)
                {
                    StartCoroutine(DPad_Cooldown(keypadY, .25f, 1));
                    dpad_used = true;
                }
                else if (WinUI.activeInHierarchy && winscript.has_won)
                {
                    StartCoroutine(DPad_Cooldown(keypadY, .25f, 2));
                    dpad_used = true;
                }
            }
        }
        if (Input.GetKeyDown("joystick button 1"))
        {
            if (PauseUI.activeInHierarchy)
            {
                SelectOption(0);
            }
            else if (DeathUI.activeInHierarchy)
            {
                SelectOption(1);
            }
            else if (WinUI.activeInHierarchy)
            {
                SelectOption(2);
            }
        }
    }
    public void Pause_Game()
    {
        StartCoroutine(Pause());
    }
    public void Unpause_Game()
    {
        StartCoroutine(Unpause());
        
    }
    public void SlowDownTime()
    {
        StartCoroutine(ChangeTime(1f, 0.1f, slowDownDuration));
    }
    public void NormalizeTime()
    {
        StartCoroutine(ChangeTime(0.1f, 1f, slowDownDuration));
    }
    IEnumerator ChangeTime(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            Time.timeScale = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        Time.timeScale = v_end;
    }
    IEnumerator Pause()
    {
        player_movement.enabled = false;
        IsPaused = true;
        PauseBackground.SetActive(true);
        SlowDownTime();
        anim.Play("Pause_Background_Fade_In");
        yield return new WaitForSecondsRealtime(slowDownDuration);
        PauseUI.SetActive(true);
        EnableOutline();
        anim.Play("Pause_UI_Fade_In");

    }
    IEnumerator Unpause()
    {
        player_movement.enabled = true;
        player_attack.enabled = true;
        IsPaused = false;
        anim.Play("Pause_UI_Fade_Out");
        yield return new WaitForSecondsRealtime(0.51f);
        outline.SetActive(false);
        PauseUI.SetActive(false);

        anim.Play("Pause_Background_Fade_Out");
        yield return new WaitForSecondsRealtime(slowDownDuration);
        NormalizeTime();
        PauseBackground.SetActive(false);

    }
    IEnumerator CoolDown()
    {
        isCoolDown = true;
        yield return new WaitForSecondsRealtime(1f);
        isCoolDown = false;
    }
    public void GoToMainMenu()
    {
        soundmanager.StopMusic();
        soundmanager.PlayedMusic = false;
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    public void DeathScreen()
    {
        if (IsPaused)
        {
            Unpause_Game();
        }
        StartCoroutine(EnableDeathUI());
    }
    IEnumerator EnableDeathUI()
    {
        
        FindObjectOfType<SoundManagerScript>().PlaySFX(1);
        FindObjectOfType<HUD_Script>().HealthSlider.transform.parent.gameObject.SetActive(false);
        SlowDownTime();
        DeathBackground.SetActive(true);
        yield return new WaitForSecondsRealtime(0.01f);
        anim.Play("Death_Background_Fade_In");
        yield return new WaitForSecondsRealtime(3f);
        FreezePlayer();
        EnableOutline();
        DeathUI.SetActive(true);
        SetOutline(1);
        yield return new WaitForSecondsRealtime(0.01f);
        anim.Play("Death_UI_Fade_In");
    }
    private void FreezePlayer()
    {
        player_attack.enabled = false;
        player_movement.rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        player_movement.rb.freezeRotation = true;
    }
    private void SetOutline(int menu_kind)
    {
        soundmanager.PlaySFX(2);
        if (menu_kind == 0)
        {
            outline.transform.position = PauseButtons[selectedItem].transform.position;
        }
        else if (menu_kind == 1)
        {
            outline.transform.position = DeathButtons[selectedItem].transform.position;
        }
        else if (menu_kind == 2)
        {
            outline.transform.position = WinButtons[selectedItem].transform.position;
        }

    }
    IEnumerator DPad_Cooldown(float axis_value, float cooldown_duration, int menu)
    {
        if (axis_value > 0.1f)
        {
            selectedItem--;
            if (selectedItem < 0)
            {
                if (menu == 0)
                {
                    selectedItem = PauseButtons.Length - 1;
                }
                else if (menu == 1)
                {
                    selectedItem = DeathButtons.Length - 1;
                }
                else if (menu == 2)
                {
                    selectedItem = WinButtons.Length - 1;
                }

            }
        }
        else if (axis_value < -0.1f)
        {
            selectedItem++;
            if (menu == 0)
            {
                if (selectedItem > PauseButtons.Length - 1)
                {
                    selectedItem = 0;
                }
            }
            else if (menu == 1)
            {
                if (selectedItem > DeathButtons.Length - 1)
                {
                    selectedItem = 0;
                }
            }
            else if (menu == 2)
            {
                if (selectedItem > WinButtons.Length - 1)
                {
                    selectedItem = 0;
                }
            }

        }
        SetOutline(menu);
        yield return new WaitForSecondsRealtime(cooldown_duration);
        dpad_used = false;
    }
    private void SelectOption(int menu_kind)
    {
        soundmanager.PlaySFX(3);
        if (menu_kind == 0)
        {
            switch (selectedItem)
            {
                case 0:
                    Unpause_Game();
                    break;
                case 1:
                    GoToMainMenu();
                    break;
                case 2:
                    Restart();
                    break;
            }
        }
        else if (menu_kind == 1)
        {
            switch (selectedItem)
            {
                case 0:
                    GoToMainMenu();
                    break;
                case 1:
                    Restart();
                    break;
            }
        }
        else if (menu_kind == 2)
        {
            switch (selectedItem)
            {
                case 0:
                    GoToMainMenu();
                    break;
                case 1:
                    Restart();
                    break;
            }
        }
    }
    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void EnableOutline()
    {
        selectedItem = 0;
        outline.SetActive(true);
        if (PauseUI.activeInHierarchy)
        {
            outline.transform.position = PauseButtons[selectedItem].transform.position;
        }
        else if (DeathUI.activeInHierarchy)
        {
            outline.transform.position = DeathButtons[selectedItem].transform.position;
        }
        else if (WinUI.activeInHierarchy)
        {
            outline.transform.position = WinButtons[selectedItem].transform.position;
        }
    }
    }


