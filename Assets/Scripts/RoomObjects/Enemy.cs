using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class Enemy : RoomObject
{
    // ==========================
    // Public fields
    // ==========================
    public int contactDamage = 1;
    public float movementSpeed = 2.0f;

    // ==========================
    // Protected fields
    // ==========================
    protected Rigidbody rb;
    protected Health health;
    protected Vector3 startPosition;
    protected bool defeated = false;

    GameManager gameManager;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player Hurtbox"))
        {
            Health playerHealth = other.GetComponentInParent<Health>();
            if (playerHealth != null)
                playerHealth.TakeDamage(contactDamage, transform.position);

            OnHitPlayer();
            return;
        }

        GridWalk otherEnemy = other.GetComponentInParent<GridWalk>();

        if (otherEnemy != null && otherEnemy != this)
        {
            OnHitGridWalker(otherEnemy);
        }
    }

    protected virtual void OnHitPlayer()
    {
    }
    protected virtual void OnHitGridWalker(GridWalk other)
    {
    }

    public override void Init(RoomHandler handler)
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        health.OnDeath += Defeat;
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        startPosition = transform.position;
        base.Init(handler);
        room.RegisterEnemy();
    }

    protected override void RoomEntered()
    {
        if (defeated)
            return;

        transform.position = startPosition;
        health.ResetHealth();
        base.RoomEntered();
    }

    void FixedUpdate()
    {
        Move();
    }

    protected abstract void Move();

    protected virtual void Defeat()
    {
        defeated = true;
        room.EnemyDefeated();
        gameManager.PlaySFX(gameManager.enemyDeath);
        
        gameObject.SetActive(false);
    }
}