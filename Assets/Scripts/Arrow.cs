using UnityEngine;

public class Arrow : Projectile
{
    public override void Launch(Vector2 direction)
    {
        base.Launch(direction);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        rb.freezeRotation = true;
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
