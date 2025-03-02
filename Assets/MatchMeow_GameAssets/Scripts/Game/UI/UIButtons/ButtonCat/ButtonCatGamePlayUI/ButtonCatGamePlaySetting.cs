using Cysharp.Threading.Tasks;

namespace MatchMeow_GameAssets.Scripts.Game.UI.UIButtons.ButtonCat.ButtonCatGamePlayUI
{
    public class ButtonCatGamePlaySetting : ButtonBase
    {
        protected override async UniTask OnClickUniTask()
        {
            await base.OnClickUniTask();

            // LevelManager.Instance.ReloadLevel();
            DoWhenClicked();

        }

        private void DoWhenClicked()
        {
            UIManager.Instance.OpenUI<SettingUI>();
        }
    }
}
