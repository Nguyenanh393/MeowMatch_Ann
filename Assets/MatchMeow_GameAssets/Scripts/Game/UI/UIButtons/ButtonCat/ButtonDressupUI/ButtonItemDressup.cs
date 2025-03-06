using _Pool.Pool;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonItemDressup : PoolUnit
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Button previewButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private GameObject equippedIndicator;
    [SerializeField] private Text nameText;

    private ShopItem _item;
    private int _localIndex;
    private int _databaseIndex;
    private ShopItemType _itemType;

    public void Initialize(ShopItem item, int localIndex, int databaseIndex, ShopItemType itemType)
    {
        // (item, i, itemIndex, _currentItemType
        _item = item;
        _localIndex = localIndex;
        _databaseIndex = databaseIndex;
        _itemType = itemType;

        itemImage.sprite = item.icon;
        nameText.text = item.displayName;
    }

    public void SetPreviewAction()
    {
        SoundManager.Instance.PlayButtonSound();
        UIManager.Instance.GetUI<CatDressupUI>().PreviewItem(_localIndex, _item);
    }

    public void SetEquipAction()
    {
        SoundManager.Instance.PlayButtonSound();
        UIManager.Instance.GetUI<CatDressupUI>().EquipItem(_databaseIndex);
    }

    public void SetEquippedState(bool equipped)
    {
        equippedIndicator.SetActive(equipped);
        equipButton.interactable = !equipped;
    }
}
