using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
    public int health = 100;
    public GameObject player;
    public bool hasdied = false;
    private UI ui_script;
    void Start()
    {
        ui_script = FindObjectOfType<UI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !hasdied)
        {
            Die();
        }
    }
    public void Die()
    {
        ui_script.DeathScreen();
        hasdied = true;
    }
}
