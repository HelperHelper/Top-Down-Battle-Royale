using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : GameTrigger
{
    public string keyType;

    public virtual void Opened()
    {
        Trigger();
    }

    void OnTriggerStay(Collider other)
    {
        var keychain = other.GetComponent<KeyChain>();

        if (keychain != null && keychain.HaveBriefcase(keyType))
        {
            Opened();
            //Solo destruye el script, si esta en la puerta no queremos destruir la puerta.
            Destroy(this);
          
        }
    }

}
