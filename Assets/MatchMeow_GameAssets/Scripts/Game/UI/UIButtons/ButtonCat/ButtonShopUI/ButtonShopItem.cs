using _Pool.Pool;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class ButtonShopItem : PoolUnit
{
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text quantityText;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image currencyIcon;
    [SerializeField] private GameObject buyButtonObj;

    [SerializeField] private Sprite coinImage;
    [SerializeField] private Sprite heartImage;

    private ShopItem _item;
    private ButtonEquip _equipButton;
    private int _itemIndex;

    public void Initialize(ShopItem item, int itemIndex)
    {
        _item = item;
        _itemIndex = itemIndex;
        itemNameText.text = item.displayName;
        priceText.text = item.price.ToString();
        itemIcon.sprite = item.icon;

        UpdateItemUI();
    }

    public void UpdateItemUI()
    {
        bool isOwned = PlayerInventoryManager.Instance.IsItemOwned(_item.itemType, _itemIndex);
        // bool isEquipped = PlayerInventoryManager.Instance.IsItemEquipped(_item.itemType, _itemIndex);

        if (_item.canBuyMultiple)
        {
            int quantity = PlayerInventoryManager.Instance.GetItemQuantity(_item.itemType, _itemIndex);
            quantityText.text = "x" + quantity;
            quantityText.gameObject.SetActive(true);
            buyButtonObj.SetActive(true);
        }
        else
        {
            quantityText.gameObject.SetActive(false);
            if (isOwned)
            {
                buyButtonObj.SetActive(false);
            }
            else
            {
                buyButtonObj.SetActive(true);
            }
        }
        bool itemCoinCurrency = _item.isCoinCurrency;

        if (itemCoinCurrency)
        {
            currencyIcon.sprite = coinImage;
        } else
        {
            currencyIcon.sprite = heartImage;
        }
    }

    public void OnClickUniTask()
    {
        // Check if player has enough currency before attempting purchase
        bool hasEnoughCurrency = _item.isCoinCurrency ?
            PlayerCurrencyManager.Instance.HasEnoughCoins(_item.price) :
            PlayerCurrencyManager.Instance.HasEnoughHearts(_item.price);

        if (!hasEnoughCurrency)
        {
            Debug.Log($"Not enough {(_item.isCoinCurrency ? "coins" : "hearts")} to purchase {_item.displayName}");
            // Optional: Show notification to player
            return;
        }

        bool success = PlayerInventoryManager.Instance.PurchaseItem(
            _item.itemType,
            _itemIndex,
            _item.price,
            _item.canBuyMultiple,
            _item.isCoinCurrency
        );

        Debug.Log($"PurchaseItem: {success}");

        if (success)
        {
            UpdateItemUI();
            if (_item.isCoinCurrency)
            {
                UIManager.Instance.GetUI<ShopUI>().SetCoinText();
            }
            else
            {
                UIManager.Instance.GetUI<ShopUI>().SetHeartText();
            }
        }
    }
}
