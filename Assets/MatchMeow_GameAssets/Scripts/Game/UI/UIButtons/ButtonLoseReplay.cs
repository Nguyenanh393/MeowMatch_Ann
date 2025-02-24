using Cysharp.Threading.Tasks;

public class ButtonLoseReplay : ButtonBase
{
protected override async UniTask OnClickUniTask()
{
    await base.OnClickUniTask();

    DoWhenClicked();

}

private void DoWhenClicked()
{
    UIManager.Instance.CloseUI<LoseUI>();
    GameManager.Instance.OnReloadGame().Forget();
    // UIManager.Instance.GamePlayObjectCanvas.gameObject.SetActive(true);
}
}
