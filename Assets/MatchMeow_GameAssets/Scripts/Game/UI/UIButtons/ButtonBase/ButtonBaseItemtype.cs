using Cysharp.Threading.Tasks;
using MatchMeow_GameAssets.Scripts.Game.UI.UICanvas.CatUI;
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
        if (buttonImage != null)
        {
            buttonImage.sprite = itemIcon;
        }
        else
        {
            UIManager.Instance.GetUI<CatGamePlayUI>().GetObjectImage(itemType).sprite = itemIcon;
        }
    }
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        // LevelManager.Instance.ReloadLevel();
        DoWhenClicked();

    }

    protected virtual void DoWhenClicked()
    {
        UIManager.Instance.OpenUI<CatPopUpUI>();

        UIManager.Instance.GetUI<CatPopUpUI>().ItemType = ItemType;

        UIManager.Instance.GetUI<CatPopUpUI>().LoadOwnedItems();

        UIManager.Instance.GetUI<CatPopUpUI>().SetBaseButton(this);
    }
}

