using System;
using UnityEngine;
using UnityEngine.UI;

public class CatGamePlayUI : UICanvas
{
    [SerializeField] private Text coinText;
    [SerializeField] private Text heartText;

    [SerializeField] private Image catHatImage;
    [SerializeField] private Image catNecklaceImage;
    [SerializeField] private Image catBedImage;
    [SerializeField] private Image catLitterBoxImage;
    [SerializeField] private Image catFoodBowlImage;

    [SerializeField] private Sprite catBedDefaultImage;

    private void OnEnable()
    {
        SetCoinText();
        SetHeartText();
        UpdateEquippedItems();
    }

    private void SetCoinText()
    {
        int currentCoin = PlayerCurrencyManager.Instance.GetCoins();
        coinText.text = currentCoin.ToString();
    }

    private void SetHeartText()
    {
        int currentHeart = PlayerCurrencyManager.Instance.GetHearts();
        heartText.text = currentHeart.ToString();
    }

    private void UpdateEquippedItems()
    {
        // Reset images
        catHatImage.sprite = null;
        catHatImage.enabled = false;
        catNecklaceImage.sprite = null;
        catNecklaceImage.enabled = false;
        catBedImage.sprite = catBedDefaultImage;
        // catBedImage.enabled = false;

        // Check for equipped hat
        UpdateEquippedItemOfType(ShopItemType.HAT, catHatImage);

        // Check for equipped necklace
        UpdateEquippedItemOfType(ShopItemType.NECKLACE, catNecklaceImage);
        UpdateEquippedItemOfType(ShopItemType.BED, catBedImage);
    }

    private void UpdateEquippedItemOfType(ShopItemType itemType, Image targetImage)
    {
        foreach (var database in PlayerInventoryManager.Instance.ItemDatabases)
        {
            if (database == null) continue;

            for (int i = 0; i < database.items.Count; i++)
            {
                ShopItem item = database.items[i];
                if (item.itemType == itemType && PlayerInventoryManager.Instance.IsItemEquipped(itemType, i))
                {
                    targetImage.sprite = item.icon;
                    targetImage.enabled = true;
                    return;
                }
            }
        }
    }
}
