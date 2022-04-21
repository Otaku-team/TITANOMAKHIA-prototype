using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlotManager : MonoBehaviour
{
    #region Variables
    WeaponHolderSlot leftHandSlot;
    WeaponHolderSlot rightHandSlot;

    public DamageCollider leftHandDamageCollider;
    public DamageCollider rightHandDamageCollider;

    public WeaponItem attackingWeapon;

    Animator animator;
    QuickSlotsUI quickSlotsUi;
    public PlayerStats playerStats;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        quickSlotsUi = FindObjectOfType<QuickSlotsUI>();

        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

        foreach (var weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.isRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
        }
    }

    public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            #region Handle Left Weapon Idle Aninations
            leftHandSlot.LoadWeaponModel(weaponItem);
            LoadLeftWeaponDamageCollider();
            quickSlotsUi.UpdateWeaponQuickSlotsUI(true, weaponItem);

            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.Left_Hand_Idle, .2f);
            }
            else
            {
                animator.CrossFade("Left Arm Empty", .2f);
            }
            #endregion
        }
        else
        {
            #region Handle Right Weapon Idle Aninations
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();
            quickSlotsUi.UpdateWeaponQuickSlotsUI(false, weaponItem);

            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.Right_Hand_Idle, .2f);
            }
            else
            {
                animator.CrossFade("Left Arm Empty", .2f);
            }
            #endregion
        }
    }

    #region Handle Weapon's damage DamageCollider
    private void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    private void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void OpenRightDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void OpenLeftDamageCollider()
    {
        leftHandDamageCollider.EnableDamageCollider();
    }

    public void CloseRightDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    public void CloseLeftDamageCollider()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }
    #endregion
    #region Handle Weapon's Stamina Drainage
    public void DrainStaminaLightAttack()
    {
        if (playerStats == null) return;
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStaminaCost * attackingWeapon.lightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        if (playerStats == null) return;
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStaminaCost * attackingWeapon.heavyAttackMultiplier));
    }
    #endregion
}
