using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD_Script : MonoBehaviour
{
    public Slider HealthSlider;
    public TextMeshProUGUI Health_text;
    private PlayerHealthScript healthscript;

    
    // Start is called before the first frame update
    void Start()
    {
        healthscript = FindObjectOfType<PlayerHealthScript>();
    }
    private void OnGUI()
    {
        HealthSlider.value = healthscript.health;
        Health_text.text = "Health: " + healthscript.health;
    }
}
