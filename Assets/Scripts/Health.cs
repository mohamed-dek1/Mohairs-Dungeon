using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

public class Health : MonoBehaviour
{
    public event Action OnDeath;
    public UnityEvent onTakeDamage;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float invincibleTime = 0.0f;

    private int currentHealth;
    private float invincibleUntil = 0.0f;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (GameManager.god_mode)
        {
            if (tag == "Player")
            {
                currentHealth = maxHealth;
            }
        }
    } 

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0)
            return;
        if (tag == "Player" && GameManager.god_mode)
            return;
        if (Time.time < invincibleUntil)
            return;
        invincibleUntil = Time.time + invincibleTime;

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
        invincibleUntil = 0.0f;
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
