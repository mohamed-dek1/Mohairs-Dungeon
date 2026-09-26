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
        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Defeat();
            Destroy(gameObject);
        }
    }
}
