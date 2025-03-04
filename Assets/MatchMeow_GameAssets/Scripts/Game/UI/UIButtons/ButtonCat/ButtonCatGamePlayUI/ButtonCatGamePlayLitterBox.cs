using System;
using Cysharp.Threading.Tasks;


public class ButtonCatGamePlayLitterBox : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        // LevelManager.Instance.ReloadLevel();
        DoWhenClicked();

    }

    private void DoWhenClicked()
    {
        UIManager.Instance.OpenUI<MultiFuctionLitterUI>();

    }
}
