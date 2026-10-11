
using System;
using System.Collections.Generic;
using UnityEngine;

public class Wallmaster : Enemy
{
    // ==========================
    // Settings
    // ==========================
    [SerializeField] private int pursuitStepLimit = 12;
    [SerializeField] private float stepDelay = 0.3f;
    [SerializeField] private float tileSize = 0.5f;

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

    private Vector3 entryPosition;
    private Vector3 currentTile;
    private Vector3 targetTile;

    private readonly List<Vector3> pursuitPath = new List<Vector3>();

    private int pursuitSteps;
    private float stepTimer;
    private bool isMoving;
    private bool isPursuitStep;

    // ==========================
    // Room setup
    // ==========================
    protected override void RoomEntered()
    {
        base.RoomEntered();

        entryPosition = startPosition;
        rb.position = entryPosition;

        currentTile = entryPosition;
        targetTile = entryPosition;

        pursuitPath.Clear();
        pursuitPath.Add(entryPosition);

        state = WallmasterState.Pursuing;
        pursuitSteps = 0;
        stepTimer = 0f;
        isMoving = false;
        isPursuitStep = false;
        carriedPlayer = null;
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

        if (isMoving)
        {
            MoveTowardTarget();
            SyncCarriedPlayer();
            return;
        }

        if (stepTimer > 0f)
        {
            stepTimer -= Time.fixedDeltaTime;
            SyncCarriedPlayer();
            return;
        }

        if (state == WallmasterState.Pursuing)
            BeginPursuitStep();
        else
            BeginReturnStep();

        SyncCarriedPlayer();
    }

    private void MoveTowardTarget()
    {
        Vector3 nextPosition = Vector3.MoveTowards(
            rb.position,
            targetTile,
            movementSpeed * Time.fixedDeltaTime
        );

        nextPosition.z = entryPosition.z;
        rb.MovePosition(nextPosition);

        if (Vector2.Distance(
            new Vector2(nextPosition.x, nextPosition.y),
            new Vector2(targetTile.x, targetTile.y)) > 0.001f)
        {
            return;
        }

        rb.MovePosition(targetTile);

        currentTile = targetTile;
        isMoving = false;
        stepTimer = stepDelay;

        OnTileReached();
    }

    private void OnTileReached()
    {
        if (isPursuitStep)
        {
            pursuitPath.Add(currentTile);
            isPursuitStep = false;

            if (state == WallmasterState.Pursuing)
            {
                pursuitSteps++;

                if (pursuitSteps >= pursuitStepLimit)
                    state = WallmasterState.Retreating;
            }

            return;
        }

        // We have reached the previous tile on the recorded path.
        if (pursuitPath.Count > 1)
            pursuitPath.RemoveAt(pursuitPath.Count - 1);

        if (pursuitPath.Count <= 1)
            FinishReturn();
    }

    // ==========================
    // Pursuit
    // ==========================
    private void BeginPursuitStep()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject == null)
            return;

        Vector3 playerPosition = playerObject.transform.position;

        Vector2 difference = new Vector2(
            playerPosition.x - currentTile.x,
            playerPosition.y - currentTile.y
        );

        if (difference.sqrMagnitude < 0.001f)
            return;

        Vector2Int primaryDirection;
        Vector2Int secondaryDirection = Vector2Int.zero;

        // Prioritize the axis with the largest distance to the player.
        if (Mathf.Abs(difference.x) > Mathf.Abs(difference.y))
        {
            primaryDirection = difference.x > 0f
                ? Vector2Int.right
                : Vector2Int.left;

            if (Mathf.Abs(difference.y) > 0.001f)
            {
                secondaryDirection = difference.y > 0f
                    ? Vector2Int.up
                    : Vector2Int.down;
            }
        }
        else
        {
            primaryDirection = difference.y > 0f
                ? Vector2Int.up
                : Vector2Int.down;

            if (Mathf.Abs(difference.x) > 0.001f)
            {
                secondaryDirection = difference.x > 0f
                    ? Vector2Int.right
                    : Vector2Int.left;
            }
        }

        if (TryBeginStep(primaryDirection))
            return;

        if (secondaryDirection != Vector2Int.zero &&
            TryBeginStep(secondaryDirection))
        {
            return;
        }

        // No available step toward the player.
        state = WallmasterState.Retreating;
    }

    private bool TryBeginStep(Vector2Int direction)
    {
        Vector3 nextTile = currentTile + new Vector3(
            direction.x * tileSize,
            direction.y * tileSize,
            0f
        );

        if (IsBlocked(nextTile))
            return false;

        targetTile = nextTile;
        isMoving = true;
        isPursuitStep = true;

        return true;
    }

    // ==========================
    // Carrying player
    // ==========================
    protected override void OnHitPlayer()
    {
        if (state != WallmasterState.Pursuing)
            return;

        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject == null)
            return;

        ArrowKeyMovement player =
            playerObject.GetComponentInParent<ArrowKeyMovement>();

        GrabPlayer(player);
    }

    public void GrabPlayer(ArrowKeyMovement player)
    {
        if (player == null || state != WallmasterState.Pursuing)
            return;

        carriedPlayer = player;
        carriedPlayer.SetCarried(true);

        state = WallmasterState.CarryingPlayer;
    }

    private void SyncCarriedPlayer()
    {
        if (carriedPlayer == null)
            return;

        Vector3 playerPosition = new Vector3(
            rb.position.x,
            rb.position.y,
            carriedPlayer.transform.position.z
        );

        carriedPlayer.SetCarriedPosition(playerPosition);
    }

    // ==========================
    // Retreat and return
    // ==========================
    private void BeginReturnStep()
    {
        if (pursuitPath.Count <= 1)
        {
            FinishReturn();
            return;
        }

        // Follow the path in reverse, one tile at a time.
        targetTile = pursuitPath[pursuitPath.Count - 2];
        isMoving = true;
        isPursuitStep = false;
    }

    private void FinishReturn()
    {
        bool carryingPlayer = state == WallmasterState.CarryingPlayer;

        if (carriedPlayer != null)
        {
            carriedPlayer.SetCarried(false);
            carriedPlayer = null;
        }

        if (carryingPlayer)
            OnPlayerCarriedOut?.Invoke();
        else
            OnDespawn?.Invoke();

        gameObject.SetActive(false);
    }

    // ==========================
    // Collision checks
    // ==========================
    private bool IsBlocked(Vector3 position)
    {
        Collider[] hits = Physics.OverlapBox(
            new Vector3(position.x, position.y, entryPosition.z),
            new Vector3(tileSize * 0.4f, tileSize * 0.4f, 0.4f)
        );

        foreach (Collider hit in hits)
        {
            if (hit.transform == transform ||
                hit.transform.IsChildOf(transform) ||
                hit.isTrigger)
            {
                continue;
            }

            if (hit.CompareTag("wall") ||
                hit.GetComponentInParent<LockedDoor>() != null)
            {
                return true;
            }

            GridWalk otherEnemy = hit.GetComponentInParent<GridWalk>();

            if (otherEnemy != null && otherEnemy != this)
                return true;

            Wallmaster otherWallmaster =
                hit.GetComponentInParent<Wallmaster>();

            if (otherWallmaster != null && otherWallmaster != this)
                return true;
        }

        return false;
    }
}
