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

    // Turns an upward pointing sprite to face the way it's flying
    protected void FaceDirection(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        rb.freezeRotation = true;
    }

    // Projectiles are used up by whatever they hit
    protected override void OnHitEnemy()
    {
        Destroy(gameObject);
    }

    protected override void OnHitSolid()
    {
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}