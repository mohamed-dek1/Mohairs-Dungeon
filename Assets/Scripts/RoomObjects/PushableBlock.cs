using UnityEngine;
using System.Collections;

public class PushableBlock : RoomObject
{
    private bool beenPushed = false;
    [SerializeField] private float pushDelay = 0.5f;     // Time player must continuously push
    [SerializeField] private float moveSpeed = 2f;       // Speed at which block slides
    [SerializeField] private float tileSize = 1.0f;      // Grid unit size (0.5 to match ArrowKeyMovement)
    [SerializeField] LockedDoor lockedDoor;
    [SerializeField] Vector2 correctPosition;
    [SerializeField] private bool isPuzzleBlock = false;

    private float pushTimer = 0f;
    private Vector2 pushDirection = Vector2.zero;
    private bool isMoving = false;
    
    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (beenPushed || isMoving) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 contactNormal = collision.contacts[0].normal;

            Vector2 currentPushDir = contactNormal;

            if (Mathf.Abs(currentPushDir.x) > Mathf.Abs(currentPushDir.y))
            {
                currentPushDir = new Vector2(Mathf.Sign(currentPushDir.x), 0);
            }
            else
            {
                currentPushDir = new Vector2(0, Mathf.Sign(currentPushDir.y));
            }

            if (IsPlayerPressingDirection(currentPushDir))
            {
                if (currentPushDir == pushDirection)
                {
                    pushTimer += Time.deltaTime;

                    if (pushTimer >= pushDelay)
                    {
                        StartCoroutine(MoveBlock(pushDirection));
                    }
                }
                else
                {
                    // Reset timer if direction changed
                    pushDirection = currentPushDir;
                    pushTimer = 0f;
                }
            }
            else
            {
                pushDirection = currentPushDir;
                pushTimer = 0f;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pushTimer = 0f;
            pushDirection = Vector2.zero;
        }
    }

    private bool IsPlayerPressingDirection(Vector2 direction)
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        if (direction == Vector2.right && inputX > 0) return true;
        if (direction == Vector2.left && inputX < 0) return true;
        if (direction == Vector2.up && inputY > 0) return true;
        if (direction == Vector2.down && inputY < 0) return true;

        return false;
    }

    private IEnumerator MoveBlock(Vector2 direction)
    {
        isMoving = true;
        beenPushed = true;

        gameManager.PlaySFX(gameManager.secretFound);

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(direction.x, direction.y, 0f) * tileSize;

        float progress = 0f;
        while (progress < 1f)
        {
            progress += Time.deltaTime * moveSpeed;
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
            yield return null;
        }

        transform.position = targetPosition;

        if (isPuzzleBlock && lockedDoor != null)
        {
            Debug.Log("Checking block position: " + transform.position + " vs correct position: " + correctPosition);
            if ((Vector2)transform.position == correctPosition)
            {
                Debug.Log("Block is in the correct position!");
                lockedDoor.SetBlockPositionCorrect(true);
            }
        }
        isMoving = false;
    }
}
