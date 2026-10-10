using UnityEngine;

public class HeartCollectable : Collectable
{
    [SerializeField] private int value = 1;
    [SerializeField] private Health playerHealth;

    protected override void OnCollect(Inventory inventory)
    {
        playerHealth.Heal(value);
    }
}
