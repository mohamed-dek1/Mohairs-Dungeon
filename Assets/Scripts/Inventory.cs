using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    int rupee_count = 0;
    private string currentWeapon = "sword";

    public void AddRupees(int num_rupees)
    {
        rupee_count += num_rupees;
    }

    public void SetCurrentWeapon(string weapon)
    {
        currentWeapon = weapon;
    }

    public string GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public int GetRupees()
    {
        return rupee_count;
    }
}
