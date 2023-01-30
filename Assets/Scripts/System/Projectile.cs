using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public bool DestroyedOnHit = true;
    public float TimeToDestroyed = 4.0f;
    public float ReachRadius = 5.0f;
    public float damage = 10.0f;
    public AudioClip DestroyedSound;

    public GameObject PrefabOnDestruction;

    Weapon owner;
    Rigidbody mrigidbody;
    float timeSinceLaunch;

    void Awake()
    {
        PoolSystem.Instance.InitPool(PrefabOnDestruction, 4);
        mrigidbody = GetComponent<Rigidbody>();
    }

    public void Launch(Weapon launcher, Vector3 direction, float force)
    {
        owner = launcher;

       transform.position = launcher.GetCorrectedMuzzlePlace();
        transform.forward = launcher.EndPoint.forward;

        gameObject.SetActive(true);
        timeSinceLaunch = 0.0f;
        mrigidbody.AddForce(direction * force);
    }

    void OnCollisionEnter(Collision other)
    {
        if (DestroyedOnHit)
        {
            Destroy();
        }
    }

    void Destroy()
    {
        Vector3 position = transform.position;

        var effect = PoolSystem.Instance.GetInstance<GameObject>(PrefabOnDestruction);
        effect.transform.position = position;
        effect.SetActive(true);

        Collider[] hitColliders = Physics.OverlapSphere(position, ReachRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if(hitColliders[i].TryGetComponent(out Health health))
            {
                health.ProjectileTakeDamage(damage, position);
            }
        }

        gameObject.SetActive(false);
        mrigidbody.velocity = Vector3.zero;
        mrigidbody.angularVelocity = Vector3.zero;
        owner.ReturnProjecticle(this);

    }

    void Update()
    {
        timeSinceLaunch += Time.deltaTime;

        if (timeSinceLaunch >= TimeToDestroyed)
        {
            Destroy();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, ReachRadius);
    }
}
