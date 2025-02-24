public class ButtonSettingExit : ButtonBaseExit
{
    protected override void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<SettingUI>();
        base.DoWhenClicked();
    }
}

