using UnityEngine;

public enum DirectionMode { None, LeftRight, UpDownSide }

public class EnemyAnimator : MonoBehaviour
{
    // ==========================
    // Public fields
    // ==========================
    public DirectionMode directionMode = DirectionMode.None;
    public float frameRate = 0.15f;

    // ==========================
    // Serialized fields
    // ==========================
    [SerializeField] private Sprite[] moveDown;  // None uses only this
    [SerializeField] private Sprite[] moveUp;    // UpDownSide only
    [SerializeField] private Sprite[] moveSide;  // LeftRight and UpDownSide
    [SerializeField] private bool spritesFaceRight = true;

    // ==========================
    // Private fields
    // ==========================
    private Rigidbody rb;
    private SpriteRenderer spriteRenderer;
    private Sprite[] currentSprites;
    private Direction facing = Direction.Down;
    private bool facingRight = true;
    private int currFrame;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currFrame = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFacing();

        if (directionMode == DirectionMode.None)
            SetDirection(moveDown);
        else if (directionMode == DirectionMode.LeftRight)
            SetDirection(moveSide);
        else if (facing == Direction.Up)
            SetDirection(moveUp);
        else if (facing == Direction.Down)
            SetDirection(moveDown);
        else
            SetDirection(moveSide);

        if (IsSideView())
            spriteRenderer.flipX = IsFacingFlipped();

        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            timer = 0.0f;

            if (currentSprites.Length == 1 && !IsSideView())
            {
                // only one frame: animate by flipping it back and forth
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
            else
            {
                currFrame = (currFrame + 1) % currentSprites.Length;
                spriteRenderer.sprite = currentSprites[currFrame];
            }
        }
    }

    private void SetDirection(Sprite[] sprites)
    {
        if (currentSprites == sprites)
            return;

        currentSprites = sprites;
        currFrame = 0;
        timer = 0f;
        spriteRenderer.flipX = false;
        spriteRenderer.sprite = currentSprites[currFrame];
    }

    private void UpdateFacing()
    {
        Vector2 velocity = rb.linearVelocity;
        if (velocity.sqrMagnitude < 0.001f)
            return;

        if (velocity.x > 0.01f)
            facingRight = true;
        else if (velocity.x < -0.01f)
            facingRight = false;

        if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.y))
        {
            if (velocity.x > 0)
                facing = Direction.Right;
            else
                facing = Direction.Left;
        }
        else
        {
            if (velocity.y > 0)
                facing = Direction.Up;
            else
                facing = Direction.Down;
        }
    }

    private bool IsSideView()
    {
        if (directionMode == DirectionMode.LeftRight)
            return true;

        if (directionMode == DirectionMode.UpDownSide)
            return facing == Direction.Left || facing == Direction.Right;

        return false;
    }

    // Flip side sprites so they face the way the enemy is moving
    private bool IsFacingFlipped()
    {
        if (facingRight)
            return !spritesFaceRight;
        else
            return spritesFaceRight;
    }
}