using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    Rigidbody rb;
    ArrowKeyMovement arrowKeyMovement;

    private bool transition = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        arrowKeyMovement = GetComponent<ArrowKeyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider door)
    {
        if (transition)
        {
            return;
        }
    }

    private Vector2 getDirFromTag(string tag)
    {
        switch (tag)
        {
            case "LeftDoor":
                return Vector2.left;
            case "RightDoor":
                return Vector2.right;
            case "UpDoor":
                return Vector2.up;
            case "DownDoor":
                return Vector2.down;
            default:
                return Vector2.zero;
        }
    }
}
