using System.Collections.Generic;
using MatchMeow_GameAssets.Scripts.Game.UI.UIButtons.ButtonCat.ButtonCatGamePlayUI;
using UnityEngine;
using UnityEngine.UI;

namespace MatchMeow_GameAssets.Scripts.Game.UI.UICanvas.CatUI
{
    public class CatPopUpUI : PopUpUI
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private Text itemNameText;
        [SerializeField] private Text itemDescriptionText;
        [SerializeField] private Text itemQuantityText;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button prevButton;
        [SerializeField] private Button equipButton;
        [SerializeField] private List<ShopItemData> shopItemDatabases;

        private ShopItemType _itemType;
        private Dictionary<ShopItemType, List<KeyValuePair<ShopItem, int>>> _allOwnedItems = new Dictionary<ShopItemType, List<KeyValuePair<ShopItem, int>>>();
        private List<KeyValuePair<ShopItem, int>> _currentItems = new List<KeyValuePair<ShopItem, int>>();
        private int _currentItemIndex = 0;
        private ButtonBaseItemtype _baseButton;

        private void Awake()
        {
            LoadAllOwnedItems();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (nextButton != null)
                nextButton.onClick.AddListener(ShowNextItem);

            if (prevButton != null)
                prevButton.onClick.AddListener(ShowPreviousItem);

            if (equipButton != null)
                equipButton.onClick.AddListener(EquipCurrentItem);
        }

        protected void OnDisable()
        {
            // base.OnDisable();

            if (nextButton != null)
                nextButton.onClick.RemoveListener(ShowNextItem);

            if (prevButton != null)
                prevButton.onClick.RemoveListener(ShowPreviousItem);

            if (equipButton != null)
                equipButton.onClick.RemoveListener(EquipCurrentItem);
        }

        // Load all owned items from all types
        private void LoadAllOwnedItems()
        {
            _allOwnedItems.Clear();

            foreach (var database in shopItemDatabases)
            {
                if (database == null) continue;

                for (int i = 0; i < database.items.Count; i++)
                {
                    ShopItem item = database.items[i];
                    if (PlayerInventoryManager.Instance.IsItemOwned(item.itemType, i))
                    {
                        if (!_allOwnedItems.ContainsKey(item.itemType))
                        {
                            _allOwnedItems[item.itemType] = new List<KeyValuePair<ShopItem, int>>();
                        }
                        _allOwnedItems[item.itemType].Add(new KeyValuePair<ShopItem, int>(item, i));
                    }
                }
            }
        }

        // Public method to refresh all items
        public void LoadOwnedItems()
        {
            LoadAllOwnedItems();
            ShowItemsByType(_itemType);
        }

        public ShopItemType ItemType
        {
            get => _itemType;
            set
            {
                ShowItemsByType(value);
            }
        }

        // Show items of selected type
        public void ShowItemsByType(ShopItemType itemType)
        {
            _itemType = itemType;
            _currentItemIndex = 0;

            _currentItems.Clear();

            if (_allOwnedItems.ContainsKey(itemType))
            {
                _currentItems.AddRange(_allOwnedItems[itemType]);
            }

            UpdateItemDisplay();
        }

        private void ShowNextItem()
        {
            if (_currentItems.Count == 0)
                return;

            _currentItemIndex++;
            if (_currentItemIndex >= _currentItems.Count)
                _currentItemIndex = 0; // Circular navigation

            UpdateItemDisplay();
        }

        private void ShowPreviousItem()
        {
            if (_currentItems.Count == 0)
                return;

            _currentItemIndex--;
            if (_currentItemIndex < 0)
                _currentItemIndex = _currentItems.Count - 1; // Circular navigation

            UpdateItemDisplay();
        }

        private void UpdateItemDisplay()
        {
            if (_currentItems.Count == 0)
            {
                // No items owned
                itemNameText.text = "No items owned";
                itemDescriptionText.text = "";
                itemQuantityText.gameObject.SetActive(false);
                itemImage.sprite = null;

                if (equipButton != null)
                    equipButton.gameObject.SetActive(false);

                return;
            }

            var currentItem = _currentItems[_currentItemIndex];
            ShopItem item = currentItem.Key;
            int itemIndex = currentItem.Value;

            // Update UI elements
            itemNameText.text = item.displayName;
            itemDescriptionText.text = item.description;
            itemImage.sprite = item.icon;

            // Show quantity if item can be bought multiple times
            int quantity = PlayerInventoryManager.Instance.GetItemQuantity(_itemType, itemIndex);
            if (item.canBuyMultiple && quantity > 1)
            {
                itemQuantityText.text = $"x{quantity}";
                itemQuantityText.gameObject.SetActive(true);
            }
            else
            {
                itemQuantityText.gameObject.SetActive(false);
            }

            // _baseButton.SetImage(item.icon);

            // Update equip button state
            if (equipButton != null)
            {
                bool isEquipped = PlayerInventoryManager.Instance.IsItemEquipped(_itemType, itemIndex);
                equipButton.GetComponentInChildren<Text>().text = isEquipped ? "Equipped" : "Equip";
                equipButton.gameObject.SetActive(true);
                equipButton.interactable = !isEquipped;

            }
        }

        private void EquipCurrentItem()
        {
            if (_currentItems.Count == 0 || _currentItemIndex >= _currentItems.Count)
                return;

            int itemIndex = _currentItems[_currentItemIndex].Value;
            var currentItem = _currentItems[_currentItemIndex].Key;
            PlayerInventoryManager.Instance.EquipItem(_itemType, itemIndex);

            // Update UI after equipping
            UpdateItemDisplay();

            // Update the base button's image to match the equipped item
            if (_baseButton != null && _baseButton.ItemType == _itemType)
            {
                _baseButton.SetImage(currentItem.icon);
            }

            // Find and update all buttons of this item type in the scene
            // ButtonBaseItemtype[] allItemTypeButtons = FindObjectsOfType<ButtonBaseItemtype>();
            // foreach (var button in allItemTypeButtons)
            // {
            //     if (button.ItemType == _itemType)
            //     {
            //         button.SetImage(currentItem.icon);
            //     }
            // }

            UIManager.Instance.CloseUI<CatPopUpUI>();
        }


        public void SetBaseButton(ButtonBaseItemtype buttonCatGamePlayBowl)
        {
            _baseButton = buttonCatGamePlayBowl;
        }

    }
}
