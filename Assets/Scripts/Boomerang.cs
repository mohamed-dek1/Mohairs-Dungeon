using UnityEngine;

public class Boomerang : Projectile
{
    private float launchTime;
    protected bool isReturning = false;
    protected Transform thrower; // flies back to this
    private Transform carriedItem = null;

    [SerializeField] float returnDelay = 0.5f;
    [SerializeField] float rotationSpeed = 720f; // degrees per second
    
    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // enemy boomerangs (GoriyaBoomerang) set their own thrower
        if (player != null)
        {
            thrower = player.transform;
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
            if (thrower == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector2 directionToThrower = (thrower.position - transform.position).normalized;
            rb.linearVelocity = directionToThrower * speed;

            float distanceToThrower = Vector2.Distance(transform.position, thrower.position);
            if (distanceToThrower < 0.001f)
            {
                Catch();
            }
        }

        if (carriedItem != null)
        {
            carriedItem.position = transform.position;
        }

    }

    private void Catch()
    {
        if (carriedItem != null)
        {
            // Trigger pickup logic if the item has an ItemPickup component
            if (carriedItem.TryGetComponent<Collectable>(out var item))
            {
                item.Collect(thrower.GetComponent<Inventory>());
            }
        }

        Destroy(gameObject);
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

        if (other.CompareTag("Collectable"))
        {
            if (carriedItem == null)
            {
                carriedItem = other.transform;
                
                // if (other.TryGetComponent<Collider>(out var col)) col.enabled = false;

                isReturning = true;
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
