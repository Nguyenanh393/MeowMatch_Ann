using System;
using Cysharp.Threading.Tasks;

public class ButtonCleanLitter : ButtonBase
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
            UIManager.Instance.CloseUI<CatGamePlayUI>();
            // Open the cleaning UI
            UIManager.Instance.OpenUI<LoadingUI>();
            UIManager.Instance.CloseUI<LoadingUI>(2.1f);

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
