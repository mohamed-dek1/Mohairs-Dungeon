using UnityEngine;
using System.Collections;

public class RoomTransition : MonoBehaviour
{
    public float roomWidth = 16f;
    public float roomHeight = 11f;
    public float transitionTime = 1.0f;
    public float walkSpeed = 2.0f;

    Rigidbody rb;
    private ArrowKeyMovement arrowKeyMovement;
    bool transitioning = false;

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

    void OnTriggerEnter(Collider other)
    {
        if (transitioning)
            return;
        
        Vector2 direction;
        switch (other.tag)
        {
            case "LeftDoor":
                direction = Vector2.left;
                break;
            case "RightDoor":
                direction = Vector2.right;
                break;
            case "UpDoor":
                direction = Vector2.up;
                break;
            case "DownDoor":
                direction =Vector2.down;
                break;
            default:
                return;
        }

        if (Vector2.Dot(rb.linearVelocity, direction) <= 0)
            return;

        StartCoroutine(Transition(direction));
    }

    IEnumerator Transition(Vector2 direction)
    {
        transitioning = true;
        arrowKeyMovement.enabled = false;

        Transform cam = Camera.main.transform;
        Vector3 camStart = cam.position;
        Vector3 camEnd = camStart + new Vector3(direction.x * roomWidth, direction.y * roomHeight, 0);

        float t = 0f;
        while (t < transitionTime)
        {
            t += Time.deltaTime;
            cam.position = Vector3.Lerp(camStart, camEnd, t / transitionTime);
            rb.linearVelocity = direction * walkSpeed;
            yield return null;
        }

        cam.position = camEnd;
        rb.linearVelocity = Vector2.zero;
        arrowKeyMovement.enabled = true;
        transitioning = false;
    }
}
