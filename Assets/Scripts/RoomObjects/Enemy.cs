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

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (other.TryGetComponent<Health>(out Health playerHealth))
            playerHealth.TakeDamage(contactDamage);
    }

    public override void Init(RoomHandler handler)
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
        health.OnDeath += Defeat;

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
        gameObject.SetActive(false);
    }
}