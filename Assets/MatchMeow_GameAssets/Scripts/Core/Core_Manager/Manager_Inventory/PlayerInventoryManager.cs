using UnityEngine;
using System.Collections.Generic;

public class PlayerInventoryManager : Singleton<PlayerInventoryManager>
{
    // Track items using <ItemType, <ItemIndex, Count>>
    private Dictionary<ShopItemType, Dictionary<int, int>> itemQuantities = new Dictionary<ShopItemType, Dictionary<int, int>>();
    private Dictionary<ShopItemType, Dictionary<int, bool>> equippedItems = new Dictionary<ShopItemType, Dictionary<int, bool>>();

    [SerializeField] private List<ShopItemData> itemDatabases;

    public List<ShopItemData> ItemDatabases => itemDatabases;

    private void Awake()
    {
        InitializeInventory();
        LoadInventory();
    }

    private void InitializeInventory()
    {
        // First initialize all dictionaries
        foreach (ShopItemType type in System.Enum.GetValues(typeof(ShopItemType)))
        {
            if (!itemQuantities.ContainsKey(type))
            {
                itemQuantities[type] = new Dictionary<int, int>();
                equippedItems[type] = new Dictionary<int, bool>();
            }
        }

        // Initialize items from databases while preserving PlayerPrefs data
        foreach (var database in itemDatabases)
        {
            if (database == null) continue;

            for (int i = 0; i < database.items.Count; i++)
            {
                var item = database.items[i];
                ShopItemType type = item.itemType;

                // Make sure the dictionary for this type exists
                if (!itemQuantities.ContainsKey(type))
                {
                    itemQuantities[type] = new Dictionary<int, int>();
                    equippedItems[type] = new Dictionary<int, bool>();
                }

                // Load saved data from PlayerPrefs if it exists, or initialize to 0 if not
                int savedQuantity = PlayerPrefs.GetInt($"Item_{type}_{i}", 0);
                bool savedEquipState = PlayerPrefs.GetInt($"Equipped_{type}_{i}", 0) == 1;

                // Add or update the entry
                itemQuantities[type][i] = savedQuantity;
                equippedItems[type][i] = savedEquipState;
            }
        }
    }

    // Purchase an item using type and index
    public bool PurchaseItem(ShopItemType itemType, int itemIndex, int price, bool canBuyMultiple, bool isCoinCurrency)
    {
        // Check if player has enough currency
        bool hasEnoughCurrency = isCoinCurrency ?
            PlayerCurrencyManager.Instance.HasEnoughCoins(price) :
            PlayerCurrencyManager.Instance.HasEnoughHearts(price);

        if (!hasEnoughCurrency)
            return false;

        // Deduct the appropriate currency
        if (isCoinCurrency)
        {
            PlayerCurrencyManager.Instance.SpendCoins(price);
        }
        else
        {
            PlayerCurrencyManager.Instance.SpendHearts(price);
        }

        // Initialize if needed
        if (!itemQuantities.ContainsKey(itemType))
        {
            itemQuantities[itemType] = new Dictionary<int, int>();
            equippedItems[itemType] = new Dictionary<int, bool>();
        }

        if (!itemQuantities[itemType].ContainsKey(itemIndex))
        {
            itemQuantities[itemType][itemIndex] = 0;
            equippedItems[itemType][itemIndex] = false;
        }

        if (canBuyMultiple || itemQuantities[itemType][itemIndex] == 0)
        {
            itemQuantities[itemType][itemIndex]++;
            SaveInventory();
            return true;
        }

        return false;
    }

    // For backwards compatibility
    public bool PurchaseItem(ShopItemType itemType, int price, bool canBuyMultiple, bool isCoinCurrency)
    {
        // Use index 0 for backwards compatibility
        return PurchaseItem(itemType, 0, price, canBuyMultiple, isCoinCurrency);
    }

