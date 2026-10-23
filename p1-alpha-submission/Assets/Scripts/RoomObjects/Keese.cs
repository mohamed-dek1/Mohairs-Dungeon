using UnityEngine;

public class Keese : Enemy
{
    // ==========================
    // Public fields
    // ==========================
    [SerializeField] private float startingSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 3.0f;
    [SerializeField] private float acceleration = 0.5f;

    [SerializeField] private float minDirectionTime = 0.5f;
    [SerializeField] private float maxDirectionTime = 1.5f;
    [SerializeField] private float pauseChance = 0.15f;
    [SerializeField] private float minPauseTime = 0.2f;
    [SerializeField] private float maxPauseTime = 0.6f;

    // ==========================
    // Private fields
    // ==========================
    private Vector2 moveDir;
    private float currentSpeed;
    private float directionTimer;
    private float pauseTimer;
    private EnemyAnimator enemyAnimator;
    private float baseFrameRate;

    // ==========================
    // Room setup
    // ==========================
    protected override void RoomEntered()
    {
        base.RoomEntered();

        currentSpeed = startingSpeed;
        moveDir = RandomDirection();
        directionTimer = Random.Range(minDirectionTime, maxDirectionTime);
        pauseTimer = 0f;

        enemyAnimator = GetComponent<EnemyAnimator>();
        if (enemyAnimator != null)
            baseFrameRate = enemyAnimator.frameRate;

    }
    // ==========================
    // Movement
    // ==========================
    protected override void Move()
    {
        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.fixedDeltaTime;
            return;
        }

        directionTimer -= Time.fixedDeltaTime;

        if (directionTimer <= 0f)
        {
            if (Random.value < pauseChance)
            {
                pauseTimer = Random.Range(minPauseTime, maxPauseTime);
                currentSpeed = startingSpeed;
                directionTimer = Random.Range(minDirectionTime, maxDirectionTime);
                return;
            }

            ChooseDirection();
        }

        currentSpeed = Mathf.MoveTowards(
            currentSpeed,
            maxSpeed,
            acceleration * Time.fixedDeltaTime
        );

        Vector2 nextPosition =
            (Vector2)rb.position + moveDir * currentSpeed * Time.fixedDeltaTime;

        if (IsBlocked(nextPosition))
        {
            ChooseDirection();
            return;
        }

        float speedRatio = currentSpeed / maxSpeed;
        enemyAnimator.SetFrameRate(baseFrameRate / Mathf.Max(speedRatio, 0.1f));

        rb.MovePosition(nextPosition);
    }

    // ==========================
    // Helpers
    // ==========================
    private bool IsBlocked(Vector2 position)
    {
        Collider[] hits = Physics.OverlapBox(
            new Vector3(position.x, position.y, rb.position.z),
            new Vector3(0.3f, 0.3f, 0.3f)
        );

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("wall") ||
                hit.GetComponent<LockedDoor>() != null)
            {
                return true;
            }
        }

        return false;
    }

    private Vector2 RandomDirection()
    {
        Vector2[] directions =
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right,
            new Vector2(1f, 1f),
            new Vector2(1f, -1f),
            new Vector2(-1f, 1f),
            new Vector2(-1f, -1f)
        };

        return directions[Random.Range(0, directions.Length)];
    }

    private void ChooseDirection()
    {
        moveDir = RandomDirection();
        directionTimer = Random.Range(minDirectionTime, maxDirectionTime);
    }
}