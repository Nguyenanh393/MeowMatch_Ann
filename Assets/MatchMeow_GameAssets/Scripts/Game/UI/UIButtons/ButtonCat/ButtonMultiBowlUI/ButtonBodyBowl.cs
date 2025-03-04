namespace MatchMeow_GameAssets.Scripts.Game.UI.UIButtons.ButtonCat.ButtonMultiBowlUI
{
    public class ButtonBodyBowl : ButtonBaseItemtype
    {
        protected override void DoWhenClicked()
        {
            UIManager.Instance.CloseUI<MultiFunctionBowlUI>();
            base.DoWhenClicked();
        }
    }
}
