using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public enum ItemType
{
    None,
    Sword,
    Bow,
    Bomb,
    Boomerang
}

public class Inventory : MonoBehaviour
{
    [SerializeField] private int keys = 0;
    [SerializeField] private int coins = 0;
    [SerializeField] private int bombs = 0;

    [SerializeField] private ItemType currentWeapon = ItemType.Sword;
    [SerializeField] private ItemType currentItem = ItemType.None;

    private HashSet<ItemType> unlockedItems = new HashSet<ItemType>();

    public void Start()
    {
        unlockedItems.Add(ItemType.Sword);
    }

    public void Update()
    {
        if (GameManager.god_mode)
        {
            coins = 99;
            bombs = 99;
            keys = 99;
        }

        UnlockItem(ItemType.Bow);

        if (currentItem == ItemType.Bow)
        {
            SetCurrentItem(ItemType.Bow);
        }
    }

    // UNLOCK AND HAS ITEM //

    public void UnlockItem(ItemType item)
    {
        if (!unlockedItems.Contains(item))
            unlockedItems.Add(item);
    }

    public bool HasItem(ItemType item) {
        return unlockedItems.Contains(item);
    }
    
    // CURRENT WEAPON AND ITEM //

    public ItemType CurrentWeapon {
        get { return currentWeapon; }
    }

    public ItemType CurrentItem {
        get { return currentItem; }
    }

    public void SetCurrentWeapon(ItemType weapon)
    {
        currentWeapon = weapon;
    }

    public void SetCurrentItem(ItemType item)
    {
        if (item == ItemType.None || HasItem(item))
            currentItem = item;
    }

    // COINS AND BOMBS //

    public int Coins {
        get { return coins; }
    }

    public int Bombs {
        get { return bombs; }
    }

    public void AddCoins(int amount) {
        coins += amount;
        if (coins > 99)
        {
            coins = 99;
        }
    }
    public bool UseArrow()
    {
        if (coins <= 0) return false;
        coins--;
        return true;
    }

    public bool UseCoin(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            return true;
        }
        return false;
    }

    public void AddBombs(int amount) {
        bombs += amount;

        if (bombs > 99)
        {
            bombs = 99;
        }
    }
    public bool UseBomb()
    {
        if (bombs <= 0) return false;
        bombs--;
        return true;
    }

    // KEYS //

    public int Keys {
        get { return keys; }
    }

    public void AddKey() {
        keys++;
        if (keys > 99)
        {
            keys = 99;
        }
    }

    public bool UseKey()
    {
        if (keys <= 0) return false;
        keys--;
        return true;
    }
}
