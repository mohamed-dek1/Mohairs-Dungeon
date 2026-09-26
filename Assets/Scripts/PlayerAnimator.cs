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
    [SerializeField] private GameObject swordBeamPrefab;
    private int swordOffset = 0;
    private int beamSwordOffset = 1;
    private int directionFacing = 1; // 0: up, 1: down, 2: left, 3: right
    private int currFrame;
    private int maxFrames = 2;
    private float timer;
    private bool isAttacking = false;
    private GameObject activeBeam;

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

        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
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

        GameObject currentWeapon = Instantiate(weapons[directionFacing + (4 * swordOffset)], transform);

        if (activeBeam == null)
        {
            FireSwordBeam();
        }

        yield return new WaitForSeconds(waitTime);

        Destroy(currentWeapon);
        spriteRenderer.sprite = currentSprite;
        isAttacking = false;
        arrowKeyMovement.SetIsAttacking(false);
    }

    private void FireSwordBeam()
    {
        Vector2 beamSwordVelocity;
        switch (directionFacing)
        {
            case 0:
                beamSwordVelocity = Vector2.up;
                break;
            case 1:
                beamSwordVelocity = Vector2.down;
                break;
            case 2:
                beamSwordVelocity = Vector2.left;
                break;
            case 3:
                beamSwordVelocity = Vector2.right;
                break;
            default:
                beamSwordVelocity = Vector2.zero;
                break;
        }

        activeBeam = Instantiate(swordBeamPrefab, transform.position, Quaternion.identity);
        
        if (activeBeam.TryGetComponent<SwordBeam>(out var beam))
        {
            beam.Launch(beamSwordVelocity);
        }
    }
}
