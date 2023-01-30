using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObjects : MonoBehaviour
{

    public float respawnTime = 10f;
    public Vector3[] respawnPositions;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Desactiva el objeto
            gameObject.SetActive(false);

            // Programa el respawn del objeto para después de respawnTime segundos
            Invoke("RespawnObject", respawnTime);
        }
        else if(other.CompareTag("Bullet"))
        {
            Invoke("RespawnObject", respawnTime);
        }
    }

    private void RespawnObject()
    {
         // Elije una posición al azar del arreglo
        int randomIndex = Random.Range(0, respawnPositions.Length);
        Vector3 respawnPosition = respawnPositions[randomIndex];
        // Reubica el objeto en la posición especificada
        transform.position = respawnPosition;

        // Activa el objeto
        gameObject.SetActive(true);
    }
}
