using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckForAFK : MonoBehaviour
{

    private float timer = 0.0f;
    public float afkDelay = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckAFK();
    }

    void CheckAFK()
    {
        if (Input.anyKey)
        {
            timer = 0.0f;
        }

        timer += Time.deltaTime;
        if (timer > afkDelay)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
