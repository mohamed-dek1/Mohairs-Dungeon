using UnityEngine;

public class SwordShard : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float maxLife = 0.5f;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector2 direction)
    {
        rb.linearVelocity = direction * speed;

        Destroy(gameObject, maxLife);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("rupee") || other.CompareTag("wall") || other.CompareTag("shard") || other.CompareTag("beam sword")) return;

        if (other.CompareTag("enemy"))
        {
            Destroy(other.gameObject);
        }

        Destroy(gameObject);
    }
}
