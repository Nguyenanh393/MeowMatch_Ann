
using System;
using Cysharp.Threading.Tasks;

public class BottonDressupExit : ButtonBaseExit
{
    protected override async UniTask OnClickUniTask()
    {
        UIManager.Instance.CloseUI<CatDressupUI>();
        UIManager.Instance.OpenUI<LoadingUI>();
        UIManager.Instance.CloseUI<LoadingUI>(2f);

        await UniTask.Delay(TimeSpan.FromSeconds(2));

        await base.OnClickUniTask();

    }
    protected override void DoWhenClicked()
    {
        UIManager.Instance.OpenUI<CatGamePlayUI>();
        base.DoWhenClicked();
    }
}
