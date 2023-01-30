using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AmmoInventoryEntry
{
    [AmmoType]
    public int ammoType;
    public int amount = 0;
}

public class Controller : MonoBehaviour
{
    public static Controller Instance { get; protected set; }

    public Transform WeaponPosition;

    public Weapon[] startingWeapons;
    public AmmoInventoryEntry[] startingAmmo;


    [Header("Control Settings")]
    public float MouseSensitivity = 100.0f;
    public float PlayerSpeed = 5.0f;
    public float RunningSpeed = 7.0f;
    public float JumpSpeed = 5.0f;

    //// Cabecera para controlar el audio del jugador
    //[Header("Audio")]
    //// public RandomPlayer FootstepPlayer;

    float verticalSpeed = 0.0f;
    bool isPaused = false;
    int currentWeapon;

    float verticalAngle, horizontalAngle;
    public float Speed { get; private set; } = 0.0f;

    public bool LockControl { get; set; }
    public bool CanPause { get; set; } = true;
    public bool key { get; set; } = false;

    public bool Grounded => grounded;


    CharacterController characterController;

    bool grounded;
    float groundedTimer;
    float speedAtJump = 0.0f;

    List<Weapon> weapons = new List<Weapon>();
    Dictionary<int, int> ammoInventory = new Dictionary<int, int>();

    void Awake()
    {
        Instance = this;
        
    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

        isPaused = false;
        grounded = true;

        characterController = GetComponent<CharacterController>();

        for (int i = 0; i < startingWeapons.Length; ++i)
        {
            PickupWeapon(startingWeapons[i]);
        }

        for (int i = 0; i < startingAmmo.Length; ++i)
        {
            ChangeAmmo(startingAmmo[i].ammoType, startingAmmo[i].amount);
        }

        currentWeapon = -1;
        ChangeWeapon(0);
        WeaponInfoUI.Instance.IconColorWeapon();

        for (int i = 0; i < startingAmmo.Length; ++i)
        {
            ammoInventory[startingAmmo[i].ammoType] = startingAmmo[i].amount;
        }

        verticalAngle = 0.0f;
        horizontalAngle = transform.localEulerAngles.y;
    }

    void Update()
    {

        bool wasGrounded = grounded;
        bool loosedGrounding = false;

        //definimos nuestra propia conexión a tierra y no usamos la del controlador de personaje ya que el controlador de personaje puede parpadear
        //entre aterrizado/no aterrizado en pequeños pasos y similares. Así que hacemos que el controlador "no esté conectado a tierra" solo si
        //si el controlador del jugador reporta no estar conectado a tierra por al menos .5 segundos;
        if (!characterController.isGrounded)
        {
            if (grounded)
            {
                groundedTimer += Time.deltaTime;
                if (groundedTimer >= 0.5f)
                {
                    loosedGrounding = true;
                    grounded = false;
                }
            }
        }
        else
        {
            groundedTimer = 0.0f;
            grounded = true;
        }

        Speed = 0;
        Vector3 move = Vector3.zero;
        if (!isPaused && !LockControl)
        {

            bool running = weapons[currentWeapon].CurrentState == Weapon.WeaponState.Idle && Input.GetButton("Run");
    
            float actualSpeed = running ? RunningSpeed : PlayerSpeed;



            // Move around with WASD - Muévete con WASD
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            if (move.sqrMagnitude > 1.0f)
                move.Normalize();

            float usedSpeed = grounded ? actualSpeed : speedAtJump;
            
            move = move * usedSpeed * Time.deltaTime;

           

            move = transform.TransformDirection(move);
            characterController.Move(move);

           Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
           Vector3 moveVelocity = moveInput * MouseSensitivity;

            Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                Debug.DrawLine(cameraRay.origin, pointToLook, Color.cyan);

                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }

            weapons[currentWeapon].triggerDown = Input.GetMouseButton(0);

            Speed = move.magnitude / (PlayerSpeed * Time.deltaTime);
        }

        // Fall down / gravity - caida gravedad
        verticalSpeed = verticalSpeed - 10.0f * Time.deltaTime;
        if (verticalSpeed < -10.0f)
            verticalSpeed = -10.0f; // max fall speed - velocidad maxima de caida
        var verticalMove = new Vector3(0, verticalSpeed * Time.deltaTime, 0);
        var flag = characterController.Move(verticalMove);
        if ((flag & CollisionFlags.Below) != 0)
            verticalSpeed = 0;

    }

    public void DisplayCursor(bool display)
    {
        isPaused = display;
        Cursor.lockState = display ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = display;
    }

    void PickupWeapon(Weapon prefab)
    {
   
           var w = Instantiate(prefab, WeaponPosition, false);
            w.name = prefab.name;
            w.transform.localPosition = Vector3.zero;
            w.transform.localRotation = Quaternion.identity;
            w.gameObject.SetActive(false);
            w.PickedUp(this);

            weapons.Add(w);
    }

    void ChangeWeapon(int number)
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
        weapons[currentWeapon].PlayerSelected();
    }

    public int GetAmmo(int ammoType)
    {
        int value = 0;
        ammoInventory.TryGetValue(ammoType, out value);

        return value;
    }


    public void ChangeAmmo(int ammoType, int amount)
    {
        if (!ammoInventory.ContainsKey(ammoType))
            ammoInventory[ammoType] = 0;

        var previous = ammoInventory[ammoType];
        ammoInventory[ammoType] = Mathf.Clamp(ammoInventory[ammoType] + amount, 0, 999);

        if (weapons[currentWeapon].ammoType == ammoType)
        {
            if (previous > 0 && amount > 0)
            {//we just grabbed ammo for a weapon that add non left, so it's disabled right now. Reselect it.
                weapons[currentWeapon].PlayerSelected();
            }
            WeaponInfoUI.Instance.UpdateAmmoAmount(GetAmmo(ammoType));
        }
    }

    //public void PlayFootstep()
    //{
    //    FootstepPlayer.PlayRandom();
    //}
}
