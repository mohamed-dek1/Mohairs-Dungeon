using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private string currentWeapon = "sword";

    [SerializeField] private int rupees = 0;
    [SerializeField] private int keys = 0;

    public void AddRupees(int amount)
    {
        rupees += amount;
    }

    public void AddKey()
    {
        keys++;
    }

    public bool UseKey()
    {
        if (keys <= 0)
            return false;

        keys--;
        return true;
    }

    public int GetRupees()
    {
        return rupees;
    }

    public int GetKeys()
    {
        return keys;
    }

    public void SetCurrentWeapon(string weapon)
    {
        currentWeapon = weapon;
    }

    public string GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
