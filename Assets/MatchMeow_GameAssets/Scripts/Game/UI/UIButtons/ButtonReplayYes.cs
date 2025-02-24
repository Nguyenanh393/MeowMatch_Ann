using Cysharp.Threading.Tasks;

public class ButtonReplayYes : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        DoWhenClicked();

    }

    private void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<ReplayPopUpUI>();
        GameManager.Instance.OnReloadGame();
        // UIManager.Instance.GamePlayObjectCanvas.gameObject.SetActive(true);
    }
}

