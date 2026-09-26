public class SwordShard : Projectile
{
    protected override void OnHitEnemy()
    {
        Destroy(gameObject);
    }
}