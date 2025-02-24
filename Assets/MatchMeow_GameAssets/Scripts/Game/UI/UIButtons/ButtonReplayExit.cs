using Cysharp.Threading.Tasks;

public class ButtonReplayExit : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        DoWhenClicked();

    }

    private void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<ReplayPopUpUI>();
        UIManager.Instance.GamePlayObject.SetActive(true);
        UIManager.Instance.GetUI<GamePlayUI>().CountDownText.ResumeCountdown();
    }
}
