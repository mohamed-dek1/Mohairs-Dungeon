using UnityEngine;

public class Weapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
            return;

        if (other.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Defeat();
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