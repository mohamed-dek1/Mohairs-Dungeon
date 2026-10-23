using UnityEngine;

public class Aquamentus : GridWalk
{
    // ==========================
    // Movement settings
    // ==========================
    [SerializeField] private int walkRange = 2; // tiles it can walk left of its start
    [SerializeField] private float keepWalkingChance = 0.6f;

    // ==========================
    // Attack settings
    // ==========================
    [SerializeField] private Fireball fireballPrefab;
    [SerializeField] private Transform mouth;
    [SerializeField] private float spreadAngle = 20.0f; // degrees between the middle fireball and each outer one
    [SerializeField] private float minFireDelay = 1.5f;
    [SerializeField] private float maxFireDelay = 3.0f;
    [SerializeField] private float mouthOpenTime = 0.5f;

    // ==========================
    // Sprites
    // ==========================
    [SerializeField] private Sprite[] walkSprites;
    [SerializeField] private Sprite[] mouthOpenSprites;

    // ==========================
    // Private fields
    // ==========================
    private EnemyAnimator enemyAnimator;
    private float fireTimer;
    private bool mouthOpen;

    // ==========================
    // Room setup
    // ==========================
    protected override void RoomEntered()
    {
        base.RoomEntered();

        enemyAnimator = GetComponent<EnemyAnimator>();
        SetMouthOpen(false);
        fireTimer = Random.Range(minFireDelay, maxFireDelay);
    }

    // ==========================
    // Movement
    // ==========================
    protected override void Move()
    {
        base.Move();
        UpdateAttack();
    }

    protected override Vector2 ChooseDirection()
    {
        // paces back and forth, sometimes turning around early
        if (moveDir != Vector2.zero && Random.value < keepWalkingChance)
            return moveDir;

        return Random.value < 0.5f ? Vector2.left : Vector2.right;
    }

    // Only walks left and right, and never further than walkRange tiles from its start
    protected override bool IsBlocked(Vector2 dir)
    {
        if (dir.y != 0f)
            return true;

        float nextX = targetTile.x + dir.x * tileSize;
        float leftEdge = startPosition.x - walkRange * tileSize;

        if (nextX < leftEdge - 0.01f || nextX > startPosition.x + 0.01f)
            return true;

        return base.IsBlocked(dir);
    }

    // ==========================
    // Attack
    // ==========================
    private void UpdateAttack()
    {
        fireTimer -= Time.fixedDeltaTime;

        // open the mouth a moment early so the player can see the attack coming
        if (!mouthOpen && fireTimer <= mouthOpenTime)
            SetMouthOpen(true);

        if (fireTimer > 0f)
            return;

        FireSpread();
        SetMouthOpen(false);
        fireTimer = Random.Range(minFireDelay, maxFireDelay);
    }

    // One fireball straight at the player and one angled to either side of it
    private void FireSpread()
    {
        if (player == null)
            return;

        Vector3 spawnPosition = mouth != null ? mouth.position : transform.position;
        Vector2 toPlayer = (Vector2)player.position - (Vector2)spawnPosition;

        LaunchFireball(spawnPosition, toPlayer);
        LaunchFireball(spawnPosition, Quaternion.Euler(0f, 0f, spreadAngle) * toPlayer);
        LaunchFireball(spawnPosition, Quaternion.Euler(0f, 0f, -spreadAngle) * toPlayer);
    }

    private void LaunchFireball(Vector3 position, Vector2 direction)
    {
        Fireball fireball = Instantiate(fireballPrefab, position, Quaternion.identity);
        fireball.Launch(direction);
    }

    // ==========================
    // Helpers
    // ==========================
    private void SetMouthOpen(bool open)
    {
        mouthOpen = open;

        if (enemyAnimator != null)
            enemyAnimator.SetMoveDown(open ? mouthOpenSprites : walkSprites);
    }
}
