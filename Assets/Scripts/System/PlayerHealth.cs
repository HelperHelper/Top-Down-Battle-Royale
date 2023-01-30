using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.UI;

public class PlayerHealth : Health
{

    public static PlayerHealth Instance { get; private set; }

    [Header("PlayerInfo")]
  
    public float velocityRotation = 0.05f;
    bool death;
    [HideInInspector] public bool playerdeath;
    float time = 5f;

    public void Awake()
    {
        Instance = this;
    }

    protected override void OnStart()
    {
        UiPlayerHealthBar.Instance.UpdateMaxHealth((int)maxHealth);
        UiPlayerHealthBar.Instance.UpdateHealthInfo((int)maxHealth);


    }

    private void Update()
    {

       

        if (death == true) {

            Quaternion z = gameObject.transform.rotation;

            z.z -= velocityRotation;
            time = time + Time.deltaTime;

            Vector3 rotationToAdd = new Vector3(0, 0, z.z);
            transform.Rotate(rotationToAdd);

            if (time >= 13)
            {
               
                Controller.Instance.enabled = false;
              
                velocityRotation = 0;
                death = false;
                playerdeath = true;
            }

        }
    }

    protected override void OnDeath(Vector3 direction)
    {
        death = true;
        Time.timeScale = 0;
        GameOver.Instance.GameOverPlayer();
    }

    protected override void OnDamage(Vector3 direction)
    {
       UiPlayerHealthBar.Instance.SetHealthPlayerBarPercentage(currentHealth / maxHealth);
    }
}
