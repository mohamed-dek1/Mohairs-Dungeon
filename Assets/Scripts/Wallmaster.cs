using System;
using UnityEngine;

public class Wallmaster : Enemy
{
    // ==========================
    // Settings
    // ==========================
    [SerializeField] private int pursuitStepLimit = 12;
    [SerializeField] private float stepDelay = 0.3f;
    [SerializeField] private float tileSize = 0.5f;
    [SerializeField] private int maxWallSearchDistance = 30;

    // ==========================
    // Events
    // ==========================
    public event Action OnDespawn;
    public event Action OnPlayerCarriedOut;

    // ==========================
    // Private fields
    // ==========================
    private enum WallmasterState
    {
        Pursuing,
        Retreating,
        CarryingPlayer
    }

    private WallmasterState state;
    private ArrowKeyMovement carriedPlayer;

    private Vector2Int currentGridPosition;
    private Vector2Int targetGridPosition;
    private Vector2Int entryGridPosition;

    private int pursuitSteps;
    private float stepTimer;
    private bool hasTarget;

    // ==========================
    // Room setup
    // ==========================
    protected override void RoomEntered()
    {
        base.RoomEntered();

        currentGridPosition = WorldToGrid(transform.position);
        entryGridPosition = currentGridPosition;
        targetGridPosition = currentGridPosition;

        state = WallmasterState.Pursuing;
        pursuitSteps = 0;
        stepTimer = stepDelay;
        carriedPlayer = null;
        hasTarget = false;
    }

    // ==========================
    // Movement
    // ==========================
    protected override void Move()
    {
        if (state == WallmasterState.CarryingPlayer &&
            carriedPlayer == null)
        {
            state = WallmasterState.Retreating;
        }

        stepTimer -= Time.fixedDeltaTime;

        if (stepTimer > 0f)
            return;

        stepTimer = stepDelay;

        if (state == WallmasterState.Pursuing)
            PursuePlayer();
        else if (state == WallmasterState.Retreating)
            RetreatToWall();
        else if (state == WallmasterState.CarryingPlayer)
            CarryPlayerToWall();
    }

    // ==========================
    // Pursuit
    // ==========================
    private void PursuePlayer()
    {
        Transform playerTransform = GameObject.FindWithTag("Player")?.transform;

        if (playerTransform == null)
            return;

        Vector2Int playerPosition = WorldToGrid(playerTransform.position);

        if (currentGridPosition == playerPosition)
            return;

        Vector2Int direction = GetDirectionToPlayer(
            currentGridPosition,
            playerPosition
        );

        if (!TryMoveOneTile(direction))
        {
            Vector2Int alternateDirection = GetAlternateDirection(
                currentGridPosition,
                playerPosition,
                direction
            );

            if (alternateDirection == Vector2Int.zero ||
                !TryMoveOneTile(alternateDirection))
            {
                state = WallmasterState.Retreating;
                return;
            }
        }

        pursuitSteps++;

        if (pursuitSteps >= pursuitStepLimit)
            state = WallmasterState.Retreating;
    }

    private Vector2Int GetDirectionToPlayer(
        Vector2Int current,
        Vector2Int target)
    {
        Vector2Int difference = target - current;

        if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
            return difference.x > 0 ? Vector2Int.right : Vector2Int.left;

        return difference.y > 0 ? Vector2Int.up : Vector2Int.down;
    }

    private Vector2Int GetAlternateDirection(
        Vector2Int current,
        Vector2Int target,
        Vector2Int previousDirection)
    {
        Vector2Int difference = target - current;

        if (previousDirection.x != 0 && difference.y != 0)
            return difference.y > 0 ? Vector2Int.up : Vector2Int.down;

        if (previousDirection.y != 0 && difference.x != 0)
            return difference.x > 0 ? Vector2Int.right : Vector2Int.left;

        return Vector2Int.zero;
    }

