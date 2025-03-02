using System;
using Cysharp.Threading.Tasks;

namespace MatchMeow_GameAssets.Scripts.Game.UI.UIButtons.ButtonShopUI
{
    public class ButtonShopExit : ButtonBaseExit
    {
        protected override async UniTask OnClickUniTask()
        {
            UIManager.Instance.CloseUI<ShopUI>();
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
}
