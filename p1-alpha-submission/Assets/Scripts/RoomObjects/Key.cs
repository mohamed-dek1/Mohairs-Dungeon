using UnityEngine;

public class Key : Collectable
{
    protected override void OnCollect(Inventory inventory)
    {
        inventory.AddKey();
    }
}