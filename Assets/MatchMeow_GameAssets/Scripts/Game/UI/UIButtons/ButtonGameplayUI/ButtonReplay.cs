// using _UI.Scripts.UI;
using Cysharp.Threading.Tasks;

public class ButtonReplay : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        // LevelManager.Instance.ReloadLevel();
        DoWhenClicked();

    }

    private void DoWhenClicked()
    {
        UIManager.Instance.OpenUI<ReplayPopUpUI>();
        UIManager.Instance.GamePlayObject.SetActive(false);
        UIManager.Instance.GetUI<GamePlayUI>().CountDownText.PauseCountdown();
    }
}

