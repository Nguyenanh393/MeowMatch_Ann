using MatchMeow_GameAssets.Scripts.Game.UI.UIButtons.ButtonBase;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSideBarDressUp : ButtonBaseSideBar
{
    [SerializeField] private Text categoryText;
    [SerializeField] private Image categoryIcon;


    public void Initialize(ShopItemType itemType)
    {
        ItemType = itemType;
        ButtonNameText.text = ItemType.ToString();


    }

    public void OnClick()
    {
        SoundManager.Instance.PlayButtonSound();
        UIManager.Instance.GetUI<CatDressupUI>().OnClickSidebarButton(this);
    }
}
