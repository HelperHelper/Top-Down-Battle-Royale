using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Weapon : MonoBehaviour
{
    public enum TriggerType
    {
        Auto,
        Manual
    }

    public enum WeaponType
    {
        Projectile
    }

    public enum WeaponState
    {
        Idle,
        Firing
    }

    [System.Serializable]
    public class AdvancedSettings
    {
        public float spreadAngle = 0.0f;
        public int projectilePerShot = 1;
        public float screenShakeMultiplier = 1.0f;
    }

    public TriggerType triggerType = TriggerType.Manual;
    public WeaponType weaponType = WeaponType.Projectile;
    public float fireRate = 0.5f;
    public float reloadTime = 2.0f;
    public int clipSize = 4;
    public float damage = 1.0f;


    [AmmoType]
    public int ammoType = -1;

    public Projectile projectilePrefab;
    public float projectileLaunchForce = 200.0f;

    public Transform EndPoint;
    Animator animator;


    public AdvancedSettings advancedSettings;

    [Header("Animation Clips")]
    public AnimationClip FireAnimationClip;
  

    [Header("Audio Clips")]
    public AudioClip FireAudioClip;
    public AudioClip ReloadAudioClip;

    [Header("Visual Settings")]
    public bool DisabledOnEmpty;

    [Header("Visual Display")]
    public AmmoDisplay AmmoDisplay;

    public bool triggerDown
    {
        get { return TriggerDown; }
        set
        {
            TriggerDown = value;
            if (!TriggerDown) shotDone = false;
        }
    }

    public WeaponState CurrentState => currentState;
    public int ClipContent => clipContent;
    public Controller Owner => owner;

    public AiAgent ownerAi => ownerai;

    Controller owner;
    AiAgent ownerai;


    WeaponState currentState;
    bool shotDone;
    float shotTimer = -1.0f;
    bool TriggerDown;
    int clipContent;

    Vector3 convertedMuzzlePos;

    Queue<Projectile> projectilePool = new Queue<Projectile>();

    int fireNameHash = Animator.StringToHash("fire");

   

    void Awake()
    {

        animator = GetComponentInParent<Animator>();
        clipContent = clipSize;

        if (projectilePrefab != null)
        {
            //a minimum of 4 is useful for weapon that have a clip size of 1 and where you can throw a second
            //or more before the previous one was recycled/exploded.
            ///un mínimo de 4 es útil para armas que tienen un tamaño de cargador de 1 y donde se puede lanzar un segundo
            // o más antes de que el anterior haya sido reciclado / explotado.
            int size = Mathf.Max(4, clipSize) * advancedSettings.projectilePerShot;
            for (int i = 0; i < size; ++i)
            {
                Projectile p = Instantiate(projectilePrefab);
                p.gameObject.SetActive(false);
                projectilePool.Enqueue(p);
            }
        }
    }

    public void PickedUp(Controller c)
    {
        owner = c;
 
    }

    public void PickedUp(AiAgent c)
    {
        ownerai = c;

    }

    public void PutAway()
    {
        if (gameObject.CompareTag("WeaponPlayer"))
        {
            animator.WriteDefaultValues();
        }
        if (gameObject.CompareTag("WeaponEnemy"))
        {
            animator.WriteDefaultValues();
        }

    }


    public void PlayerSelected()
    {
        var ammoRemaining = owner.GetAmmo(ammoType);
        // var upperBodyControl = animator;

        if (DisabledOnEmpty)
            gameObject.SetActive(ammoRemaining != 0 || clipContent != 0);


        WeaponInfoUI.Instance.UpdateClipInfo(this);
        WeaponInfoUI.Instance.UpdateAmmoAmount(owner.GetAmmo(ammoType));

        if (clipContent == 0 && ammoRemaining != 0)
        {
            //this can only happen if the weapon ammo reserve was empty and we picked some since then. So directly
            //reload the clip when wepaon is selected
            //esto sólo puede ocurrir si la reserva de munición del arma estaba vacía y recogimos algo desde entonces. Así que directamente
            //recargar el cargador cuando se selecciona arma  
            int chargeInClip = Mathf.Min(ammoRemaining, clipSize);
            clipContent += chargeInClip;
            if (AmmoDisplay)
                AmmoDisplay.UpdateAmount(clipContent, clipSize);
            owner.ChangeAmmo(ammoType, -chargeInClip);
            WeaponInfoUI.Instance.UpdateClipInfo(this);
        }

        if (FireAnimationClip != null)
        {
            animator.GetLayerIndex("upperBody");

            animator.SetFloat("fireSpeed", FireAnimationClip.length / fireRate);



        }


        currentState = WeaponState.Idle;

        triggerDown = false;
        shotDone = false;


        if (AmmoDisplay)
            AmmoDisplay.UpdateAmount(clipContent, clipSize);



        animator.SetTrigger("selected");

    }

    public void Selected()
    {
     
            var ammoReaminingAi = ownerai.GetAmmo(ammoType);
            if (DisabledOnEmpty)
                gameObject.SetActive(ammoReaminingAi != 0 || clipContent != 0);

            if (clipContent == 0 && ammoReaminingAi != 0)
            {
                //this can only happen if the weapon ammo reserve was empty and we picked some since then. So directly
                //reload the clip when wepaon is selected
                //esto sólo puede ocurrir si la reserva de munición del arma estaba vacía y recogimos algo desde entonces. Así que directamente
                //recargar el cargador cuando se selecciona arma  
                int chargeInClip = Mathf.Min(ammoReaminingAi, clipSize);
                clipContent += chargeInClip;
                if (AmmoDisplay)
                    AmmoDisplay.UpdateAmount(clipContent, clipSize);
            }

        

        if (FireAnimationClip != null)
        {
            animator.GetLayerIndex("upperBody");
            animator.SetFloat("fireSpeed", FireAnimationClip.length / fireRate);

            

        }

            currentState = WeaponState.Idle;

            triggerDown = false;
            shotDone = false;


        if (AmmoDisplay)
                AmmoDisplay.UpdateAmount(clipContent, clipSize);

      

            animator.SetTrigger("selected");
    }

    public void Fire()
    {
        if (currentState != WeaponState.Idle || shotTimer > 0 || clipContent == 0)
            return;

        clipContent -= 1;



        shotTimer = fireRate;

        if (AmmoDisplay)
            AmmoDisplay.UpdateAmount(clipContent, clipSize);

        if(owner != null)
        WeaponInfoUI.Instance.UpdateClipInfo(this);

        //the state will only change next frame, so we set it right now.
        //el estado sólo cambiará en el próximo frame, así que lo establecemzos ahora mismo.
        currentState = WeaponState.Firing;

        animator.SetTrigger("fire");

        if (weaponType == WeaponType.Projectile)
        {
            ProjectileShot();
        }
    }

    void ProjectileShot()
    {
        for (int i = 0; i < advancedSettings.projectilePerShot; ++i)
        {
            float angle = Random.Range(0.0f, advancedSettings.spreadAngle * 0.5f);
            Vector2 angleDir = Random.insideUnitCircle * Mathf.Tan(angle * Mathf.Deg2Rad);

            Vector3 dir = EndPoint.transform.forward + (Vector3)angleDir;
            dir.Normalize();

            var p = projectilePool.Dequeue();

            p.gameObject.SetActive(true);
            p.Launch(this, dir, projectileLaunchForce);
        }

    }

    //For optimization, when a projectile is "destroyed" it is instead disabled and return to the weapon for reuse.
    //Por optimización, cuando un proyectil es "destruido" en su lugar es desactivado y devuelto al arma para su reutilización.
    public void ReturnProjecticle(Projectile p)
    {
        projectilePool.Enqueue(p);
    }

    public void Reload()
    {
        if (currentState != WeaponState.Idle || clipContent == clipSize)
            return;

        if(owner != null)
        {

            int remainingBullet = owner.GetAmmo(ammoType);

            if (remainingBullet == 0)
            {
                //No more bullet, so we disable the gun so it's displayed on empty (useful e.g. for  grenade)
                // Ya no hay bala, así que desactivamos el arma para que se muestre en vacío (útil, por ejemplo, para la granada)

                if (DisabledOnEmpty)
                    gameObject.SetActive(false);
                return;
            }

                int chargeInClip = Mathf.Min(remainingBullet, clipSize - clipContent);

                ////the state will only change next frame, so we set it right now.
                ////el estado sólo cambiará en el próximo frame, así que lo establecemos ahora mismo.
                //currentState = WeaponState.Reloading;

                clipContent += chargeInClip;

            if (AmmoDisplay)
                AmmoDisplay.UpdateAmount(clipContent, clipSize);

            owner.ChangeAmmo(ammoType, -chargeInClip);

            WeaponInfoUI.Instance.UpdateClipInfo(this);

        }

        if (ownerai != null)
        {

            int remainingBullet = ownerai.GetAmmo(ammoType);

            if (remainingBullet == 0)
            {
                //No more bullet, so we disable the gun so it's displayed on empty (useful e.g. for  grenade)
                // Ya no hay bala, así que desactivamos el arma para que se muestre en vacío (útil, por ejemplo, para la granada)

                if (DisabledOnEmpty)
                    gameObject.SetActive(false);
                return;
            }

            int chargeInClip = Mathf.Min(remainingBullet, clipSize - clipContent);

            ////the state will only change next frame, so we set it right now.
            ////el estado sólo cambiará en el próximo frame, así que lo establecemos ahora mismo.
            //currentState = WeaponState.Reloading;

            clipContent += chargeInClip;

            if (AmmoDisplay)
                AmmoDisplay.UpdateAmount(clipContent, clipSize);

        }
       
    }

    void Update()
    {
            //Debug.Log("El jugador usa su arma y actualiza las propiedades del controlador");
            UpdateControllerState();

        if (shotTimer > 0)
            shotTimer -= Time.deltaTime;
    }

    void UpdateControllerState()
    {
        if(owner != null)
        {
        animator.SetFloat("speed", owner.Speed);

        animator.SetBool("grounded", owner.Grounded);

        }

        var info = animator.GetCurrentAnimatorStateInfo(0);

        WeaponState newState;
            if (info.shortNameHash == fireNameHash)
                newState = WeaponState.Firing;
            else
                newState = WeaponState.Idle;

            if (newState != currentState)
            {
                var oldState = currentState;
                currentState = newState;

                if (oldState == WeaponState.Firing)
                {//we just finished firing, so check if we need to auto reload
                 //acabamos de terminar de disparar, asi que comprueba si necesitamos auto recargar

                    if (clipContent == 0)
                        Reload();
                }
            }
        

        if (triggerDown)
        {
            if (triggerType == TriggerType.Manual)
            {
                if (!shotDone)
                {
                    shotDone = true;

                    Fire();
                }
            }
            else
                Fire();
        }
    }


    /// <summary>
    /// This will compute the corrected position of the muzzle flash in world space. Since the weapon camera use a
    /// different FOV than the main camera, using the muzzle spot to spawn thing rendered by the main camera will appear
    /// disconnected from the muzzle flash. So this convert the muzzle post from
    /// world -> view weapon -> clip weapon -> inverse clip main cam -> inverse view cam -> corrected world pos
    /// Esto calculará la posición corregida del fogonazo en el espacio del mundo. Dado que la cámara del arma utiliza un
    /// diferente FOV que la cámara principal, usando el punto de la boca del cañón para generar algo renderizado por la cámara principal aparecerá
    /// desconectado del fogonazo. Asi que esto convierte el muzzle post de
    /// world -> view weapon -> clip weapon -> inverse clip main cam -> inverse view cam -> corrected world pos
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCorrectedMuzzlePlace()
    {
        Vector3 position = EndPoint.position;

        return position;
    }
}

