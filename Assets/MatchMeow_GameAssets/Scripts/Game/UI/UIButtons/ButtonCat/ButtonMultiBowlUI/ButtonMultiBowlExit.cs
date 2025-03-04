
using MatchMeow_GameAssets.Scripts.Game.UI.UICanvas.CatUI;

public class ButtonMultiBowlExit : ButtonBaseExit
{

    protected override void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<MultiFunctionBowlUI>();
        base.DoWhenClicked();
    }
}
