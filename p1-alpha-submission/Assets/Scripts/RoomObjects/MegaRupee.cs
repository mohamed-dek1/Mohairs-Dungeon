using UnityEngine;

public class MegaRupee : Collectable
{
    [SerializeField] private int value = 10;

    protected override void OnCollect(Inventory inventory)
    {
        inventory.AddCoins(value);
    }
}
