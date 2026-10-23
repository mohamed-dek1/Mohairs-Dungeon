using UnityEngine;

public class Gel : GridWalk
{
    // ==========================
    // Public fields
    // ==========================
    public float keepStraightChance = 0.3f;
    public float minWaitTime = 0.5f;
    public float maxWaitTime = 1.5f;

    // ==========================
    // Movement
    // ==========================
    protected override Vector2 ChooseDirection()
    {
        Wait(Random.Range(minWaitTime, maxWaitTime));
        return RandomDirection(keepStraightChance);
    }

    protected override void OnHitPlayer()
    {
        Vector2 toPlayer = (Vector2)player.position - (Vector2)rb.position;

        if (Vector2.Dot(moveDir, toPlayer) > 0)
            ReverseDirection();
    }

    protected override void OnHitGridWalker(GridWalk other)
    {
        ReverseDirection();
    }
}