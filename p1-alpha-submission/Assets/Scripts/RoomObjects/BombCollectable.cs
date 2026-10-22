using UnityEngine;

public class BombCollectable : Collectable
{
    protected override void OnCollect(Inventory inventory)
    {
        inventory.AddBombs(1);
    }
}
