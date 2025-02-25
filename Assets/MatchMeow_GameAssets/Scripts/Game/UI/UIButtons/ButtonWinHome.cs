namespace MatchMeow_GameAssets.Scripts.Game.UI.UIButtons
{
    public class ButtonWinHome : ButtonBaseHome
    {
        protected override void DoWhenClicked()
        {
            base.DoWhenClicked();
            LevelManager.Instance.LoadNextLevel();
        }
    }
}
