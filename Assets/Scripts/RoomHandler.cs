using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    [SerializeField] private LockedDoor[] lockedDoors;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(LockedDoor door in lockedDoors)
        {
            door.OpenDoor();
            Debug.Log("Open Door");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
