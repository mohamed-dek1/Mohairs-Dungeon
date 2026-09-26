using UnityEngine;

public class Projectile : Weapon
{
    [SerializeField] protected float speed = 8f;
    [SerializeField] protected float maxLife = 2f;

    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Launch(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, maxLife);
    }
}