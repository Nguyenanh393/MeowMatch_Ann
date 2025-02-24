
using Cysharp.Threading.Tasks;

public class ButtonWinHome : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        DoWhenClicked();

    }

    private void DoWhenClicked()
    {
        UIManager.Instance.CloseAll();
        UIManager.Instance.GamePlayObject.SetActive(false);
        UIManager.Instance.OpenUI<MainMenuUI>();
        LevelManager.Instance.LoadNextLevel();
        UIManager.Instance.GetUI<GamePlayUI>().CountDownText.ResetCountdown();
    }
}
