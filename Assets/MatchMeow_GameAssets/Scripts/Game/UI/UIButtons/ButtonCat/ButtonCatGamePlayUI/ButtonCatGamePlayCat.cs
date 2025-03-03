using Cysharp.Threading.Tasks;

public class ButtonCatGamePlayCat : ButtonBase
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

