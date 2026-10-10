using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject[] dustClouds;
    [SerializeField] private float damageRadius = 5f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float dustLifetime = 2f;
    [SerializeField] private float bombTimer = 1.0f;
    [SerializeField] private float xOffset = 1f;
    [SerializeField] private float yOffset = 1f;


    public IEnumerator Explode()
    {
        foreach (GameObject dustChild in dustClouds)
        {
            if (dustChild != null)
            {
                dustChild.transform.SetParent(null);
                dustChild.SetActive(true);
                Destroy(dustChild, dustLifetime);
            }
        }
        
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, damageRadius);
        HashSet<Health> damagedTargets = new HashSet<Health>();

        foreach (Collider hit in hitColliders)
        {
            if (hit.CompareTag("Player") || hit.CompareTag("enemy"))
            {
                Health health = hit.GetComponentInParent<Health>();
                if (health != null && !damagedTargets.Contains(health))
                {
                    health.TakeDamage(damage);
                    Debug.Log("Bomb damaged: " + hit.name + " for " + damage + " damage.");
                }
            }
        }

        yield return new WaitForSeconds(bombTimer);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
