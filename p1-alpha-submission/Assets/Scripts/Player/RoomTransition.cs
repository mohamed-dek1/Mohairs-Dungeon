using UnityEngine;
using System.Collections;

public class RoomTransition : MonoBehaviour
{
    public float roomWidth = 16f;
    public float roomHeight = 11f;
    public float transitionTime = 1.0f;
    public float walkSpeed = 2.0f;

    [SerializeField] private Transform bowRoomPlayerSpawn;
    [SerializeField] private Transform bowRoomCameraSpawn;
    [SerializeField] private Transform mainRoomPlayerSpawn;
    [SerializeField] private Transform mainRoomCameraSpawn;

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;

    private bool goingToBowRoom = false;
    private bool leavingBowRoom = false;

    Rigidbody rb;
    private ArrowKeyMovement arrowKeyMovement;
    bool transitioning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        arrowKeyMovement = GetComponent<ArrowKeyMovement>();

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
        }
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
                Debug.Log("Transitioning to the left room.");
                direction = Vector2.left;
                break;
            case "RightDoor":
                Debug.Log("Transitioning to the right room.");
                direction = Vector2.right;
                break;
            case "UpDoor":
                Debug.Log("Transitioning to the upper room.");
                direction = Vector2.up;
                break;
            case "DownDoor":
                Debug.Log("Transitioning to the lower room.");
                direction =Vector2.down;
                break;
            case "BowRoomEnter":
                Debug.Log("Entering Bow Room");
                direction = Vector2.right;
                goingToBowRoom = true;
                break;
            case "BowRoomExit":
                Debug.Log("Exiting Bow Room");
                direction = Vector2.up;
                leavingBowRoom = true;
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
        Debug.Log("Transitioning to new room in direction: " + direction);
        transitioning = true;
        arrowKeyMovement.enabled = false;
        rb.linearVelocity = Vector2.zero;

        if (goingToBowRoom || leavingBowRoom)
        {
            Debug.Log("Fading out for room transition.");
            yield return StartCoroutine(Fade(0f, 1f, fadeDuration));

            Transform cam = Camera.main.transform;

            if (goingToBowRoom)
            {
                if (bowRoomPlayerSpawn != null) transform.position = bowRoomPlayerSpawn.position;
                if (bowRoomCameraSpawn != null) cam.position = bowRoomCameraSpawn.position;

                // GameManager.Instance?.EnterBowRoom();
                goingToBowRoom = false;
            }
            else if (leavingBowRoom)
            {
                if (mainRoomPlayerSpawn != null) transform.position = mainRoomPlayerSpawn.position;
                if (mainRoomCameraSpawn != null) cam.position = mainRoomCameraSpawn.position;

                // GameManager.Instance?.ExitBowRoom();
                leavingBowRoom = false;
            }

            Debug.Log("Fading in after room transition.");
            yield return StartCoroutine(Fade(1f, 0f, fadeDuration));
        }

        else
        {
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
        }

        rb.linearVelocity = Vector2.zero;
        arrowKeyMovement.enabled = true;
        transitioning = false;
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        if (fadeCanvasGroup == null) yield break;

        float elapsed = 0f;
        fadeCanvasGroup.alpha = startAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        fadeCanvasGroup.alpha = endAlpha;
    }
}
