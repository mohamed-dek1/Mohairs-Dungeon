using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour
{
    Rigidbody rb;
    SpriteRenderer spriteRenderer;
    public float frameRate = 0.1f;
    public float attackTime = 0.1f;
    [SerializeField] private Sprite[] moveUp;
    [SerializeField] private Sprite[] moveDown;
    [SerializeField] private Sprite[] moveLeft;
    [SerializeField] private Sprite[] moveRight;
    private Sprite[] currentSprites;
    [SerializeField] private ArrowKeyMovement arrowKeyMovement;
    [SerializeField] private Sprite[] attacks;
    [SerializeField] private GameObject[] weapons;
    private int directionFacing = 1; // 0: up, 1: down, 2: left, 3: right
    private int currFrame;
    private int maxFrames = 2;
    private float timer;
    private bool isAttacking = false;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Attack");
            isAttacking = true;
            arrowKeyMovement.SetIsAttacking(true);
            StartCoroutine(PlayAttackAnimation(attackTime));
        }

        if (dir == Vector2.zero || isAttacking)
            return;

        if (dir.x > 0 && currentSprites != moveRight)
        {
            currentSprites = moveRight;
            directionFacing = 3;
            fastUpdate = true;
        }
        else if (dir.x < 0 && currentSprites != moveLeft)
        {
            currentSprites = moveLeft;
            directionFacing = 2;
            fastUpdate = true;
        }
        else if (dir.y > 0 && currentSprites != moveUp)
        {
            currentSprites = moveUp;
            directionFacing = 0;
            fastUpdate = true;
        }
        else if (dir.y < 0 && currentSprites != moveDown)
        {
            currentSprites = moveDown;
            directionFacing = 1;
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

    IEnumerator PlayAttackAnimation(float waitTime)
    {
        Sprite currentSprite = spriteRenderer.sprite;
        spriteRenderer.sprite = attacks[directionFacing];
        GameObject currentWeapon = Instantiate(weapons[directionFacing], transform);
        yield return new WaitForSeconds(waitTime);
        spriteRenderer.sprite = currentSprite;
        Destroy(currentWeapon);
        isAttacking = false;
        arrowKeyMovement.SetIsAttacking(false);
    }
}
