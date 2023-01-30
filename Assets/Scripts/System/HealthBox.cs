using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBox : MonoBehaviour
{
    public float maxHealtBox;
 
    void Reset()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerCollisionOnly");
        GetComponent<Collider>().isTrigger = true;

    }
    void OnTriggerEnter(Collider other)
    {
       /// Debug.Log("Entra al collider de la vida");
          Health h = other.GetComponent<Health>();


          if (h.currentHealth + maxHealtBox > h.maxHealth)
          {
            h.currentHealth = h.maxHealth;

            UiPlayerHealthBar.Instance.SetHealthPlayerBarPercentage(h.currentHealth / h.maxHealth);
           
        }
          else
          {
            h.currentHealth += maxHealtBox;
            UiPlayerHealthBar.Instance.SetHealthPlayerBarPercentage(h.currentHealth / h.maxHealth);

        }
            UiPlayerHealthBar.Instance.UpdateHealthInfo((int)h.currentHealth);
  
  
  
    }

}
