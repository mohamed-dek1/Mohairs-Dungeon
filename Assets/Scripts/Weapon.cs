using UnityEngine;

public class Weapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (tag == "beam sword")
        {
            Debug.Log("Hit something");
            Destroy(this.gameObject);
        }
        if (other.CompareTag("enemy"))
        {
            Debug.Log("Hit enemy");
            Destroy(other.gameObject);
        }
    }
}
