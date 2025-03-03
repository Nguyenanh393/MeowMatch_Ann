using System.Collections.Generic;
using System.Linq;
using _Pool.Pool;
using UnityEngine;
using UnityEngine.UI;

public class CatDressupUI : PopUpUI
{
    [Header("UI References")]
    [SerializeField] private Transform sidebarContainer;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private Image catHatImage;
    [SerializeField] private Image catNecklaceImage;

    [Header("Data")]
    [SerializeField] private List<ShopItemData> shopItemDatabasesDressUp;

    private ShopItemType _currentItemType;
    private int _previewItemIndex = -1;

    // Collections
    private readonly Dictionary<ShopItemType, List<KeyValuePair<ShopItem, int>>> _ownedItems = new Dictionary<ShopItemType, List<KeyValuePair<ShopItem, int>>>();
    private readonly Dictionary<ShopItemType, ButtonSideBarDressUp> _sidebarButtons = new Dictionary<ShopItemType, ButtonSideBarDressUp>();
    private readonly Dictionary<ShopItemType, List<ButtonItemDressup>> _itemButtons = new Dictionary<ShopItemType, List<ButtonItemDressup>>();
    private readonly HashSet<ShopItemType> _availableItemTypes = new HashSet<ShopItemType>();

    private ButtonSideBarDressUp _currentSelectedSidebar;

    #region Lifecycle Methods

    public void Awake()
    {
       InitializeSidebar();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        // Refresh data every time UI is enabled
        LoadAllOwnedItems();
        InitializeDressup();
        RefreshAllItems();

        if (_sidebarButtons.Count > 0 && _sidebarButtons.ContainsKey(ShopItemType.HAT))
        {
            SelectSidebar(_sidebarButtons[ShopItemType.HAT]);
        }
    }

    #endregion

    #region Initialization Methods

    private void InitializeSidebar()
    {
        _availableItemTypes.Clear();

        // Find all item types from databases
        foreach (ShopItemData database in shopItemDatabasesDressUp)
        {
            if (database == null) continue;

            foreach (ShopItem item in database.items)
            {
                _availableItemTypes.Add(item.itemType);
            }
        }

        // Create sidebar buttons
        foreach (ShopItemType itemType in _availableItemTypes)
        {
            ButtonSideBarDressUp sidebarButton = SimplePool.Spawn<ButtonSideBarDressUp>(
                PoolType.POOLTYPE_BUTTON_ITEMDRESSUP_SIDEBAR, sidebarContainer);
            sidebarButton.Initialize(itemType);
            sidebarButton.gameObject.name = itemType.ToString();
            _sidebarButtons[itemType] = sidebarButton;
        }
    }

    private void InitializeDressup()
    {
        if (_itemButtons.Count > 0) return;

        // Clear existing buttons
        ClearItemContainer();
        _itemButtons.Clear();

        // Create buttons for owned items
        foreach (var kvp in _ownedItems)
        {
            ShopItemType itemType = kvp.Key;
            List<KeyValuePair<ShopItem, int>> items = kvp.Value;
            List<ButtonItemDressup> itemButtons = new List<ButtonItemDressup>(items.Count);

            for (int i = 0; i < items.Count; i++)
            {
                ButtonItemDressup itemButton = CreateItemButton(items[i].Key, i, items[i].Value, itemType);
                itemButtons.Add(itemButton);
                itemButton.gameObject.SetActive(false);
            }

            _itemButtons[itemType] = itemButtons;
        }
    }

    private ButtonItemDressup CreateItemButton(ShopItem item, int localIndex, int databaseIndex, ShopItemType itemType)
    {
        ButtonItemDressup itemButton = SimplePool.Spawn<ButtonItemDressup>(
            PoolType.POOLTYPE_BUTTON_ITEMDRESSUP, itemContainer);

        itemButton.Initialize(item, localIndex, databaseIndex, itemType);

        bool isEquipped = PlayerInventoryManager.Instance.IsItemEquipped(itemType, databaseIndex);
        itemButton.SetEquippedState(isEquipped);

        return itemButton;
    }

    private void ClearItemContainer()
    {
        foreach (Transform child in itemContainer)
        {
            if (child.TryGetComponent(out PoolUnit poolUnit))
                SimplePool.Despawn(poolUnit);
        }
    }

    #endregion

    #region Data Loading

    private void LoadAllOwnedItems()
    {
        _ownedItems.Clear();

        // Load owned items for each available type
        foreach (ShopItemType itemType in _availableItemTypes)
        {
            LoadItemsOfType(itemType);
        }
    }

    private void LoadItemsOfType(ShopItemType itemType)
    {
        List<KeyValuePair<ShopItem, int>> items = new List<KeyValuePair<ShopItem, int>>();

        foreach (var database in PlayerInventoryManager.Instance.ItemDatabases)
        {
            if (database == null) continue;

            for (int i = 0; i < database.items.Count; i++)
            {
                ShopItem item = database.items[i];
                if (item.itemType == itemType && PlayerInventoryManager.Instance.IsItemOwned(itemType, i))
                {
                    items.Add(new KeyValuePair<ShopItem, int>(item, i));
                }
            }
        }

        // Always add the entry, even if empty
        _ownedItems[itemType] = items;
    }

