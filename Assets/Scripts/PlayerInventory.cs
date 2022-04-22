using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    #region Variables
    WeaponSlotManager weaponSlotManager;
    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;
    public WeaponItem unarmedWeapon;

    public List<WeaponItem> weaponsInRightHandSlot;
    public List<WeaponItem> weaponsInLeftHandSlots;

    public int currentRightWeaponIndex = -1;
    public int currentLeftWeaponIndex = -1;

    public List<WeaponItem> weaponsInventory;
    #endregion

    private void Awake()
    {
        weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
    }

    private void Start()
    {
        weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
        weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
    }
}
