using System;
using Cysharp.Threading.Tasks;


public class ButtonCatGamePlayShop : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        UIManager.Instance.CloseUI<CatGamePlayUI>();
        UIManager.Instance.OpenUI<LoadingUI>();
        UIManager.Instance.CloseUI<LoadingUI>(2.1f);

        await UniTask.Delay(TimeSpan.FromSeconds(2));
        // LevelManager.Instance.ReloadLevel();
        DoWhenClicked();

    }

    private void DoWhenClicked()
    {
        UIManager.Instance.OpenUI<ShopUI>();

    }
}

