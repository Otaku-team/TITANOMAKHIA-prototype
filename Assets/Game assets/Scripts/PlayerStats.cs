using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Variables
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public int staminalevel;
    public int maxStamina;
    public int currentStamina;

    public HealthBar healthBar;
    public StaminaBar staminabar;

    AnimatorHandler animHandler;
    #endregion

    private void Awake()
    {
        animHandler = GetComponentInChildren<AnimatorHandler>();
        staminabar = FindObjectOfType<StaminaBar>();
    }

    private void Start()
    {
        currentHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        maxStamina = SetMaxStaminaFromStaminaLevel();
        currentStamina = maxStamina;
        staminabar.SetMaxStamina(maxStamina);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        return maxHealth = healthLevel * 10;
    }

    private int SetMaxStaminaFromStaminaLevel()
    {
        return maxStamina = staminalevel * 10;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);
        animHandler.PlayTargetAnimation("Damage_01", true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animHandler.PlayTargetAnimation("Dead", true);
        }
    }

    public void TakeStaminaDamage(int damage)
    {
        currentStamina -= damage;
        staminabar.SetCurrentStamina(currentStamina);
    }
}
