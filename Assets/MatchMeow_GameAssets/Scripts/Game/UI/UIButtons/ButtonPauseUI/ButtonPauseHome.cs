public class ButtonPauseHome : ButtonBaseHome
{
    protected override void DoWhenClicked()
    {
        base.DoWhenClicked();
        LevelManager.Instance.ReloadLevel();
    }
}

