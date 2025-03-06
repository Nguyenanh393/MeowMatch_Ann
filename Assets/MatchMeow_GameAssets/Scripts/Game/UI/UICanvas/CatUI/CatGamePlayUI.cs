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
    [SerializeField] private Image catBodyBowlImage;
    [SerializeField] private Image catBodyLitterBoxImage;

    [SerializeField] private Sprite catBodyBowlDefaultImage;
    [SerializeField] private Sprite catBedDefaultImage;
    [SerializeField] private Sprite catLitterBoxDefaultImage;


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
        catFoodBowlImage.sprite = null;
        catFoodBowlImage.enabled = false;

        catBedImage.sprite = catBedDefaultImage;
        // catBedImage.enabled = false;
        catBodyBowlImage.sprite = catBodyBowlDefaultImage;
        // catBodyBowlImage.enabled = false;
        catBodyLitterBoxImage.sprite = catLitterBoxDefaultImage;
        // Check for equipped hat
        UpdateEquippedItemOfType(ShopItemType.HAT, catHatImage);
        // Check for equipped necklace
        UpdateEquippedItemOfType(ShopItemType.NECKLACE, catNecklaceImage);
        UpdateEquippedItemOfType(ShopItemType.BED, catBedImage);
        UpdateEquippedItemOfType(ShopItemType.BOWL, catBodyBowlImage);
        UpdateEquippedItemOfType(ShopItemType.LITTER_BOX, catLitterBoxImage);
        UpdateEquippedItemOfType(ShopItemType.FOOD, catFoodBowlImage);
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

    public Image GetObjectImage(ShopItemType itemType)
    {
        return itemType switch
        {
            ShopItemType.HAT => catHatImage,
            ShopItemType.NECKLACE => catNecklaceImage,
            ShopItemType.BED => catBedImage,
            ShopItemType.LITTER_BOX => catLitterBoxImage,
            ShopItemType.FOOD => catFoodBowlImage,
            ShopItemType.BOWL => catBodyBowlImage,
            _ => null
        };
    }
}
