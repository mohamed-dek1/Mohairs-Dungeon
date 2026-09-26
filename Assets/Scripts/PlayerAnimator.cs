using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Rigidbody rb;
    SpriteRenderer spriteRenderer;
    public float frameRate = 0.1f;
    [SerializeField] private Sprite[] moveUp;
    [SerializeField] private Sprite[] moveDown;
    [SerializeField] private Sprite[] moveLeft;
    [SerializeField] private Sprite[] moveRight;
    private Sprite[] currentSprites;
    private int currFrame;
    private int maxFrames = 2;
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
        Vector2 dir = rb.linearVelocity;
        bool fastUpdate = false;

        if (dir == Vector2.zero)
            return;

        if (dir.x > 0 && currentSprites != moveRight)
        {
            currentSprites = moveRight;
            fastUpdate = true;
        }
        else if (dir.x < 0 && currentSprites != moveLeft)
        {
            currentSprites = moveLeft;
            fastUpdate = true;
        }
        else if (dir.y > 0 && currentSprites != moveUp)
        {
            currentSprites = moveUp;
            fastUpdate = true;
        }
        else if (dir.y < 0 && currentSprites != moveDown)
        {
            currentSprites = moveDown;
            fastUpdate = true;
        }

        if (fastUpdate)
        {
            currFrame = 0;
            timer = 0.0f;
            spriteRenderer.sprite = currentSprites[currFrame];
            return;
        }

        timer += Time.deltaTime;
        if (timer > frameRate)
        {
            currFrame = (currFrame + 1) % maxFrames;
            timer = 0.0f;
            spriteRenderer.sprite = currentSprites[currFrame];
        }
    }
}
