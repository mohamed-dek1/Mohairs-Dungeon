using UnityEngine;
using UnityEngine.UI;

public class Hearts : MonoBehaviour
{
    [SerializeField] private GameObject[] heartSlots;
    [SerializeField] private Sprite[] heartSprites; // 0 = Full, 1 = Half, 2 = Empty
    [SerializeField] private Health playerHealth;

    void Update()
    {
        if (playerHealth == null) return;

        UpdateHeartsUI(playerHealth.GetCurrentHealth());
    }

    private void UpdateHeartsUI(int health)
    {
        if (health >= 80)
        {
            heartSlots[0].GetComponent<Image>().sprite = heartSprites[0];
            heartSlots[1].GetComponent<Image>().sprite = heartSprites[0];
            heartSlots[2].GetComponent<Image>().sprite = heartSprites[0];
        } 
        else if (health >= 70)
        {
            heartSlots[0].GetComponent<Image>().sprite = heartSprites[0];
            heartSlots[1].GetComponent<Image>().sprite = heartSprites[0];
            heartSlots[2].GetComponent<Image>().sprite = heartSprites[1];
        } 
        else if (health >= 60)
        {
            heartSlots[0].GetComponent<Image>().sprite = heartSprites[0];
            heartSlots[1].GetComponent<Image>().sprite = heartSprites[0];
            heartSlots[2].GetComponent<Image>().sprite = heartSprites[2];
        } 
        else if (health >= 50)
        {
            heartSlots[0].GetComponent<Image>().sprite = heartSprites[0];
            heartSlots[1].GetComponent<Image>().sprite = heartSprites[1];
            heartSlots[2].GetComponent<Image>().sprite = heartSprites[2];
        } 
        else if (health >= 40)
        {
            heartSlots[0].GetComponent<Image>().sprite = heartSprites[0];
            heartSlots[1].GetComponent<Image>().sprite = heartSprites[2];
            heartSlots[2].GetComponent<Image>().sprite = heartSprites[2];
        } 
        else if (health >= 30)
        {
            heartSlots[0].GetComponent<Image>().sprite = heartSprites[0];
            heartSlots[1].GetComponent<Image>().sprite = heartSprites[2];
            heartSlots[2].GetComponent<Image>().sprite = heartSprites[2];
        } 
        else if (health >= 20)
        {
            heartSlots[0].GetComponent<Image>().sprite = heartSprites[1];
            heartSlots[1].GetComponent<Image>().sprite = heartSprites[2];
            heartSlots[2].GetComponent<Image>().sprite = heartSprites[2];
        }
    }
}
