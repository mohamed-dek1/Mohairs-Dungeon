using UnityEngine;

public class ShardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] swordShardPrefab;

    void Start()
    {
        Vector2[] directions = new Vector2[]
        {
            new Vector2(1, 1), // NE
            new Vector2(-1, 1), // SE
            new Vector2(-1, -1), // SW
            new Vector2(1, -1) // NW
        };

        int count = 0;
        foreach (Vector2 dir in directions)
        {
            GameObject shard = Instantiate(swordShardPrefab[count], transform.position, Quaternion.identity);

            if (shard.TryGetComponent<SwordShard>(out var shardScript))
            {
                shardScript.Launch(dir);
            }

            count++;
        }

        Destroy(gameObject);
    }
}
