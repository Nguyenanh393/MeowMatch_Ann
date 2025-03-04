using System;
using Cysharp.Threading.Tasks;

public class ButtonFeedBowl : ButtonBaseItemtype
{
    // protected override async UniTask OnClickUniTask()
    // {
    //     await base.OnClickUniTask();
    //
    //
    //     DoWhenClicked();
    //
    // }

    protected override async void DoWhenClicked()
    {
        if (CooldownManager.Instance.IsFeedingAvailable())
        {
            UIManager.Instance.CloseUI<MultiFunctionBowlUI>();
            base.DoWhenClicked();

        }
        else
        {
            // Show cooldown message for 3 seconds
            await CooldownManager.Instance.ShowCooldownMessage(CooldownManager.FEEDING_BOWL_KEY, 0.4f);
        }
    }
}

