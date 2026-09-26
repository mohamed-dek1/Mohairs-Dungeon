using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // ==========================
    // Public fields
    // ==========================
    public float frameRate = 0.1f;

    // ==========================
    // Serialized fields
    // ==========================
    [SerializeField] private Sprite[] moveUp;
    [SerializeField] private Sprite[] moveDown;
    [SerializeField] private Sprite[] moveLeft;
    [SerializeField] private Sprite[] moveRight;
    [SerializeField] private Sprite[] attacks; // Up, Down, Left, Right
    [SerializeField] private Sprite[] playerStunSprite; // Up, Down, Left, Right
    [SerializeField] private ArrowKeyMovement arrowKeyMovement;

    // ==========================
    // Private fields
    // ==========================
    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;
    private Sprite[] currentSprites;
    private int currFrame;
    private int maxFrames = 2;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSprites = moveDown;
        currFrame = 0;
    }

    void Update()
    {
        Direction facing = arrowKeyMovement.GetDirectionFacing();

        // Stun State Priority
        if (arrowKeyMovement.GetIsStunned())
        {
            spriteRenderer.sprite = playerStunSprite[(int)facing];
            return;
        }

        // Attack State Priority
        if (arrowKeyMovement.GetIsAttacking())
        {
            spriteRenderer.sprite = attacks[(int)facing];
            currentSprites = null;
            return;
        }

        // Movement State
        if (facing == Direction.Right) {
            SetDirection(moveRight);
        }
        else if (facing == Direction.Left) {
            SetDirection(moveLeft);
        }
        else if (facing == Direction.Up) {
            SetDirection(moveUp);
        }
        else if (facing == Direction.Down) {
            SetDirection(moveDown);
        }

        Vector2 velocity = rb.linearVelocity;
        if (velocity.sqrMagnitude < 0.001f)
            return;

        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            currFrame = (currFrame + 1) % maxFrames;
            timer = 0.0f;
            spriteRenderer.sprite = currentSprites[currFrame];
        }
    }

    private void SetDirection(Sprite[] sprites)
    {
        if (currentSprites == sprites)
            return;

        currentSprites = sprites;
        currFrame = 0;
        timer = 0f;
        spriteRenderer.sprite = currentSprites[currFrame];
    }
}