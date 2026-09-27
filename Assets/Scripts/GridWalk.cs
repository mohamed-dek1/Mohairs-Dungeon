using UnityEngine;
using System.Collections.Generic;

public abstract class GridWalk : Enemy
{
    // ==========================
    // Public fields
    // ==========================
    public float tileSize = 1.0f;

    // ==========================
    // Protected fields
    // ==========================
    protected Vector2 moveDir;
    protected Vector2 targetTile;
    protected Transform player;

    // ==========================
    // Private fields
    // ==========================
    private float waitTimer;

    protected override void RoomEntered()
    {
        base.RoomEntered();
        targetTile = startPosition;
        moveDir = Vector2.zero;
        waitTimer = 0.0f;

        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
    }

    protected override void Move()
    {
        if (waitTimer > 0)
        {
            waitTimer -= Time.fixedDeltaTime;
            return;
        }

        Vector2 currentPos = rb.position;

        // on center tile
        if (Vector2.Distance(currentPos, targetTile) < 0.01f)
        {
            moveDir = ChooseDirection();

            if (moveDir == Vector2.zero)
                return;

            targetTile = targetTile + moveDir * tileSize;
        }

        Vector2 nextPos = Vector2.MoveTowards(currentPos, targetTile, GetSpeed() * Time.fixedDeltaTime);
        rb.MovePosition(nextPos);
    }

    // return open direction
    protected abstract Vector2 ChooseDirection();

    protected virtual float GetSpeed()
    {
        return movementSpeed;
    }

    // ==========================
    // Helpers
    // ==========================
    protected bool IsBlocked(Vector2 dir)
    {
        Vector3 nextTile = rb.position + (Vector3)(dir * tileSize);
        Collider[] hits = Physics.OverlapBox(nextTile, new Vector3(0.4f, 0.4f, 0.4f));

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("wall") || hit.GetComponent<LockedDoor>() != null)
                return true;
        }

        return false;
    }

    protected void Wait(float seconds)
    {
        waitTimer = seconds;
    }

    // Pick random open direction
    protected Vector2 RandomDirection(float keepStraightChance)
    {
        List<Vector2> openDirs = new List<Vector2>();
        AddIfOpen(openDirs, Vector2.up);
        AddIfOpen(openDirs, Vector2.down);
        AddIfOpen(openDirs, Vector2.left);
        AddIfOpen(openDirs, Vector2.right);

        // Dead end
        if (openDirs.Count == 0)
            return -moveDir;

        if (openDirs.Contains(moveDir) && Random.value < keepStraightChance)
            return moveDir;

        return openDirs[Random.Range(0, openDirs.Count)];
    }

    // Check if player is on same row col
    protected bool IsAlignedWithPlayer(out Vector2 dirToPlayer)
    {
        Vector2 offset = (Vector2)player.position - (Vector2)rb.position;
        dirToPlayer = Vector2.zero;

        if (Mathf.Abs(offset.x) < tileSize / 2f)
        {
            if (offset.y > 0)
                dirToPlayer = Vector2.up;
            else
                dirToPlayer = Vector2.down;
            return true;
        }
        else if (Mathf.Abs(offset.y) < tileSize / 2f)
        {
            if (offset.x > 0)
                dirToPlayer = Vector2.right;
            else
                dirToPlayer = Vector2.left;
            return true;
        }

        return false;
    }

    public Vector2 GetMoveDir()
    {
        return moveDir;
    }

    private void AddIfOpen(List<Vector2> openDirs, Vector2 dir)
    {
        if (dir == -moveDir)
            return;

        if (!IsBlocked(dir))
            openDirs.Add(dir);
    }
}