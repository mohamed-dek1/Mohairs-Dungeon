using UnityEngine;

public class SwordBeam : Projectile
{
    [SerializeField] private SwordShard[] shardPrefabs; // NE, NW, SW, SE

    private bool exploded = false;

    public override void Launch(Vector2 direction)
    {
        base.Launch(direction);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        rb.freezeRotation = true;
    }

    protected override void OnHitEnemy()
    {
        Explode();
    }

    protected override void OnHitSolid()
    {
        Explode();
    }

    private void Explode()
    {
        if (exploded)
            return;

        exploded = true;

        Vector2[] directions = new Vector2[]
        {
            new Vector2(1, 1),   // NE
            new Vector2(-1, 1),  // NW
            new Vector2(-1, -1), // SW
            new Vector2(1, -1)   // SE
        };

        for (int i = 0; i < directions.Length; i++)
        {
            SwordShard shard = Instantiate(shardPrefabs[i], transform.position, Quaternion.identity);
            shard.Launch(directions[i]);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}