public class AmmoTypeAttribute : PropertyAttribute
{

}

public abstract class AmmoDisplay : MonoBehaviour
{
    public abstract void UpdateAmount(int current, int max);
}

#if UNITY_EDITOR


[CustomPropertyDrawer(typeof(AmmoTypeAttribute))]
public class AmmoTypeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        AmmoDatabase ammoDB = GameDatabase.Instance.ammoDatabase;

        if (ammoDB.entries == null || ammoDB.entries.Length == 0)
        {
            EditorGUI.HelpBox(position, "Please define at least 1 ammo type in the Game Database", MessageType.Error);
        }
        else
        {
            int currentID = property.intValue;
            int currentIdx = -1;

            //this is pretty ineffective, maybe find a way to cache that if prove to take too much time
            string[] names = new string[ammoDB.entries.Length];
            for (int i = 0; i < ammoDB.entries.Length; ++i)
            {
                names[i] = ammoDB.entries[i].name;
                if (ammoDB.entries[i].id == currentID)
                    currentIdx = i;
            }

            EditorGUI.BeginChangeCheck();
            int idx = EditorGUI.Popup(position, "Ammo Type", currentIdx, names);
            if (EditorGUI.EndChangeCheck())
            {
                property.intValue = ammoDB.entries[idx].id;
            }
        }
    }
}

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
    SerializedProperty triggerTypeProp;
    SerializedProperty weaponTypeProp;
    SerializedProperty fireRateProp;
    SerializedProperty reloadTimeProp;
    SerializedProperty clipSizeProp;
    SerializedProperty damageProp;
    SerializedProperty ammoTypeProp;
    SerializedProperty projectilePrefabProp;
    SerializedProperty projectileLaunchForceProp;
    SerializedProperty endPointProp;
    SerializedProperty advancedSettingsProp;
    SerializedProperty fireAnimationClipProp;
    SerializedProperty fireAudioClipProp;
    SerializedProperty reloadAudioClipProp;
    SerializedProperty ammoDisplayProp;
    SerializedProperty disabledOnEmpty;

    void OnEnable()
    {
        triggerTypeProp = serializedObject.FindProperty("triggerType");
        weaponTypeProp = serializedObject.FindProperty("weaponType");
        fireRateProp = serializedObject.FindProperty("fireRate");
        reloadTimeProp = serializedObject.FindProperty("reloadTime");
        clipSizeProp = serializedObject.FindProperty("clipSize");
        damageProp = serializedObject.FindProperty("damage");
        ammoTypeProp = serializedObject.FindProperty("ammoType");
        projectilePrefabProp = serializedObject.FindProperty("projectilePrefab");
        projectileLaunchForceProp = serializedObject.FindProperty("projectileLaunchForce");
        endPointProp = serializedObject.FindProperty("EndPoint");
        advancedSettingsProp = serializedObject.FindProperty("advancedSettings");
        fireAnimationClipProp = serializedObject.FindProperty("FireAnimationClip");
        fireAudioClipProp = serializedObject.FindProperty("FireAudioClip");
        reloadAudioClipProp = serializedObject.FindProperty("ReloadAudioClip");
        ammoDisplayProp = serializedObject.FindProperty("AmmoDisplay");
        disabledOnEmpty = serializedObject.FindProperty("DisabledOnEmpty");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(triggerTypeProp);
        EditorGUILayout.PropertyField(weaponTypeProp);
        EditorGUILayout.PropertyField(fireRateProp);
        EditorGUILayout.PropertyField(reloadTimeProp);
        EditorGUILayout.PropertyField(clipSizeProp);
        EditorGUILayout.PropertyField(damageProp);
        EditorGUILayout.PropertyField(ammoTypeProp);

        if (weaponTypeProp.intValue == (int)Weapon.WeaponType.Projectile)
        {
            EditorGUILayout.PropertyField(projectilePrefabProp);
            EditorGUILayout.PropertyField(projectileLaunchForceProp);
        }

        EditorGUILayout.PropertyField(endPointProp);
        EditorGUILayout.PropertyField(advancedSettingsProp, new GUIContent("Advance Settings"), true);
        EditorGUILayout.PropertyField(fireAnimationClipProp);
        EditorGUILayout.PropertyField(fireAudioClipProp);
        EditorGUILayout.PropertyField(reloadAudioClipProp);

        EditorGUILayout.PropertyField(ammoDisplayProp);
        EditorGUILayout.PropertyField(disabledOnEmpty);

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
