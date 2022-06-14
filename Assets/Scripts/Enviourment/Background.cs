using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private GameObject player;
    public GameObject[] backgrounds;
    public float offset;
    public float lerp_time;
    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }
    private void Update()
    {
        MoveBackground();
    }
    public void MoveBackground()
    {
        this.backgrounds[0].transform.position = player.transform.position;
        float cur_lerp_time = lerp_time;
        foreach (GameObject current_bg in this.backgrounds)
        {
            if (System.Array.IndexOf(this.backgrounds, current_bg) == 0)
            {
                continue;
            }
            else
            {
                int previous = System.Array.IndexOf(this.backgrounds, current_bg) - 1;
                float c = lerp_time -= offset; 
                current_bg.transform.position = Vector3.LerpUnclamped(this.transform.position, backgrounds[previous].transform.position, c);
            }
        }
        lerp_time = cur_lerp_time;
    }
    



}
