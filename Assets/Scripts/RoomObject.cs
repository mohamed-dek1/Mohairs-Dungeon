using UnityEngine;

public class RoomObject : MonoBehaviour
{
    protected RoomHandler room;

    public virtual void Init(RoomHandler handler)
    {
        room = handler;
        room.OnRoomEnter += RoomEntered;
        room.OnRoomExit += RoomExited;
        gameObject.SetActive(false);
    }

    protected virtual void RoomEntered()
    {
        gameObject.SetActive(true);
    }

    protected virtual void RoomExited()
    {
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (room != null)
        {
            room.OnRoomEnter -= RoomEntered;
            room.OnRoomExit -= RoomExited;
        }
    }
}