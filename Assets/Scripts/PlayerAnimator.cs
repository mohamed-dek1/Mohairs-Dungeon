using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public float frameRate = 0.1f;
    [SerializeField] private Sprite[] moveUp;
    [SerializeField] private Sprite[] moveDown;
    [SerializeField] private Sprite[] moveLeft;
    [SerializeField] private Sprite[] moveRight;
    private int currFrame;
    private int maxFrames = 2;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currFrame = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h == 0 && v == 0)
        {
            return;
        }

        timer += Time.deltaTime;

        if (timer > frameRate)
        {
            currFrame = (currFrame + 1) % maxFrames;
            timer = 0.0f;
            if (h > 0)
            {
                spriteRenderer.sprite = moveRight[currFrame];
            }
            else if (h < 0)
            {
                spriteRenderer.sprite = moveLeft[currFrame];
            }
            else if (v > 0)
            {
                spriteRenderer.sprite = moveUp[currFrame];
            }
            else if (v < 0)
            {
                spriteRenderer.sprite = moveDown[currFrame];
            }
        }
    }
}
