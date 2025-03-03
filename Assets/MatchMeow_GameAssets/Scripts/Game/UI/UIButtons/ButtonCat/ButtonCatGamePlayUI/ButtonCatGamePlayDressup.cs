using Cysharp.Threading.Tasks;
using MatchMeow_GameAssets.Scripts.Game.UI.UICanvas.CatUI;


public class ButtonCatGamePlayDressup : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        DoWhenClicked();

    }

    private void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<CatGamePlayUI>();
        UIManager.Instance.OpenUI<CatDressupUI>();
    }
}

