using UnityEngine;
using UnityEngine.UI;

public class ItemSlots : MonoBehaviour
{
    public Inventory inventory;

    [SerializeField] private Image slotAImage;
    [SerializeField] private Image slotBImage;

    public Sprite swordSprite;
    public Sprite bowSprite;
    public Sprite bombSprite;
    
    
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
            ItemType itemType = inventory.CurrentItem;
            switch (itemType)
            {
                case ItemType.Bomb:
                    slotBImage.GetComponent<Image>().enabled = true;
                    slotBImage.sprite = bombSprite;
                    break;
                case ItemType.Bow:
                    slotBImage.GetComponent<Image>().enabled = true;
                    slotBImage.sprite = bowSprite;
                    break;
                default:
                    slotBImage.GetComponent<Image>().enabled = false;
                    slotBImage.sprite = null;
                    break;
            }
            
            itemType = inventory.CurrentWeapon;
            switch (itemType)
            {
                case ItemType.Sword:
                    slotAImage.GetComponent<Image>().enabled = true;
                    slotAImage.sprite = swordSprite;
                    break;
                case ItemType.Bow:
                    slotAImage.GetComponent<Image>().enabled = true;
                    slotAImage.sprite = bowSprite;
                    break;
                case ItemType.Bomb:
                    slotAImage.GetComponent<Image>().enabled = true;
                    slotAImage.sprite = bombSprite;
                    break;
                default:
                    slotAImage.GetComponent<Image>().enabled = false;
                    slotAImage.sprite = null;
                    break;
            }
        }
    }
}
