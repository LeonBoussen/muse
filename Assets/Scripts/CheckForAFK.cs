using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CheckForAFK : MonoBehaviour
{

    [SerializeField] private float timer = 0.0f;
    public float afkDelay = 30.0f;
    private PlayerMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = FindObjectOfType<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckAFK();
    }

    void CheckAFK()
    {
        if (movement.dirX > 0.9f || movement.dirX < -0.9f)
        {
            timer = 0.0f;
        }
        if (Input.anyKey)
        {
            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
            {
                timer = 0.0f;
            }
        }

        timer += Time.deltaTime;
        if (timer > afkDelay)
        {
            Time.timeScale = 1f;
            SoundManagerScript soundmanager = FindObjectOfType<SoundManagerScript>();
            soundmanager.StopMusic();
            soundmanager.PlayedMusic = false;
            SceneManager.LoadScene("MainMenu");
        }
    }
}
