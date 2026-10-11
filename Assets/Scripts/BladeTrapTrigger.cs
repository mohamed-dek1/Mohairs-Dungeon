
using UnityEngine;

public class BladeTrapTrigger : MonoBehaviour
{
    public enum Axis { Horizontal, Vertical }

    // ==========================
    // Settings
    // ==========================
    [SerializeField] private BladeTrap bladeTrap;
    [SerializeField] private Axis axis;

    // ==========================
    // Trigger detection
    // ==========================
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || bladeTrap == null)
            return;

        Vector2 trapPosition = bladeTrap.transform.position;
        Vector2 playerPosition = other.transform.position;

        Vector2 direction;

        if (axis == Axis.Horizontal)
        {
            direction = playerPosition.x >= trapPosition.x
                ? Vector2.right
                : Vector2.left;
        }
        else
        {
            direction = playerPosition.y >= trapPosition.y
                ? Vector2.up
                : Vector2.down;
        }

        bladeTrap.TryActivate(direction);
    }
}
