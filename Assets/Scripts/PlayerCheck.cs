using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    private Enemy enemy_script;
    private void Start()
    {
        enemy_script = GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            enemy_script.isMoving = true;
        }
        if (col.CompareTag("Enemy"))
        {
            return;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            enemy_script.isMoving = false;
        }
        if (col.CompareTag("Enemy"))
        {
            return;
        }
    }
}
