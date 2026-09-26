using UnityEngine;

public abstract class Enemy : RoomObject
{
    public int contactDamage = 1;
    public float movementSpeed = 2.0f;

    protected Rigidbody rb;
    protected bool defeated = false;
    protected Vector3 startPosition;

    public override void Init(RoomHandler handler)
    {
        rb = GetComponent<Rigidbody>();

        startPosition = transform.position;
        base.Init(handler);
        room.RegisterEnemy();
    }

    protected override void RoomEntered()
    {
        if (defeated)
            return;

        transform.position = startPosition;
        base.RoomEntered();
    }

    void FixedUpdate()
    {
        Move();
    }

    // Every enemy must write its own movement
    protected abstract void Move();

    public virtual void Defeat()
    {
        defeated = true;
        room.EnemyDefeated();
        gameObject.SetActive(false);
    }
}