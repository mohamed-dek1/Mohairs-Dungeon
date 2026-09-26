using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    
    public int currentHealth;
    public UnityEvent onTakeDamage;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    [ContextMenu("Take Damage")]
    public void TakeDamage()
    {
        int damage = 10;
        currentHealth -= damage;

        onTakeDamage?.Invoke();

        if (currentHealth <= 0)
        {
            if (gameObject.CompareTag("Player"))
            {
                Debug.Log("Player has died.");
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                Debug.Log($"{gameObject.name} has died.");
                Destroy(gameObject);
            }
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
}
