using UnityEngine;
using UnityEngine.UI;

public class ButtonBaseItemtype : ButtonBase
{
    [SerializeField] private ShopItemType itemType;
    [SerializeField] private Image buttonImage;

    public ShopItemType ItemType => itemType;
    // public Image ButtonImage => buttonImage;

    public void SetImage(Sprite itemIcon)
    {
        buttonImage.sprite = itemIcon;
    }
}

