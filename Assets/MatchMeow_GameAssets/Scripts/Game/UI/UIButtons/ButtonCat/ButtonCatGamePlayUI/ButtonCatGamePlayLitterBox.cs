using System;
using Cysharp.Threading.Tasks;


public class ButtonCatGamePlayLitterBox : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();


        DoWhenClicked();

    }

    private async void DoWhenClicked()
    {
        if (CooldownManager.Instance.IsLitterBoxAvailable())
        {
            // Open the cleaning UI
            UIManager.Instance.CloseUI<CatGamePlayUI>();
            UIManager.Instance.OpenUI<LoadingUI>();
            UIManager.Instance.CloseUI<LoadingUI>(2f);

            await UniTask.Delay(TimeSpan.FromSeconds(2));

            UIManager.Instance.OpenUI<CleanLitterBoxUI>();
        }
        else
        {
            // Show cooldown message for 3 seconds
            await CooldownManager.Instance.ShowCooldownMessage(CooldownManager.LITTER_BOX_KEY, 0.4f);
        }
    }
}
