using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement movement;
    private UI ui_script;
    public GameObject WinUI;
    public GameObject WinBackground;

    private bool has_won = false;

    private void Start()
    {
        movement = FindObjectOfType<PlayerMovement>();
        ui_script = FindObjectOfType<UI>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !has_won)
        {
            StartCoroutine(ActivateWinScreen());
            has_won = true;
        }
    }
    
    IEnumerator ActivateWinScreen()
    {
        movement.enabled = false;;
        WinBackground.SetActive(true);
        ui_script.SlowDownTime();
        anim.Play("Win_Background_Fade_In");
        yield return new WaitForSecondsRealtime(1);
        WinUI.SetActive(true);
        ui_script.outline.SetActive(true);
        anim.Play("WinUI_Fade_In");
    }
}
