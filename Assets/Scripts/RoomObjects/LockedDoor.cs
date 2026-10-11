using UnityEngine;

public class LockedDoor : RoomObject
{
    BoxCollider boxcollider;
    SpriteRenderer spriterenderer;
    [SerializeField] private Sprite openDoor;
    [SerializeField] private bool needsKey = true; // false = opens when the room is cleared
    [SerializeField] private bool opensWhenRoomCleared = false; // false = opens when the puzzle is solved
    [SerializeField] private bool isBlockPositionCorrect = false;
    [SerializeField] private LockedDoor[] linkedDoors;

    private bool isOpen = false;

    GameManager gameManager;

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        boxcollider = GetComponent<BoxCollider>();
        spriterenderer = GetComponent<SpriteRenderer>();
    }

    public override void Init(RoomHandler handler)
    {
        room = handler;

        if (!needsKey && opensWhenRoomCleared)
            room.OnRoomCleared += OpenDoor;
    }

    public void SetBlockPositionCorrect(bool value)
    {
        isBlockPositionCorrect = value;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isOpen || opensWhenRoomCleared)
            return;

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Player Hurtbox"))
        {
            if (needsKey)
            {
                Inventory inventory = collision.gameObject.GetComponent<Inventory>();
                if (inventory != null && inventory.UseKey())
                    OpenDoor();
            }

            Debug.Log("Player collided with locked door!");
            Debug.Log("isBlockPositionCorrect: " + isBlockPositionCorrect);
            Debug.Log("opensWhenRoomCleared: " + opensWhenRoomCleared);
            if (isBlockPositionCorrect)
            {
                Debug.Log("Block is in the correct position, opening door!");
                OpenDoor();
                return;
            }
        }
    }

    public void OpenDoor()
    {
        if (isOpen)
            return;

        gameManager.PlaySFX(gameManager.doorOpen);

        isOpen = true;
        spriterenderer.sprite = openDoor;
        boxcollider.enabled = false;

        foreach (LockedDoor door in linkedDoors)
            door.OpenDoor();
    }
}