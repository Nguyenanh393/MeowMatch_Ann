using Cysharp.Threading.Tasks;


public class ButtonCatGamePlayClean : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        DoWhenClicked();

    }

    private void DoWhenClicked()
    {

    }
}
