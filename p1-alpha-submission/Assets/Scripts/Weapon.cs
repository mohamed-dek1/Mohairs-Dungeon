using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected int damage = 1;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
            return;

        if (other.TryGetComponent<Health>(out Health targetHealth))
        {
            targetHealth.TakeDamage(damage);
            OnHitEnemy();
        }
        else if (!other.isTrigger)
        {
            OnHitSolid();
        }
    }

    // What happens to the weapon itself after a hit. The melee sword does nothing.
    protected virtual void OnHitEnemy() { }
    protected virtual void OnHitSolid() { }
}