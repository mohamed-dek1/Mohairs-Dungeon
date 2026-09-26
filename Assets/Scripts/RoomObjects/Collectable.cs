using UnityEngine;

public abstract class Collectable : RoomObject
{
    protected bool collected = false;

    protected override void RoomEntered()
    {
        if (collected)
            return;

        base.RoomEntered();
    }

    public void Collect(Inventory inventory)
    {
        if (collected)
            return;

        collected = true;
        OnCollect(inventory);
        gameObject.SetActive(false);
    }

    // Each collectible decides what picking it up does
    protected abstract void OnCollect(Inventory inventory);
}