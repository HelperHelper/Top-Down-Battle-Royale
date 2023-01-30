using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Key : MonoBehaviour
{
    public string keyType;
    public TMP_Text keyNameText;
    public GameObject key;
    KeyIcon keyimage;

    // cuando se habilita la llave y se activa se ejecuta está función
    // mostrando la información de que es una llave
    void OnEnable()
    {
        keyNameText.text = keyType;
    }

    // Cuando el jugador coliciona con la  llave lo agarra y lo destruye
    void OnTriggerEnter(Collider other)
    {

        

        

        var keychain = other.GetComponent<KeyChain>();

      
        if (keychain != null)
        {
            keyimage = GameObject.FindGameObjectWithTag("KeyIcon").GetComponent<KeyIcon>();
            keyimage.IconOnColor();
            keychain.GrabbedBriefcase(keyType);
            // gameObject.SetActive(false);
            if (other.CompareTag("Player"))
            {
                //Debug.Log("El jugador recogio el maletin");
                Controller.Instance.key = true;
                Destroy(gameObject);

            }
        }
    }
}
