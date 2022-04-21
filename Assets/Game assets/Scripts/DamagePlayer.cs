using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    #region Variables
    public int damage;
    #endregion

    private void OnTriggerEnter(Collider other)
    {
        var playerStats = other.GetComponent<PlayerStats>();
        playerStats.TakeDamage(damage);
    }
}
