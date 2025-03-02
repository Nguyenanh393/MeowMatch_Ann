using Cysharp.Threading.Tasks;

public class ButtonPause : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        // LevelManager.Instance.ReloadLevel();
        DoWhenClicked();

    }

    private void DoWhenClicked()
    {
        UIManager.Instance.OpenUI<PausePopUpUI>();
        UIManager.Instance.GamePlayObject.SetActive(false);
        UIManager.Instance.GetUI<GamePlayUI>().CountDownText.PauseCountdown();
    }
}
