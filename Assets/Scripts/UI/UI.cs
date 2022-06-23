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
    public GameObject outline;

    private Animator anim;
    public float slowDownDuration;
    public GameObject DeathUI;
    public GameObject PauseUI;

    public GameObject DeathBackground;
    public GameObject PauseBackground;
    private int selectedItem;

    private bool IsPaused = false;
    private bool isCoolDown = false;
    private bool dpad_used = false;

    private PlayerMovement player_movement;
    private PlayerHealthScript player_health;
    private PlayerAttack player_attack;
    // Start is called before the first frame update
    void Start()
    {
        selectedItem = 0;
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
            if (!isCoolDown && !player_health.hasdied)
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
                    StartCoroutine(DPad_Cooldown(keypadY, .25f, true));
                    dpad_used = true;
                }
                if (DeathUI.activeInHierarchy && player_health.hasdied)
                {
                    StartCoroutine(DPad_Cooldown(keypadY, .25f, false));
                    dpad_used = true;
                }
            }
        }
        if (Input.GetKeyDown("joystick button 1"))
        {
            if (PauseUI.activeInHierarchy)
            {
                SelectOption(true);
            }
            else if (DeathUI.activeInHierarchy)
            {
                SelectOption(false);
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
        outline.SetActive(true);
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
        //soundmanager.StopMusic();
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
        FindObjectOfType<HUD_Script>().HealthSlider.transform.parent.gameObject.SetActive(false);
        SlowDownTime();
        DeathBackground.SetActive(true);
        yield return new WaitForSecondsRealtime(0.01f);
        anim.Play("Death_Background_Fade_In");
        yield return new WaitForSecondsRealtime(3f);
        FreezePlayer();
        outline.SetActive(true);
        DeathUI.SetActive(true);
        SetOutline(false);
        yield return new WaitForSecondsRealtime(0.01f);
        anim.Play("Death_UI_Fade_In");
    }
    private void FreezePlayer()
    {
        player_attack.enabled = false;
        player_movement.rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        player_movement.rb.freezeRotation = true;
    }
    private void SetOutline(bool isPauseMenu)
    {
        if (isPauseMenu)
        {
            outline.transform.position = PauseButtons[selectedItem].transform.position;
        }
        else
        {
            outline.transform.position = DeathButtons[selectedItem].transform.position;
        }

    }
    IEnumerator DPad_Cooldown(float axis_value, float cooldown_duration, bool isPauseMenu)
    {
        if (axis_value > 0.1f)
        {
            selectedItem--;
            if (selectedItem < 0)
            {
                if (isPauseMenu)
                {
                    selectedItem = PauseButtons.Length - 1;
                }
                else
                {
                    selectedItem = DeathButtons.Length - 1;
                }

            }
        }
        else if (axis_value < -0.1f)
        {
            selectedItem++;
            if (isPauseMenu)
            {
                if (selectedItem > PauseButtons.Length - 1)
                {
                    selectedItem = 0;
                }
            }
            else
            {
                if (selectedItem > DeathButtons.Length - 1)
                {
                    selectedItem = 0;
                }
            }

        }
        if (isPauseMenu)
        {
            SetOutline(true);
        }
        else
        {
            SetOutline(false);
        }

        yield return new WaitForSecondsRealtime(cooldown_duration);
        dpad_used = false;
    }
    private void SelectOption(bool IsPauseMenu)
    {
        if (IsPauseMenu)
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
        else
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
    }


