using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyChain : MonoBehaviour
{
    List<string> keyTypeOwned = new List<string>();

    // metodo para saber si cogio la llave
    public void GrabbedBriefcase(string keyType)
    {
        keyTypeOwned.Add(keyType);
     
    }

    // metodo para verificar que sigue teniendo la llave o dispone de la llave cuando intenta acceder a la puerta
    public bool HaveBriefcase(string keyType)
    {
        return keyTypeOwned.Contains(keyType);

    }

    //// metodo que permite usar la llave y luego de usarlo removerlo
    //public void UseKey(string keyType)
    //{
       
    //    keyTypeOwned.Remove(keyType);
    //}
}
