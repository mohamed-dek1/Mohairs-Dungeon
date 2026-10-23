using UnityEngine;

// Basically a Stalfos
public class Goriya : Stalfos
{
    // ==========================
    // Attack settings
    // ==========================
    [SerializeField] private GoriyaBoomerang boomerangPrefab;
    [SerializeField] private float throwChance = 0.1f;         // after each step
    [SerializeField] private float alignedThrowChance = 0.5f;  // after a step towards player

    // ==========================
    // Private fields
    // ==========================
    private GoriyaBoomerang activeBoomerang;

    // ==========================
    // Room setup
    // ==========================
    protected override void RoomExited()
    {
        base.RoomExited();
        RemoveBoomerang();
    }

    protected override void Defeat()
    {
        RemoveBoomerang();
        base.Defeat();
    }

    // ==========================
    // Movement
    // ==========================
    protected override Vector2 ChooseDirection()
    {
        // stands still
        if (activeBoomerang != null)
            return Vector2.zero;

        // walks between throws
        if (moveDir != Vector2.zero && WantsToThrow())
        {
            ThrowBoomerang();
            return Vector2.zero;
        }

        return base.ChooseDirection();
    }

    // ==========================
    // Attack
    // ==========================
    private bool WantsToThrow()
    {
        if (player == null || IsBlocked(moveDir))
            return false;

        if (IsAlignedWithPlayer(out Vector2 dirToPlayer) && dirToPlayer == moveDir)
            return Random.value < alignedThrowChance;

        return Random.value < throwChance;
    }

    private void ThrowBoomerang()
    {
        activeBoomerang = Instantiate(boomerangPrefab, transform.position, Quaternion.identity);
        activeBoomerang.Throw(moveDir, transform);
    }

    private void RemoveBoomerang()
    {
        if (activeBoomerang != null)
            Destroy(activeBoomerang.gameObject);
    }
}
