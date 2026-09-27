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

    // health is in half hearts, so each heart slot holds 2
    private void UpdateHeartsUI(int health)
    {
        for (int i = 0; i < heartSlots.Length; i++)
        {
            int halves = Mathf.Clamp(health - i * 2, 0, 2);
            heartSlots[i].GetComponent<Image>().sprite = heartSprites[2 - halves];
        }
    }
}
