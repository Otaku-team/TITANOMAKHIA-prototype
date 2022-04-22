using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    #region Variables
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Idle Animations")]
    public string Right_Hand_Idle;
    public string Left_Hand_Idle;

    [Header("One Handed Attack Animations")]
    public string OH_Light_Attack_1;
    public string OH_Light_Attack_2;
    public string OH_Heavy_Attack_1;

    [Header("Stamina cost")]
    public int baseStaminaCost;
    public float lightAttackMultiplier;
    public float heavyAttackMultiplier;
    #endregion
}
