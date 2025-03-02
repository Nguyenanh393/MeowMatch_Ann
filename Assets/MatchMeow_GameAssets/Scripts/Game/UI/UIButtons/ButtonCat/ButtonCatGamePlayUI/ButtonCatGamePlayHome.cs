using Cysharp.Threading.Tasks;

namespace MatchMeow_GameAssets.Scripts.Game.UI.UIButtons.ButtonCat.ButtonCatGamePlayUI
{
    public class ButtonCatGamePlayHome : ButtonBase
    {
        protected override async UniTask OnClickUniTask()
        {
            await base.OnClickUniTask();

            DoWhenClicked();

        }

        private void DoWhenClicked()
        {
            UIManager.Instance.CloseUI<CatGamePlayUI>();
            UIManager.Instance.OpenUI<MainMenuUI>();
        }
    }
}
