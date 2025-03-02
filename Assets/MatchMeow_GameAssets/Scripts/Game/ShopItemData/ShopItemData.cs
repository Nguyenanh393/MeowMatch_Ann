using UnityEngine;
using System.Collections.Generic;

public enum ShopItemType
{
    FOOD,
    CAT_SAND,
    HAT,
    NECKLACE,
    LITTER_BOX,
    BOWL,
    BED
}

// Individual shop item class
[System.Serializable]
public class ShopItem
{
    public ShopItemType itemType;
    public string displayName;
    public Sprite icon;
    public int price;
    public bool canBuyMultiple;
    public string description;
    public bool isCoinCurrency;
}

// Scriptable object that holds a collection of shop items
[CreateAssetMenu(fileName = "ShopItemDatabase", menuName = "Shop/ShopItemDatabase")]
public class ShopItemData : ScriptableObject
{
    public List<ShopItem> items = new List<ShopItem>();

    // Helper method to find item by type
    public ShopItem GetItemByType(ShopItemType type)
    {
        return items.Find(item => item.itemType == type);
    }
}

