using Cysharp.Threading.Tasks;

public class ButtonMainMenuSetting : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        // LevelManager.Instance.ReloadLevel();
        DoWhenClicked();

    }

    private void DoWhenClicked()
    {
        UIManager.Instance.OpenUI<SettingUI>();
    }
}
