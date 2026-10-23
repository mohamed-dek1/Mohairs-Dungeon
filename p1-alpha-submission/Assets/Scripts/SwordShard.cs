public class SwordShard : Projectile
{
    // Shards fly through walls, only enemies stop them
    protected override void OnHitSolid() { }
}