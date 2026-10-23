using UnityEngine;

public class GoriyaBoomerang : Boomerang
{
    public void Throw(Vector2 direction, Transform goriya)
    {
        thrower = goriya;
        Launch(direction);
    }

    // returns after hitting the player or a wall
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player Hurtbox"))
        {
            Health playerHealth = other.GetComponentInParent<Health>();
            if (playerHealth != null)
                playerHealth.TakeDamage(damage, transform.position);

            isReturning = true;
            return;
        }

        if (!other.isTrigger)
            OnHitSolid();
    }

    // Boomerang catch
    void OnTriggerStay(Collider other)
    {
        if (isReturning && other.transform == thrower)
            Destroy(gameObject);
    }
}
