using System;
using Cysharp.Threading.Tasks;

namespace MatchMeow_GameAssets.Scripts.Game.UI.UIButtons.ButtonCat.ButtonCatGamePlayUI
{
    public class ButtonCatGamePlayLitterBox : ButtonBase
    {
        protected override async UniTask OnClickUniTask()
        {
            await base.OnClickUniTask();

            UIManager.Instance.CloseUI<CatGamePlayUI>();
            UIManager.Instance.OpenUI<LoadingUI>();
            UIManager.Instance.CloseUI<LoadingUI>(2f);

            await UniTask.Delay(TimeSpan.FromSeconds(2));

            DoWhenClicked();

        }

        private void DoWhenClicked()
        {
            UIManager.Instance.OpenUI<CleanLitterBoxUI>();
        }
    }
}
