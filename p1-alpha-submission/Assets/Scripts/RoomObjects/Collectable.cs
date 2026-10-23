using UnityEngine;

public abstract class Collectable : RoomObject
{
    protected bool collected = false;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

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
        gameManager.PlaySFX(gameManager.itemPickup);
        gameObject.SetActive(false);
    }

    // Each collectible decides what picking it up does
    protected abstract void OnCollect(Inventory inventory);
}