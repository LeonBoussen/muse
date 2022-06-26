using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject[] buttons;
    public GameObject MainScreen;
    public GameObject outline;
    private bool dpad_used = false;
    private int selectedItem;
    public GameObject Controls_Screen;

    private bool control_Screen_on;
    private SoundManagerScript soundmanager;

    private void Start()
    {
        soundmanager = FindObjectOfType<SoundManagerScript>();
    }
    private void Update()
    {
        if (Input.GetKeyDown("joystick button 1"))
        {
            SelectOption();
        }
        if (Input.GetKeyDown("joystick button 2"))
        {
            if (control_Screen_on)
            {
                ExitControls_Screen();
            }
        }
        var keypadY = Input.GetAxis("Keypad_Y");
        if (!dpad_used && !control_Screen_on)
        {
            if (keypadY > 0.8f || keypadY < -0.8f)
            {
                StartCoroutine(DPad_Cooldown(keypadY, .25f));
                dpad_used = true;
            }
        }
    }
    public void Play()
    {
        SoundManagerScript soundmanager = FindObjectOfType<SoundManagerScript>();
        soundmanager.StopMusic();
        soundmanager.PlayedMusic = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        StartCoroutine(soundmanager.BGM_Music());

    }
    public void Controls()
    {
        Controls_Screen.SetActive(true);
        control_Screen_on = true;
    }
    public void Quit()
    {
        Application.Quit();
    }
    IEnumerator DPad_Cooldown(float axis_value, float cooldown_duration)
    {
        if (axis_value > 0.1f)
        {
            selectedItem--;
            if (selectedItem < 0)
            {
                selectedItem = buttons.Length - 1;
            }
        }
        else if (axis_value < -0.1f)
        {
            selectedItem++;
            if (selectedItem > buttons.Length - 1)
            {
                selectedItem = 0;
            }

        }
        SetOutline();
        yield return new WaitForSecondsRealtime(cooldown_duration);
        dpad_used = false;
    }
    private void SelectOption()
    {
        if (control_Screen_on)
        {
            return;
        }
        else
        {
            soundmanager.PlaySFX(3);
            switch (selectedItem)
            {
                case 0:
                    Play();
                    break;
                case 1:
                    Controls();
                    break;
            }
        }
    }
    private void SetOutline()
    {
        soundmanager.PlaySFX(2);
        outline.transform.position = buttons[selectedItem].transform.position;
    }
    public void ExitControls_Screen()
    {
        soundmanager.PlaySFX(3);
        Controls_Screen.SetActive(false);
        control_Screen_on = false;
    }
}
