using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoUI : MonoBehaviour
{
    public static WeaponInfoUI Instance { get; private set; }

 
    public Text WeaponClipContent;
    public Text AmmoTypeCount;

    public Image weaponIconColor;
    Color setBYellow;

    void OnEnable()
    {
        Instance = this;
    }

    public void IconColorWeapon()
    {
        setBYellow = weaponIconColor.color;
        setBYellow.b = 0f;
        weaponIconColor.color = setBYellow;
    }

    public void UpdateClipInfo(Weapon weapon)
    {
        WeaponClipContent.text = weapon.ClipContent.ToString();
    }

    public void UpdateAmmoAmount(int amount)
    {
        AmmoTypeCount.text = amount.ToString();
    }
}
