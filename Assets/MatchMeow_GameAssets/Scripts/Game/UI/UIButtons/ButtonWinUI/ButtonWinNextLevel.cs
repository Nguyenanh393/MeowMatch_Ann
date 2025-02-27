using Cysharp.Threading.Tasks;

public class ButtonWinNextLevel : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        DoWhenClicked();

    }

    private void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<WinUI>();
        GameManager.Instance.OnLoadNextLevel().Forget();
        // UIManager.Instance.GamePlayObjectCanvas.gameObject.SetActive(true);
        UIManager.Instance.GetUI<WinUI>().IsRegardOn = true;
    }
}

