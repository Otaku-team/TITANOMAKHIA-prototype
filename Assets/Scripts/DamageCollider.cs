using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    #region Variables
    public int currentWeaponDamage = 25;

    Collider damageCollider;
    #endregion

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.isTrigger = true;
        damageCollider.enabled = false;
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if (playerStats)
            {
                playerStats.TakeDamage(currentWeaponDamage);
            }
        }

        if (other.tag == "Enemy")
        {
            EnemyStats enemyStats = other.GetComponent<EnemyStats>();

            if (enemyStats)
            {
                enemyStats.TakeDamage(currentWeaponDamage);
            }
        }
    }
}
