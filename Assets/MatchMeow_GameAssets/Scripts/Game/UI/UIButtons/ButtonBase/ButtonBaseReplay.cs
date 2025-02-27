using Cysharp.Threading.Tasks;

public class ButtonBaseReplay : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        DoWhenClicked();

    }

    protected virtual void DoWhenClicked()
    {
        GameManager.Instance.OnReloadGame().Forget();
        // UIManager.Instance.GamePlayObjectCanvas.gameObject.SetActive(true);
    }
}

