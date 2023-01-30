using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public float maxHealth;
    public int pointValue;
  //  public ParticleSystem DestroyedEffect;
    SkinnedMeshRenderer skinnedMeshRenderer;
    public float dieMaxTime = 2;

    [HideInInspector]
    public float currentHealth;


    public bool Destroyed => destroyed;

    bool destroyed = false;



    // Start is called before the first frame update
    void Start()
    {
        
        currentHealth = maxHealth;

        OnStart();
    }

    //metodo nuevo para quitar vida mediante projectil ya que el daño por raycast es diferente y pide un parametro extra que no necesita el projectil
    public void ProjectileTakeDamage(float damage, Vector3 direction)
    {
        currentHealth -= damage;

        if (currentHealth <= 0.0f && destroyed == false)
        {
            Die(direction);
            destroyed = true;
        }

         if (gameObject.CompareTag("Player"))
         {
                if (currentHealth >= 0)
                {
                    UiPlayerHealthBar.Instance.UpdateHealthInfo(((int)(currentHealth)));
                }
         }


        OnDamage(direction);

        Vector3 position = transform.position;



        //var effect = PoolSystem.Instance.GetInstance<ParticleSystem>(DestroyedEffect);
        //effect.time = 1.0f;
        //effect.Play();
        //if (destroyed == false)
        //{
        //    //Debug.Log("No se ve la sangre pero entra a hacer la particula");
        //    effect.transform.position = new Vector3(position.x, position.y - 2, position.z);
        //}
        //else
        //{
        //    effect.transform.position = new Vector3(position.x, position.y, position.z);

        //}


       
        if (currentHealth > 0)
            return;

        //if (DestroyedEffect != null)
        //{
        //var effect = PoolSystem.Instance.GetInstance<ParticleSystem>(DestroyedEffect);
        //effect.time = 0.0f;
        //effect.Play();
        //effect.transform.position = position;
        //}

        // destroyed = true;




        GameSystem.Instance.TargetDestroyed(pointValue);





    }

    private void Update()
    {
        if (destroyed == true)
        {

            dieMaxTime -= Time.deltaTime;
            if (dieMaxTime <= 0)
            {
                if (gameObject.CompareTag("Enemy"))
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject);

                    dieMaxTime = 0;
                    destroyed = false;
                }
                else
                if (gameObject.CompareTag("Player"))
                {
                    dieMaxTime = 0;
                    destroyed = false;
                }

            }
        }

    }

    private void Die(Vector3 direction)
    {

        OnDeath(direction);


    }

    protected virtual void OnStart()
    {

    }

    protected virtual void OnDeath(Vector3 direction)
    {

    }

    protected virtual void OnDamage(Vector3 direction)
    {

    }
}
