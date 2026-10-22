using UnityEngine;
using System.Collections;

public class WallmasterSpawner : RoomObject
{
    [Header("Spawning Setup")]
    [SerializeField] private Wallmaster wallmasterPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Vector2[] entryDirections; 
    
    [Header("Spawning Rules")]
    [SerializeField] private float spawnInterval = 4.0f;
    [SerializeField] private int maxActiveWallmasters = 2;

    private int currentActiveCount = 0;
    private Coroutine spawnCoroutine;

    // We do not call room.RegisterEnemy() here because the spawner itself is not an enemy.
    
    protected override void RoomEntered()
    {
        base.RoomEntered();
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnRoutine());
        }
    }

    protected override void RoomExited()
    {
        base.RoomExited();
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentActiveCount < maxActiveWallmasters && spawnPoints.Length > 0)
            {
                SpawnWallmaster();
            }
        }
    }

    private void SpawnWallmaster()
    {
        // Pick a random spawn location from the arrays
        int index = Random.Range(0, spawnPoints.Length);
        
        // 1. Instantiate the prefab
        Wallmaster newlySpawned = Instantiate(wallmasterPrefab, spawnPoints[index].position, Quaternion.identity);
        
        // 2. Initialize it using your existing room architecture
        // This automatically calls room.RegisterEnemy() inside Enemy.Init()
        newlySpawned.Init(room);
        
        // 3. Track its lifecycle so we don't exceed maxActiveWallmasters
        newlySpawned.OnDespawn += () => { currentActiveCount--; };
        newlySpawned.OnPlayerCarriedOut += () => { currentActiveCount--; };
        
        // 4. Trigger the Wallmaster's specific 2-tile emergence logic
        newlySpawned.SpawnAt(spawnPoints[index].position, entryDirections[index]);
        
        currentActiveCount++;
    }
}