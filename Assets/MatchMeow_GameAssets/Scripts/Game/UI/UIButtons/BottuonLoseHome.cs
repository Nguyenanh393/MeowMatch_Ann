public class BottuonLoseHome : ButtonBaseHome
{
    protected override void DoWhenClicked()
    {
        base.DoWhenClicked();
        LevelManager.Instance.ReloadLevel();
    }
}

