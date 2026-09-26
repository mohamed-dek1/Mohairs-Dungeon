using UnityEngine;

public class Collector : MonoBehaviour
{
    Inventory inventory;

    void Start()
    {
        inventory = GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("WARNING: GameObject with a collector has no inventory to store things in!");
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (inventory == null)
            return;

        if (other.TryGetComponent<Collectable>(out Collectable collectible))
        {
            collectible.Collect(inventory);
        }
    }
}
