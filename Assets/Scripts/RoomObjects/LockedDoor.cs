using UnityEngine;

public class LockedDoor : RoomObject
{
    BoxCollider boxcollider;
    SpriteRenderer spriterenderer;
    [SerializeField] private Sprite openDoor;
    [SerializeField] private bool needsKey = true; // false = opens when the room is cleared
    [SerializeField] private LockedDoor[] linkedDoors;

    private bool isOpen = false;

    void Awake()
    {
        boxcollider = GetComponent<BoxCollider>();
        spriterenderer = GetComponent<SpriteRenderer>();
    }

    public override void Init(RoomHandler handler)
    {
        room = handler;

        if (!needsKey)
            room.OnRoomCleared += OpenDoor;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!needsKey || isOpen)
            return;

        if (!collision.gameObject.CompareTag("Player"))
            return;

        Inventory inventory = collision.gameObject.GetComponent<Inventory>();
        if (inventory != null && inventory.UseKey())
            OpenDoor();
    }

    public void OpenDoor()
    {
        if (isOpen)
            return;

        isOpen = true;
        spriterenderer.sprite = openDoor;
        boxcollider.enabled = false;

        foreach (LockedDoor door in linkedDoors)
            door.OpenDoor();
    }
}