using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    private Animator anim;
    public float slowDownDuration;
    public GameObject DeathUI;
    public GameObject PauseUI;

    public GameObject DeathBackground;
    public GameObject PauseBackground;

    private bool IsPaused;
    private bool isCoolDown = false;

    private PlayerMovement player_movement;
    private PlayerHealthScript player_health;
    private PlayerAttack player_attack;
    // Start is called before the first frame update
    void Start()
    {
        player_movement = FindObjectOfType<PlayerMovement>();
        player_health = FindObjectOfType<PlayerHealthScript>();
        player_attack = FindObjectOfType<PlayerAttack>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        anim.Play("Pause_UI_Fade_In");

    }
    IEnumerator Unpause()
    {
        player_movement.enabled = true;
        player_attack.enabled = true;
        IsPaused = false;
        anim.Play("Pause_UI_Fade_Out");
        yield return new WaitForSecondsRealtime(0.51f);
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
        StartCoroutine(EnableDeathUI());
    }
    IEnumerator EnableDeathUI()
    {
        SlowDownTime();
        player_movement.enabled = false;
        player_attack.enabled = false;
        DeathBackground.SetActive(true);
        anim.Play("Death_Fade_In");
        yield return new WaitForSecondsRealtime(1f);
        DeathUI.SetActive(true);
        anim.Play("Death_UI_Fade_In");
    }
    public void Quit()
    {
        Application.Quit();
    }

}
