using _Pool.Pool;
using UnityEngine;
using UnityEngine.UI;

namespace MatchMeow_GameAssets.Scripts.Game.UI.UIButtons.ButtonShopUI
{
    public class ButtonSideBar : PoolUnit
    {
        [SerializeField] private Image buttonImage;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color selectedColor;
        [SerializeField] private Text buttonNameText;

        private ShopItemType _itemType;

        public ShopItemType ItemType
        {
            get => _itemType;
            set => _itemType = value;
        }

        public Image ButtonImage => buttonImage;
        public Color NormalColor => normalColor;
        public Color SelectedColor => selectedColor;
        public Text ButtonNameText => buttonNameText;

        public void OnClick()
        {
            UIManager.Instance.GetUI<ShopUI>().OnClickButtonSideBar(this);
        }
    }
}