    // ==========================
    // Retreat and carrying
    // ==========================
    private void RetreatToWall()
    {
        if (!hasTarget)
        {
            targetGridPosition = FindNearestWallTile(currentGridPosition);
            hasTarget = true;
        }

        if (currentGridPosition == targetGridPosition)
        {
            OnDespawn?.Invoke();
            gameObject.SetActive(false);
            return;
        }

        MoveOneTileTowardTarget();
    }

    public void GrabPlayer(IGridMovement player)
    {
        if (player == null || state != WallmasterState.Pursuing)
            return;

        carriedPlayer = player;
        carriedPlayer.SetCarried(true);

        state = WallmasterState.CarryingPlayer;
        hasTarget = false;
    }

    private void CarryPlayerToWall()
    {
        if (!hasTarget)
        {
            targetGridPosition = FindNearestWallTile(currentGridPosition);
            hasTarget = true;
        }

        if (currentGridPosition == targetGridPosition)
        {
            if (carriedPlayer != null)
            {
                carriedPlayer.SetCarried(false);
                carriedPlayer = null;
            }

            OnPlayerCarriedOut?.Invoke();
            gameObject.SetActive(false);
            return;
        }

        MoveOneTileTowardTarget();

        if (carriedPlayer != null)
            carriedPlayer.SetCarriedPosition(currentGridPosition);
    }

    // ==========================
    // Grid movement
    // ==========================
    private bool TryMoveOneTile(Vector2Int direction)
    {
        Vector2Int nextPosition = currentGridPosition + direction;

        if (IsWallTile(nextPosition))
            return false;

        currentGridPosition = nextPosition;
        transform.position = GridToWorld(currentGridPosition);

        if (carriedPlayer != null)
            carriedPlayer.SetGridPosition(currentGridPosition);

        return true;
    }

    private void MoveOneTileTowardTarget()
    {
        Vector2Int difference = targetGridPosition - currentGridPosition;

        Vector2Int direction;

        if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
            direction = difference.x > 0 ? Vector2Int.right : Vector2Int.left;
        else if (difference.y != 0)
            direction = difference.y > 0 ? Vector2Int.up : Vector2Int.down;
        else
            direction = Vector2Int.zero;

        if (direction != Vector2Int.zero && !TryMoveOneTile(direction))
            hasTarget = false;
    }

    // ==========================
    // Wall detection
    // ==========================
    private Vector2Int FindNearestWallTile(Vector2Int origin)
    {
        Vector2Int bestPosition = origin;
        int bestDistance = int.MaxValue;

        Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (Vector2Int direction in directions)
        {
            for (int distance = 1; distance <= maxWallSearchDistance; distance++)
            {
                Vector2Int position = origin + direction * distance;

                if (!IsWallTile(position))
                    continue;

                Vector2Int lastWalkablePosition = position - direction;
                int totalDistance = Mathf.Abs(lastWalkablePosition.x - origin.x)
                    + Mathf.Abs(lastWalkablePosition.y - origin.y);

                if (totalDistance < bestDistance)
                {
                    bestDistance = totalDistance;
                    bestPosition = lastWalkablePosition;
                }

                break;
            }
        }

        return bestPosition;
    }

    private bool IsWallTile(Vector2Int gridPosition)
    {
        Vector3 worldPosition = GridToWorld(gridPosition);

        Collider[] hits = Physics.OverlapBox(
            worldPosition,
            new Vector3(tileSize * 0.4f, tileSize * 0.4f, 0.4f)
        );

        foreach (Collider hit in hits)
        {
            if (hit.transform == transform ||
                hit.transform.IsChildOf(transform))
                continue;

            if (hit.CompareTag("wall") ||
                hit.GetComponentInParent<LockedDoor>() != null)
                return true;
        }

        return false;
    }

    private Vector2Int WorldToGrid(Vector3 position)
    {
        return new Vector2Int(
            Mathf.RoundToInt(position.x / tileSize),
            Mathf.RoundToInt(position.y / tileSize)
        );
    }

    private Vector3 GridToWorld(Vector2Int position)
    {
        return new Vector3(
            position.x * tileSize,
            position.y * tileSize,
            transform.position.z
        );
    }
}
