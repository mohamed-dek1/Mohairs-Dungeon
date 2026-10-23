using UnityEngine;
using UnityEngine.UI;

public class CurrencyDisplayer : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField] private GameObject[] coinsDigits;
    [SerializeField] private GameObject[] keysDigits;
    [SerializeField] private GameObject[] bombsDigits;
    [SerializeField] private Sprite[] digitSprites;

    // Update is called once per frame
    void Update()
    {
        if (inventory != null)
        {
            UpdateDigits(inventory.Coins, coinsDigits);
            UpdateDigits(inventory.Keys, keysDigits);
            UpdateDigits(inventory.Bombs, bombsDigits);
        }
    }

    private void UpdateDigits(int value, GameObject[] digits)
    {
        int tens = value / 10 % 10;
        int ones = value % 10;

        digits[0].GetComponent<Image>().sprite = digitSprites[tens];
        digits[1].GetComponent<Image>().sprite = digitSprites[ones];
    }
}
