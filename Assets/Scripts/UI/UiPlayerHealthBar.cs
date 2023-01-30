using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiPlayerHealthBar : MonoBehaviour
{

    public static UiPlayerHealthBar Instance { get; private set; }

    public TMP_Text healthText;
    public TMP_Text maxHealth;

    void OnEnable()
    {
        Instance = this;
    }


    public void UpdateMaxHealth(int maxhealth)
    {
        maxHealth.text = maxhealth.ToString();
    }

    public void UpdateHealthInfo(int health)
    {
        healthText.text = health.ToString();
    }

    public void SetHealthPlayerBarPercentage(float percentage)
    {
        float parentWidth = GetComponent<RectTransform>().rect.width;
        float width = parentWidth * percentage;
       
        
    }
}
