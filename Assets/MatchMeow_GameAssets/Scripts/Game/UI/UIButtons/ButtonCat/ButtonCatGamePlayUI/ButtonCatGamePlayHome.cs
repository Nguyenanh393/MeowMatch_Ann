using Cysharp.Threading.Tasks;

public class ButtonCatGamePlayHome : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        DoWhenClicked();

    }

    private void DoWhenClicked()
    {
        UIManager.Instance.CloseUI<CatGamePlayUI>();
        UIManager.Instance.OpenUI<MainMenuUI>();
    }
}

