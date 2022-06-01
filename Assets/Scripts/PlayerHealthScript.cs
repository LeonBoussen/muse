using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthScript : MonoBehaviour
{
    public int health = 100;
    public GameObject player;
    private bool hasdied = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !hasdied)
        {
            Destroy(player);
            hasdied = true;
        }
    }
}
