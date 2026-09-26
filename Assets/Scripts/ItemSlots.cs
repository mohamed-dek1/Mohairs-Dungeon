using UnityEngine;
using UnityEngine.UI;

public class ItemSlots : MonoBehaviour
{
    public Inventory inventory;

    [SerializeField] private Image slotAImage;
    [SerializeField] private Image slotBImage;

    public Sprite swordSprite;
    public Sprite bowSprite;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slotAImage.sprite = swordSprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory != null)
        {
            string currentWeapon = inventory.GetCurrentWeapon();
            if (currentWeapon == "sword") {
                slotAImage.GetComponent<Image>().enabled = true;
                slotAImage.sprite = swordSprite;
            }
            else if (currentWeapon == "bow") {
                slotAImage.GetComponent<Image>().enabled = true;
                slotAImage.sprite = bowSprite;
            } else {
                slotAImage.GetComponent<Image>().enabled = false;
            }
        }
    }
}
