using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    #region Variables
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    Animator animator;
    #endregion

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        currentHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }

    private int SetMaxHealthFromHealthLevel()
    {
        return maxHealth = healthLevel * 10;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.Play("Damage_01");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.Play("Dead");
        }
    }
}