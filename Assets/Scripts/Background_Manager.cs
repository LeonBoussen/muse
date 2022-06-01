using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Manager : MonoBehaviour
{
    public GameObject background_prefab;
    private bool outofbounds = false;
    private GameObject player;
    GameObject currentbackground;
    public List<GameObject> background_objects;
    public float X_Borders;
    public float X_SpawnOffset;
    public float X_DestroyBorder;

    private Vector3 offset_left;
    private Vector3 offset_right;

    private Vector3 Spawn_Offset_left;
    private Vector3 Spawn_Offset_right;
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
        Spawn_Offset_right = new Vector3(player.transform.position.x + X_SpawnOffset, 0, 0);
        Spawn_Offset_left = new Vector3(player.transform.position.x - X_SpawnOffset, 0, 0);
        SpawnStartingBackground();
    }
    void SpawnStartingBackground()
    {
        currentbackground = Instantiate(background_prefab, player.transform.position, Quaternion.identity);
        GameObject leftbackground = Instantiate(background_prefab, Spawn_Offset_left, Quaternion.identity);
        GameObject rightbackground = Instantiate(background_prefab, Spawn_Offset_right, Quaternion.identity);
        background_objects.Add(leftbackground);
        background_objects.Add(currentbackground);
        background_objects.Add(rightbackground);
    }
    // Update is called once per frame
    void Update()
    {
        Spawn_Offset_right = new Vector3(player.transform.position.x + X_SpawnOffset, 0, 0);
        Spawn_Offset_left = new Vector3(player.transform.position.x - X_SpawnOffset, 0, 0);
        CheckForNewBackground();
    }
    public void CheckForNewBackground()
    {
        offset_right = new Vector3(player.transform.position.x + X_Borders, 0, 0);
        offset_left = new Vector3(player.transform.position.x - X_Borders, 0, 0);
        if (!outofbounds)
        {
            if (background_objects[1].transform.position.x > offset_left.x)
            {
                StartCoroutine(NewBackground(true));
            }
            else if (background_objects[1].transform.position.x > offset_right.x)
            {
                StartCoroutine(NewBackground(false));
            }
        }
            
        }
    IEnumerator NewBackground(bool left)
    {
        outofbounds = true;
        if (left)
        {
            background_objects.RemoveAt(0);
            Destroy(background_objects[0]);
            currentbackground = Instantiate(background_prefab, Spawn_Offset_right, Quaternion.identity);
            background_objects.Add(currentbackground);
        }
        else
        {
            background_objects.RemoveAt(2);
            Destroy(background_objects[2]);
            currentbackground = Instantiate(background_prefab, Spawn_Offset_left, Quaternion.identity);
            background_objects.Add(currentbackground);
        }
        print("new background set");
        yield return new WaitForSeconds(1f);
        outofbounds = false;
    }
}
    

