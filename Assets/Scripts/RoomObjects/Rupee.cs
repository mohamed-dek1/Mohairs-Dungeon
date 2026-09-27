using UnityEngine;

public class Rupee : Collectable
{
    [SerializeField] private int value = 1;

    protected override void OnCollect(Inventory inventory)
    {
        inventory.AddCoins(value);
    }
}