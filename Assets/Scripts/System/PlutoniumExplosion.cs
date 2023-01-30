using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlutoniumExplosion : MonoBehaviour
{
    public float TimeToDestroyed = 4.0f;
    public float ReachRadius = 5.0f;
    public float damage = 10.0f;
    public AudioClip DestroyedSound;

    public GameObject PrefabOnDestruction;
    public float respawnTime = 10f;
    public Vector3[] respawnPositions;

    Rigidbody mrigidbody;
    float timeSinceLaunch;

    void Awake()
    {
        mrigidbody = GetComponent<Rigidbody>();

    }


    void OnCollisionEnter(Collision other)
    {

        //Debug.Log("Entro a la collisión" + other.gameObject.name);

        if (other.gameObject.CompareTag("Bullet"))
        {
           
                Destroy();
            Invoke("RespawnObject", respawnTime);
        }
    }

    void Destroy()
    {
        Vector3 position = transform.position;

        if (timeSinceLaunch >= TimeToDestroyed)
        {
            PoolSystem.Instance.InitPool(PrefabOnDestruction, 4);
            var effect = PoolSystem.Instance.GetInstance<GameObject>(PrefabOnDestruction);
            effect.transform.position = position;
            effect.SetActive(true);
        }

        Collider[] hitColliders = Physics.OverlapSphere(position, ReachRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].TryGetComponent(out Health health))
            {
                health.ProjectileTakeDamage(damage, position);
            }
        }

        gameObject.SetActive(false);
        mrigidbody.velocity = Vector3.zero;
        mrigidbody.angularVelocity = Vector3.zero;

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

    void Update()
    {
        timeSinceLaunch += Time.deltaTime;

        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, ReachRadius);
    }
}
