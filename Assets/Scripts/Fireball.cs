using UnityEngine;

public class Fireball : Projectile
{
    // Enemy fireballs fly over walls and blocks
    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("Player Hurtbox"))
            return;

        Health playerHealth = other.GetComponentInParent<Health>();
        if (playerHealth != null)
            playerHealth.TakeDamage(damage, transform.position);

        Destroy(gameObject);
    }
}
