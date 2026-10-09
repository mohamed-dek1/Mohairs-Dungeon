using UnityEngine;

public class Boomerang : Projectile
{
    private float launchTime;
    private bool isReturning = false;
    private Transform playerTransform;

    [SerializeField] float returnDelay = 0.5f;
    [SerializeField] float rotationSpeed = 720f; // degrees per second
    
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // this needs to be updated if we have enemies that use boomerangs...
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public override void Launch(Vector2 direction)
    {
        base.Launch(direction);
        FaceDirection(direction);
        launchTime = Time.time;
        isReturning = false;
    }

    void FixedUpdate() 
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        if (!isReturning && Time.time - launchTime >= returnDelay)
        {
            isReturning = true;
        }

        if (isReturning)
        {
            if (playerTransform == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            rb.linearVelocity = directionToPlayer * speed;

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer < 0.01f)
            {
                Destroy(gameObject);
            }
        }

    }

    protected override void OnHitSolid()
    {
        if (!isReturning)
        {
            isReturning = true;
        }
    }

    protected override void OnHitEnemy()
    {
        if (!isReturning)
        {
            isReturning = true;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isReturning)
            {
                Destroy(gameObject);
            }
            return;
        }

        if (other.TryGetComponent<Health>(out Health enemyHealth))
        {
            enemyHealth.TakeDamage(damage); // will eventually add instakill for smaller enemies

            if (other.TryGetComponent<EnemyAnimator>(out var enemyAnimator))
            {
                // enemyAnimator.TriggerStun(); will also need to stun enemies
            }

            OnHitEnemy();
        }
        else if (!other.isTrigger)
        {
            OnHitSolid();
        }
    }
}
