
using UnityEngine;

public class BladeTrap : Enemy
{
    // ==========================
    // Settings
    // ==========================
    [SerializeField] private float chargeSpeed = 6.0f;
    [SerializeField] private float returnSpeed = 1.5f;
    [SerializeField] private float returnPause = 0.5f;

    // ==========================
    // Private fields
    // ==========================
    private enum TrapState { Waiting, Charging, Pausing, Returning }
    private float pauseTimer;

    private TrapState state = TrapState.Waiting;
    private Vector2 homePosition;
    private Vector2 moveDir;

    // ==========================
    // Room setup
    // ==========================
    protected override void RoomEntered()
    {
        base.RoomEntered();

        homePosition = startPosition;
        state = TrapState.Waiting;
        moveDir = Vector2.zero;
        pauseTimer = 0f;
    }

    // ==========================
    // Activation
    // ==========================
    public void TryActivate(Vector2 direction)
    {
        if (state != TrapState.Waiting)
            return;

        moveDir = direction.normalized;
        state = TrapState.Charging;
    }

    // ==========================
    // Movement
    // ==========================
    protected override void Move()
    {
        if (state == TrapState.Waiting)
            return;

        if (state == TrapState.Charging)
        {
            Vector2 nextPos = (Vector2)rb.position +
                moveDir * chargeSpeed * Time.fixedDeltaTime;

            if (IsBlocked(nextPos))
            {
                state = TrapState.Pausing;
                pauseTimer = returnPause;
                return;
            }
            else if (state == TrapState.Pausing)
            {
                pauseTimer -= Time.fixedDeltaTime;

                if (pauseTimer <= 0f)
                    state = TrapState.Returning;
            }

            rb.MovePosition(new Vector3(nextPos.x, nextPos.y, rb.position.z));
        }
        else if (state == TrapState.Returning)
        {
            Vector2 nextPos = Vector2.MoveTowards(
                rb.position,
                homePosition,
                returnSpeed * Time.fixedDeltaTime
            );

            if (Vector2.Distance(nextPos, homePosition) < 0.01f)
            {
                nextPos = homePosition;
                state = TrapState.Waiting;
            }

            rb.MovePosition(new Vector3(nextPos.x, nextPos.y, rb.position.z));
        }
    }

    // ==========================
    // Collision checks
    // ==========================
    private bool IsBlocked(Vector2 position)
    {
        Collider[] hits = Physics.OverlapBox(
            new Vector3(position.x, position.y, rb.position.z),
            new Vector3(0.35f, 0.35f, 0.35f)
        );

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("wall") ||
                hit.GetComponentInParent<LockedDoor>() != null ||
                hit.GetComponentInParent<BladeTrap>() != null &&
                hit.GetComponentInParent<BladeTrap>() != this)
            {
                return true;
            }
        }

        return false;
    }
}
