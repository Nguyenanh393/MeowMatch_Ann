using Cysharp.Threading.Tasks;

public class ButtonBaseExit : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        DoWhenClicked();

    }

    protected virtual void DoWhenClicked()
    {
        if (!GameManager.Instance.IsState(GameState.GamePlay)) return;
        UIManager.Instance.GamePlayObject.SetActive(true);
        UIManager.Instance.GetUI<GamePlayUI>().CountDownText.ResumeCountdown();
    }
}