    #endregion

    #region UI Update Methods

    public void RefreshAllItems()
    {
        // Reload data
        LoadAllOwnedItems();

        // Update or recreate buttons
        foreach (var itemType in _itemButtons.Keys.ToList()) // ToList to avoid collection modified exception
        {
            UpdateButtonsOfType(itemType);
        }

        // Update visuals
        ShowEquippedItems();

        // Show correct items based on selection
        if (_currentSelectedSidebar != null)
        {
            ShowItemsByType(_currentSelectedSidebar.ItemType);
        }

        // Update preview
        UpdateEquippedItems();
    }

    private void UpdateButtonsOfType(ShopItemType itemType)
    {
        if (!_itemButtons.TryGetValue(itemType, out List<ButtonItemDressup> buttons) ||
            !_ownedItems.TryGetValue(itemType, out List<KeyValuePair<ShopItem, int>> items))
        {
            return;
        }

        // Same number of buttons as items - update in place
        if (buttons.Count == items.Count)
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if (buttons[i] == null) continue;

                int databaseIndex = items[i].Value;
                bool isEquipped = PlayerInventoryManager.Instance.IsItemEquipped(itemType, databaseIndex);
                buttons[i].SetEquippedState(isEquipped);
            }
            return;
        }

        // Different counts - recreate buttons
        foreach (var button in buttons)
        {
            if (button != null)
            {
                SimplePool.Despawn(button);
            }
        }

        buttons.Clear();

        // Create new buttons
        for (int i = 0; i < items.Count; i++)
        {
            ButtonItemDressup itemButton = CreateItemButton(items[i].Key, i, items[i].Value, itemType);
            buttons.Add(itemButton);
            itemButton.gameObject.SetActive(itemType == _currentItemType);
        }

        _itemButtons[itemType] = buttons;
    }

    private void ShowItemsByType(ShopItemType itemType)
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
        if (_itemButtons.TryGetValue(itemType, out List<ButtonItemDressup> buttons))
        {
            foreach (var button in buttons)
            {
                button.gameObject.SetActive(true);
            }
        }
    }

    #endregion

    #region Visual Update Methods

    private void UpdateItemVisual(Sprite icon)
    {
        // Update the appropriate visual based on item type
        switch (_currentItemType)
        {
            case ShopItemType.HAT:
                catHatImage.sprite = icon;
                catHatImage.enabled = true;
                break;
            case ShopItemType.NECKLACE:
                catNecklaceImage.sprite = icon;
                catNecklaceImage.enabled = true;
                break;
        }
    }

    private void ResetPreviewImages()
    {
        // Reset preview images based on current item type
        switch (_currentItemType)
        {
            case ShopItemType.HAT:
                catHatImage.sprite = null;
                catHatImage.enabled = false;
                break;
            case ShopItemType.NECKLACE:
                catNecklaceImage.sprite = null;
                catNecklaceImage.enabled = false;
                break;
        }
    }

    private void UpdateEquippedItems()
    {
        // Reset images
        catHatImage.sprite = null;
        catHatImage.enabled = false;
        catNecklaceImage.sprite = null;
        catNecklaceImage.enabled = false;

        // Show equipped items
        UpdateEquippedItemOfType(ShopItemType.HAT, catHatImage);
        UpdateEquippedItemOfType(ShopItemType.NECKLACE, catNecklaceImage);
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

    private void ShowEquippedItems()
    {
        // Show currently equipped items
        foreach (var database in PlayerInventoryManager.Instance.ItemDatabases)
        {
            if (database == null) continue;

            for (int i = 0; i < database.items.Count; i++)
            {
                ShopItem item = database.items[i];
                if (item.itemType == _currentItemType && PlayerInventoryManager.Instance.IsItemEquipped(_currentItemType, i))
                {
                    UpdateItemVisual(item.icon);
                    break;
                }
            }
        }
    }

    #endregion

    #region Public Methods

    public void OnClickSidebarButton(ButtonSideBarDressUp sidebarButton)
    {
        SelectSidebar(sidebarButton);
    }

    public void PreviewItem(int localIndex, ShopItem item)
    {
        _previewItemIndex = localIndex;
        UpdateItemVisual(item.icon);
    }

    public void EquipItem(int itemIndex)
    {
        PlayerInventoryManager.Instance.EquipItem(_currentItemType, itemIndex);
        RefreshAllItems();
    }

    #endregion

    #region Helper Methods

    private void SelectSidebar(ButtonSideBarDressUp sidebarButton)
    {
        // Reset previous selection
        if (_currentSelectedSidebar != null)
        {
            _currentSelectedSidebar.ButtonImage.color = _currentSelectedSidebar.NormalColor;
        }

        // Set new selection
        _currentSelectedSidebar = sidebarButton;
        sidebarButton.ButtonImage.color = sidebarButton.SelectedColor;

        // Show only items of selected type
        _currentItemType = sidebarButton.ItemType;
        _previewItemIndex = -1;

        ResetPreviewImages();
        ShowEquippedItems();
        ShowItemsByType(sidebarButton.ItemType);
    }

    #endregion
}
