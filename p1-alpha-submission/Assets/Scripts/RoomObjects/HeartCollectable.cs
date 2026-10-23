using UnityEngine;

public class HeartCollectable : Collectable
{
    [SerializeField] private int value = 1;
    [SerializeField] private Health playerHealth;

    private void Awake()
    {
        if (playerHealth == null)
        {
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        }
    }

    protected override void OnCollect(Inventory inventory)
    {
        playerHealth.Heal(value);
    }
}
