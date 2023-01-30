using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



[System.Serializable]
public class AmmoInventoryEntryEnemy
{
    [AmmoType]
    public int ammoType;
    public int amount = 0;
}

/// <summary>
/// Script utilizado para mantener la integración de la AI con las configuraciones iniciales además de controlar estados de la AI
/// </summary>
public class AiAgent : MonoBehaviour
{

    public static AiAgent Instance { get; protected set; }

    public AiStateId initialState;
    public AiAgentConfig config;
    public Transform weaponPosition;

    public Weapon[] startingWeapons;
    public AmmoInventoryEntryEnemy[] startingAmmo;
    public float chasingDistance = 20f;
    public float attackingDistance = 10f;

  [HideInInspector] public int currentWeapon;


   [HideInInspector] public List<Weapon> weapons = new List<Weapon>();
    Dictionary<int, int> ammoInventory = new Dictionary<int, int>();

    [HideInInspector] public AiStateMachine stateMachine;
    [HideInInspector] public NavMeshAgent navMeshAgent;
    [HideInInspector] public SkinnedMeshRenderer mesh;
    [HideInInspector] public GameObject character;

    void Awake()
    {
        Instance = this;

    }

    private void Start()
    {
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();


        character = GameObject.FindObjectOfType<Controller>().gameObject;

        stateMachine = new AiStateMachine(this);
       
        stateMachine.RegisterState(new AiChasePlayerState());
        stateMachine.RegisterState(new AiDeathState());
        stateMachine.RegisterState(new AiAttackPlayerState());
        stateMachine.ChangeState(initialState);


        for (int i = 0; i < startingWeapons.Length; ++i)
        {
            PickupWeapon(startingWeapons[i]);
        }

        ChangeWeapon(0);

        for (int i = 0; i < startingAmmo.Length; ++i)
        {
            ammoInventory[startingAmmo[i].ammoType] = startingAmmo[i].amount;
        }


    
        
    }

    private void ChangeWeapon(int number)
    {
        if (currentWeapon != -1)
        {
            weapons[currentWeapon].PutAway();
            weapons[currentWeapon].gameObject.SetActive(false);
        }

        currentWeapon = number;

        if (currentWeapon < 0)
            currentWeapon = weapons.Count - 1;
        else if (currentWeapon >= weapons.Count)
            currentWeapon = 0;

        weapons[currentWeapon].gameObject.SetActive(true);
        weapons[currentWeapon].Selected();
    }

    private void PickupWeapon(Weapon prefab)
    {
        var w = Instantiate(prefab, weaponPosition, false);
        w.name = prefab.name;
        w.transform.localPosition = Vector3.zero;
        w.transform.localRotation = Quaternion.identity;
        w.gameObject.SetActive(false);
        w.PickedUp(this);

        weapons.Add(w);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chasingDistance);
    }


    public int GetAmmo(int ammoType)
    {
        int value = 0;
        ammoInventory.TryGetValue(ammoType, out value);

        return value;
    }

    private void Update()
    {
        stateMachine.Update();

       
    }
}
