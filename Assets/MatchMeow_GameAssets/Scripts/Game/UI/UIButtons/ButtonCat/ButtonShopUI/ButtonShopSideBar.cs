using _Pool.Pool;
using MatchMeow_GameAssets.Scripts.Game.UI.UIButtons.ButtonBase;
using UnityEngine;
using UnityEngine.UI;

namespace MatchMeow_GameAssets.Scripts.Game.UI.UIButtons.ButtonShopUI
{
    public class ButtonShopSideBar : ButtonBaseSideBar
    {

        public void OnClick()
        {
            SoundManager.Instance.PlayButtonSound();
            UIManager.Instance.GetUI<ShopUI>().OnClickButtonSideBar(this);
        }
    }
}