    public void EquipItem(ShopItemType itemType, int itemIndex)
    {
        if (!itemQuantities.ContainsKey(itemType) || !itemQuantities[itemType].ContainsKey(itemIndex))
            return;

        if (itemQuantities[itemType][itemIndex] > 0)
        {
            // Check if this is a consumable item
            bool isConsumable = IsConsumableItem(itemType);

            // For consumable items, reduce the quantity
            if (isConsumable)
            {
                itemQuantities[itemType][itemIndex]--;
                SaveInventory();
                return;
            }

            // Initialize dictionary if needed
            if (!equippedItems.ContainsKey(itemType))
            {
                equippedItems[itemType] = new Dictionary<int, bool>();
            }

            // Unequip all items by creating a copy of keys first
            var keysCopy = new List<int>(equippedItems[itemType].Keys);
            foreach (var index in keysCopy)
            {
                equippedItems[itemType][index] = false;
            }

            // Equip the new item
            equippedItems[itemType][itemIndex] = true;
            SaveInventory();
        }
    }

// Helper method to determine if an item is consumable
    private bool IsConsumableItem(ShopItemType itemType)
    {
        // We'll consider certain types as consumable items
        switch (itemType)
        {
            case ShopItemType.FOOD:
            // case ShopItemType.TREAT:
                return true;
            default:
                return false;
        }
    }

    public void UnequipItem(ShopItemType itemType, int itemIndex)
    {
        if (equippedItems.ContainsKey(itemType) && equippedItems[itemType].ContainsKey(itemIndex))
        {
            equippedItems[itemType][itemIndex] = false;
            SaveInventory();
        }
    }

    // Backwards compatibility methods
    public void EquipItem(ShopItemType itemType)
    {
        EquipItem(itemType, 0);
    }

    public void UnequipItem(ShopItemType itemType)
    {
        UnequipItem(itemType, 0);
    }

    public bool IsItemOwned(ShopItemType itemType, int itemIndex)
    {
        return itemQuantities.ContainsKey(itemType) &&
               itemQuantities[itemType].ContainsKey(itemIndex) &&
               itemQuantities[itemType][itemIndex] > 0;
    }

    public bool IsItemEquipped(ShopItemType itemType, int itemIndex)
    {
        return equippedItems.ContainsKey(itemType) &&
               equippedItems[itemType].ContainsKey(itemIndex) &&
               equippedItems[itemType][itemIndex];
    }

    public int GetItemQuantity(ShopItemType itemType, int itemIndex)
    {
        if (!itemQuantities.ContainsKey(itemType) || !itemQuantities[itemType].ContainsKey(itemIndex))
            return 0;

        return itemQuantities[itemType][itemIndex];
    }

    // Backwards compatibility methods
    public bool IsItemOwned(ShopItemType itemType)
    {
        return IsItemOwned(itemType, 0);
    }

    public bool IsItemEquipped(ShopItemType itemType)
    {
        return IsItemEquipped(itemType, 0);
    }

    public int GetItemQuantity(ShopItemType itemType)
    {
        return GetItemQuantity(itemType, 0);
    }

    private void SaveInventory()
    {
        foreach (var typeEntry in itemQuantities)
        {
            foreach (var indexEntry in typeEntry.Value)
            {
                PlayerPrefs.SetInt($"Item_{typeEntry.Key}_{indexEntry.Key}", indexEntry.Value);
            }
        }

        foreach (var typeEntry in equippedItems)
        {
            foreach (var indexEntry in typeEntry.Value)
            {
                PlayerPrefs.SetInt($"Equipped_{typeEntry.Key}_{indexEntry.Key}", indexEntry.Value ? 1 : 0);
            }
        }

        PlayerPrefs.Save();
    }

    private void LoadInventory()
    {
        foreach (var database in itemDatabases)
        {
            if (database == null) continue;

            for (int i = 0; i < database.items.Count; i++)
            {
                var item = database.items[i];
                ShopItemType type = item.itemType;

                if (!itemQuantities.ContainsKey(type))
                {
                    itemQuantities[type] = new Dictionary<int, int>();
                    equippedItems[type] = new Dictionary<int, bool>();
                }

                itemQuantities[type][i] = PlayerPrefs.GetInt($"Item_{type}_{i}", 0);
                equippedItems[type][i] = PlayerPrefs.GetInt($"Equipped_{type}_{i}", 0) == 1;
            }
        }
    }
}
