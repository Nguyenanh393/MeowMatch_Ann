using System;
using UnityEngine;
using System.Collections.Generic;
using _Pool.Pool;
using MatchMeow_GameAssets.Scripts.Game.UI.UIButtons.ButtonShopUI;
using UnityEngine.UI;

public class ShopUI : PopUpUI
{
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private Transform sideBarItemsContainer;
    [SerializeField] private ButtonShopItem itemButtonPrefab;
    [SerializeField] private ButtonSideBar sideBarButtonPrefab;
    [SerializeField] private List<ShopItemData> shopItemDatabases;

    [SerializeField] private Text coinText;
    [SerializeField] private Text heartText;

    private Dictionary<ShopItemType, List<ButtonShopItem>> _itemButtons = new Dictionary<ShopItemType, List<ButtonShopItem>>();
    private Dictionary<ShopItemType, ButtonSideBar> _sidebarButtons = new Dictionary<ShopItemType, ButtonSideBar>();
    private ButtonSideBar _currentSelectedSidebar;
    private HashSet<ShopItemType> _availableItemTypes = new HashSet<ShopItemType>();

    public void Awake()
    {
        InitializeSidebar();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetCoinText();
        SetHeartText();

        InitializeShop();
        RefreshAllItems();
        if (_sidebarButtons.Count > 0)
        {
            SelectSidebar(_sidebarButtons[ShopItemType.FOOD]);
        }
    }

    public void SetCoinText()
    {
        int currentCoin = PlayerCurrencyManager.Instance.GetCoins();
        coinText.text = currentCoin.ToString();
    }

    public void SetHeartText()
    {
        int currentHeart = PlayerCurrencyManager.Instance.GetHearts();
        heartText.text = currentHeart.ToString();
    }

    private void InitializeSidebar()
    {

        // Find all unique item types from databases
        foreach (ShopItemData database in shopItemDatabases)
        {
            if (database == null) continue;

            foreach (ShopItem item in database.items)
            {
                _availableItemTypes.Add(item.itemType);
            }
        }

        // Create sidebar buttons for each unique item type
        foreach (ShopItemType itemType in _availableItemTypes)
        {
            ButtonSideBar sidebarButton = SimplePool.Spawn<ButtonSideBar>(sideBarButtonPrefab, sideBarItemsContainer);
            sidebarButton.ItemType = itemType;
            sidebarButton.gameObject.name = itemType.ToString(); // Rename GameObject
            sidebarButton.ButtonNameText.text = itemType.ToString(); // Set the button text
            _sidebarButtons[itemType] = sidebarButton;
        }

    }

    private void InitializeShop()
    {
        if (_itemButtons.Count > 0) return;

        // Clear any existing buttons
        foreach (Transform child in itemsContainer)
        {
            if (child.TryGetComponent(out PoolUnit poolUnit))
                SimplePool.Despawn(poolUnit);
        }

        _itemButtons.Clear();

        // Create a button for each item in each database
        foreach (ShopItemData database in shopItemDatabases)
        {
            if (database == null) continue;

            for (int i = 0; i < database.items.Count; i++)
            {
                ShopItem item = database.items[i];
                ButtonShopItem buttonItem = SimplePool.Spawn<ButtonShopItem>(itemButtonPrefab, itemsContainer);
                buttonItem.Initialize(item, i);
                buttonItem.gameObject.SetActive(false); // Hide initially

                if (!_itemButtons.ContainsKey(item.itemType))
                {
                    _itemButtons[item.itemType] = new List<ButtonShopItem> { buttonItem };
                }
                else
                {
                    _itemButtons[item.itemType].Add(buttonItem);
                }
            }
        }
    }

    public void OnClickButtonSideBar(ButtonSideBar buttonSideBar)
    {
        SelectSidebar(buttonSideBar);
    }

    private void SelectSidebar(ButtonSideBar buttonSideBar)
    {
        // Reset previous selection
        if (_currentSelectedSidebar != null)
        {
            _currentSelectedSidebar.ButtonImage.color = _currentSelectedSidebar.NormalColor;
        }

        // Set new selection
        _currentSelectedSidebar = buttonSideBar;
        buttonSideBar.ButtonImage.color = buttonSideBar.SelectedColor;

        // Show only items of selected type
        ShowItemsByType(buttonSideBar.ItemType);
    }

    private void ShowItemsByType(ShopItemType selectedType)
    {
        // Hide all items
        foreach (var buttonList in _itemButtons.Values)
        {
            foreach (var button in buttonList)
            {
                button.gameObject.SetActive(false);
            }
        }

        // Show only items of selected type
        if (_itemButtons.TryGetValue(selectedType, out List<ButtonShopItem> buttons))
        {
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(true);
            }
        }
    }

    public void RefreshAllItems()
    {
        foreach (var buttonList in _itemButtons.Values)
        {
            foreach (var button in buttonList)
            {
                if (button != null)
                {
                    button.UpdateItemUI();
                }
            }
        }
    }

    public void RefreshItemsByType(ShopItemType itemType)
    {
        if (_itemButtons.TryGetValue(itemType, out List<ButtonShopItem> buttons))
        {
            foreach (var button in buttons)
            {
                if (button != null)
                {
                    button.UpdateItemUI();
                }
            }
        }
    }
}
