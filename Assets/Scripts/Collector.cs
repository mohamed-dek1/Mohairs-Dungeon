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
        GameObject other_game_object = other.gameObject;

        if (other_game_object.tag == "rupee")
        {
            if (inventory != null)
            {
                inventory.AddRupees(1);
            }
            Destroy(other_game_object);
        }
    }
}
