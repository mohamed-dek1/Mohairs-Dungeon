using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Wallmaster : GridWalk
{
    // ==========================
    // Settings
    // ==========================
    [Header("Wallmaster Settings")]
    [SerializeField] private int pursuitStepLimit = 12;
    [SerializeField] private Vector2 entryDirection = Vector2.right;

    // ==========================
    // Events
    // ==========================
    public event Action OnDespawn;
    public event Action OnPlayerCarriedOut;

    // ==========================
    // State Tracking
    // ==========================
    private enum State { Pursuing, Retreating, CarryingPlayer }
    private State state;

    private int emergenceStepCount;
    private int pursuitStepCount;

    private ArrowKeyMovement carriedPlayer;
    private Rigidbody carriedPlayerRb;
    private Collider[] carriedPlayerColliders;
    private bool[] originalColliderStates;
    private bool originalKinematicState;

// ==========================
    // Room Setup
    // ==========================
    public override void Init(RoomHandler handler)
    {
        base.Init(handler);
        
        // Prevents Enemy.cs OnTriggerStay from applying knockback when grabbing the player
        contactDamage = 0; 
        
        // Force the Wallmaster to find the player if GridWalk missed it during dynamic spawn
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    private void ResetWallmaster()
    {
        rb.position = startPosition;
        targetTile = startPosition;
        moveDir = Vector2.zero;

        state = State.Pursuing;
        emergenceStepCount = 0;
        pursuitStepCount = 0;

        // Double check player exists before starting pursuit
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        ReleasePlayer();
    }

    protected override void RoomEntered()
    {
        base.RoomEntered();
        ResetWallmaster();
    }

    public void SpawnAt(Vector3 spawnWorldPos, Vector2 directionIntoRoom)
    {
        transform.position = spawnWorldPos;
        startPosition = spawnWorldPos;
        entryDirection = directionIntoRoom.normalized;
        
        gameObject.SetActive(true);
        ResetWallmaster();
    }

    // ==========================
    // Movement & Direction Selection
    // ==========================
    protected override void Move()
    {
        // Executes standard GridWalk movement loop
        base.Move();

        // Keep carried player synced with Wallmaster's position
        SyncCarriedPlayer();
    }

    protected override Vector2 ChooseDirection()
    {
        // Phase 1: Emergence (Move 2 tiles out through the wall into the room)
        if (state == State.Pursuing && emergenceStepCount < 2)
        {
            emergenceStepCount++;
            return entryDirection;
        }

        // Phase 2: Retreating or Carrying Player back into wall
        if (state == State.Retreating || state == State.CarryingPlayer)
        {
            Vector2 toStart = (Vector2)startPosition - targetTile;

            // Reached initial wall spawn origin tile
            if (toStart.sqrMagnitude < 0.001f)
            {
                FinishReturn();
                return Vector2.zero;
            }

            return GetCardinalDirTo(toStart);
        }

        // Phase 3: Pursuing the Player
        if (pursuitStepCount >= pursuitStepLimit)
        {
            state = State.Retreating;
            return ChooseDirection();
        }

        if (player == null) return Vector2.zero;

        Vector2 toPlayer = (Vector2)player.position - targetTile;
        if (toPlayer.sqrMagnitude < 0.001f) return Vector2.zero;

        Vector2 primaryDir;
        Vector2 secondaryDir = Vector2.zero;

        if (Mathf.Abs(toPlayer.x) > Mathf.Abs(toPlayer.y))
        {
            primaryDir = toPlayer.x > 0 ? Vector2.right : Vector2.left;
            if (Mathf.Abs(toPlayer.y) > 0.001f)
                secondaryDir = toPlayer.y > 0 ? Vector2.up : Vector2.down;
        }
        else
        {
            primaryDir = toPlayer.y > 0 ? Vector2.up : Vector2.down;
            if (Mathf.Abs(toPlayer.x) > 0.001f)
                secondaryDir = toPlayer.x > 0 ? Vector2.right : Vector2.left;
        }

        // Only consume a pursuit step if the Wallmaster actually moves
        if (!IsBlocked(primaryDir))
        {
            pursuitStepCount++;
            return primaryDir;
        }

        if (secondaryDir != Vector2.zero && !IsBlocked(secondaryDir))
        {
            pursuitStepCount++;
            return secondaryDir;
        }

        // If both paths are blocked (e.g. cornered on obstacles), 
        // trigger a retreat back into the wall rather than freezing in place.
        state = State.Retreating;
        return ChooseDirection();
    }

    // ==========================
    // Collision Override 
    // ==========================
    protected override bool IsBlocked(Vector2 dir)
    {
        // Bypass wall colliders while stepping out through the 2 wall tiles on spawn
        if (state == State.Pursuing && emergenceStepCount < 2)
        {
            return false;
        }

        // Phase 2: Ghost through obstacles to guarantee retreat to the wall without getting stuck
        if (state == State.Retreating || state == State.CarryingPlayer)
        {
            return false;
        }

        // Standard GridWalk obstacle check for room floor area
        return base.IsBlocked(dir);
    }

    // ==========================
    // Player Capture & Sync
    // ==========================
    protected override void OnHitPlayer()
    {
        if (state != State.Pursuing) return;

        ArrowKeyMovement playerMovement = player.GetComponentInParent<ArrowKeyMovement>();
        if (playerMovement != null)
        {
            GrabPlayer(playerMovement);
        }
    }

    private void GrabPlayer(ArrowKeyMovement playerMovement)
    {
        if (state == State.CarryingPlayer) return;

        carriedPlayer = playerMovement;
        carriedPlayer.enabled = false; // Disable player input

        // Lock player physics so gravity/velocity doesn't fight the Wallmaster
        carriedPlayerRb = carriedPlayer.GetComponent<Rigidbody>();
        if (carriedPlayerRb != null)
        {
            originalKinematicState = carriedPlayerRb.isKinematic;
            carriedPlayerRb.isKinematic = true;
        }

        // Disable player colliders to prevent physics fighting
        carriedPlayerColliders = carriedPlayer.GetComponentsInChildren<Collider>();
        originalColliderStates = new bool[carriedPlayerColliders.Length];

        for (int i = 0; i < carriedPlayerColliders.Length; i++)
        {
            originalColliderStates[i] = carriedPlayerColliders[i].enabled;
            carriedPlayerColliders[i].enabled = false;
        }

        state = State.CarryingPlayer;
    }

    private void SyncCarriedPlayer()
    {
        if (carriedPlayer != null)
        {
            carriedPlayer.transform.position = new Vector3(
                rb.position.x, 
                rb.position.y, 
                carriedPlayer.transform.position.z
            );
        }
    }

    private void ReleasePlayer()
    {
        if (carriedPlayer == null) return;

        carriedPlayer.enabled = true;

        if (carriedPlayerRb != null)
        {
            carriedPlayerRb.isKinematic = originalKinematicState;
        }

        if (carriedPlayerColliders != null)
        {
            for (int i = 0; i < carriedPlayerColliders.Length; i++)
            {
                if (carriedPlayerColliders[i] != null)
                    carriedPlayerColliders[i].enabled = originalColliderStates[i];
            }
        }

        carriedPlayer = null;
        carriedPlayerRb = null;
    }

    private void FinishReturn()
    {
        bool captured = (state == State.CarryingPlayer);

        ReleasePlayer();
        gameObject.SetActive(false);

        if (captured)
        {
            OnPlayerCarriedOut?.Invoke();
            // Reload the current scene when Link is successfully dragged away
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            OnDespawn?.Invoke();
        }
    }

    // ==========================
    // Helpers
    // ==========================
    private Vector2 GetCardinalDirTo(Vector2 offset)
    {
        if (Mathf.Abs(offset.x) >= Mathf.Abs(offset.y))
            return offset.x > 0 ? Vector2.right : Vector2.left;
        else
            return offset.y > 0 ? Vector2.up : Vector2.down;
    }
}