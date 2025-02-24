using Cysharp.Threading.Tasks;

public class ButtonPauseExit : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        DoWhenClicked();

    }

    private void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<PausePopUpUI>();
        UIManager.Instance.GamePlayObject.SetActive(true);
        UIManager.Instance.GetUI<GamePlayUI>().CountDownText.ResumeCountdown();
    }
}
