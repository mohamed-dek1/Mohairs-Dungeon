using UnityEngine;

public class Arrow : Projectile
{
    public override void Launch(Vector2 direction)
    {
        base.Launch(direction);
        FaceDirection(direction);
    }
}
