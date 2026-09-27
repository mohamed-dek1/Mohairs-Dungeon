using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    [System.Serializable]
    public class DropEntry
    {
        public GameObject item;
        [Range(0, 100)] public float chance;
    }

    // ==========================
    // Serialized fields
    // ==========================
    [SerializeField] private GameObject carriedItem;
    [SerializeField] private Vector3 carriedOffset = Vector3.zero;
    [SerializeField] private DropEntry[] drops;

    // ==========================
    // Private fields
    // ==========================
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer carriedRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Health health = GetComponent<Health>();
        if (health != null)
            health.OnDeath += Drop;

        if (carriedItem != null)
            ShowCarriedItem();
    }

    void LateUpdate()
    {
        // hide item when enemy is hidden
        if (carriedRenderer != null)
            carriedRenderer.enabled = spriteRenderer.enabled;
    }

    public void Drop()
    {
        // carried item replaces random drop
        if (carriedItem != null)
        {
            Instantiate(carriedItem, transform.position, Quaternion.identity);
            return;
        }

        float roll = Random.Range(0.0f, 100.0f);
        float total = 0.0f;

        foreach (DropEntry entry in drops)
        {
            total += entry.chance;

            if (roll < total)
            {
                Instantiate(entry.item, transform.position, Quaternion.identity);
                return;
            }
        }
    }

    private void ShowCarriedItem()
    {
        GameObject carried = new GameObject("CarriedItem");
        carried.transform.SetParent(transform, false);
        carried.transform.localPosition = carriedOffset;

        carriedRenderer = carried.AddComponent<SpriteRenderer>();
        carriedRenderer.sprite = carriedItem.GetComponentInChildren<SpriteRenderer>().sprite;

        // draw on top of enemy
        carriedRenderer.sortingLayerID = spriteRenderer.sortingLayerID;
        carriedRenderer.sortingOrder = spriteRenderer.sortingOrder + 1;
    }
}