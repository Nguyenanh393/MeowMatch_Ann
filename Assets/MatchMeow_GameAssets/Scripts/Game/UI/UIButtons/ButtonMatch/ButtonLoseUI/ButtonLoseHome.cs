public class ButtonLoseHome : ButtonBaseHome
{
    protected override void DoWhenClicked()
    {
        base.DoWhenClicked();
        LevelManager.Instance.ReloadLevel();
    }
}

