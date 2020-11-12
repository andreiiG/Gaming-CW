using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI hp;
    public string max;
    
    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        max = "/" + health;
        hp.text = health + max;
    }
    
    public void setHealth(int health)
    {
        slider.value = health;
        hp.text = health + max;
    }
}
