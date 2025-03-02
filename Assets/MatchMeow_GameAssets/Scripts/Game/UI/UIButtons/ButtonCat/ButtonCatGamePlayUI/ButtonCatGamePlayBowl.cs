using Cysharp.Threading.Tasks;
using MatchMeow_GameAssets.Scripts.Game.UI.UICanvas.CatUI;
using UnityEngine;

namespace MatchMeow_GameAssets.Scripts.Game.UI.UIButtons.ButtonCat.ButtonCatGamePlayUI
{
    public class ButtonCatGamePlayBowl : ButtonBaseItemtype
    {


        protected override async UniTask OnClickUniTask()
        {
            await base.OnClickUniTask();

            // LevelManager.Instance.ReloadLevel();
            DoWhenClicked();

        }

        private void DoWhenClicked()
        {
            UIManager.Instance.OpenUI<CatPopUpUI>();

            UIManager.Instance.GetUI<CatPopUpUI>().ItemType = ItemType;

            UIManager.Instance.GetUI<CatPopUpUI>().LoadOwnedItems();

            UIManager.Instance.GetUI<CatPopUpUI>().SetBaseButton(this);
        }
    }
}
