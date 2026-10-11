
using UnityEngine;

public class BladeTrapTrigger : MonoBehaviour
{
    public enum Axis { Horizontal, Vertical }

    // ==========================
    // Settings
    // ==========================
    [SerializeField] private BladeTrap[] bladeTraps;
    [SerializeField] private Axis axis;

    // ==========================
    // Trigger detection
    // ==========================
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Vector2 playerPosition = other.transform.position;

        foreach (BladeTrap bladeTrap in bladeTraps)
        {
            if (bladeTrap == null)
                continue;

            Vector2 trapPosition = bladeTrap.transform.position;
            Vector2 direction;

            if (axis == Axis.Horizontal)
            {
                if (playerPosition.x >= trapPosition.x)
                    direction = Vector2.right;
                else
                    direction = Vector2.left;
            }
            else
            {
                if (playerPosition.y >= trapPosition.y)
                    direction = Vector2.up;
                else
                    direction = Vector2.down;
            }

            bladeTrap.TryActivate(direction);
        }
    }
}
