using UnityEngine;

public class SwordBeam : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private GameObject shardSpawnerPrefab;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector2 direction)
    {
        rb.linearVelocity = direction * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        rb.freezeRotation = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player") || other.CompareTag("rupee"))
        {
            return;
        }
        // if (other.CompareTag("sword") || other.CompareTag("Player") || other.CompareTag("rupee"))
        // {
        //     return;
        // }
        if (other.CompareTag("enemy"))
        {
            Destroy(other.gameObject);
        }

        if (shardSpawnerPrefab != null)
        {
            Instantiate(shardSpawnerPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
