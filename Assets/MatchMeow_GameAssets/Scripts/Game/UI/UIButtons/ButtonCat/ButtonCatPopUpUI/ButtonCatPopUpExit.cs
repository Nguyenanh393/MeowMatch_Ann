using System;
using Cysharp.Threading.Tasks;
using MatchMeow_GameAssets.Scripts.Game.UI.UICanvas.CatUI;

public class ButtonCatPopUpExit : ButtonBaseExit
{

    protected override void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<CatPopUpUI>();
        base.DoWhenClicked();
    }
}

