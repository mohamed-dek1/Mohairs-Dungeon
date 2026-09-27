using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

public class Health : MonoBehaviour
{
    public event Action OnDeath;
    public UnityEvent onTakeDamage;

    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0)
            return;

        if (tag == "Player" && GameManager.god_mode)
            return;

        currentHealth -= amount;

        if (onTakeDamage != null)
            onTakeDamage.Invoke();

        if (currentHealth <= 0)
        {
            currentHealth = 0;

            if (OnDeath != null)
                OnDeath();
        }
    }


    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return maxHealth;
    }

    [ContextMenu("Take Damage")]
    private void TestDamage()
    {
        TakeDamage(50);
    }
}
