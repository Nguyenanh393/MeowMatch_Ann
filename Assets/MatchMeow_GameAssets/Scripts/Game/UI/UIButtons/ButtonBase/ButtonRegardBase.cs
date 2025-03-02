using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRegardBase : ButtonBase
{
    public enum RegardType
    {
        coin,
        heart
    }

    [SerializeField] private RegardType type;
    [SerializeField] private Text buttonText;
    [SerializeField] private bool isAdButton;

    private int _regardValue;
    private void OnEnable()
    {
        SetButtonText(type);
    }

    private void SetButtonText(RegardType type)
    {
        if (type == RegardType.coin)
        {
            int levelCoin = LevelManager.Instance.CurrentLevel.Coin;
            int extraBonus = isAdButton ? Constance.RegardBonusAd.COIN_BONUS_AD : Constance.RegardBonusAd.COIN_NOT_BONUS;
            int buttonValueCoin = levelCoin * extraBonus;
            _regardValue = buttonValueCoin;
            buttonText.text = buttonValueCoin.ToString();
        }
        else
        {
            int levelHeart = Constance.RegardHeart.HEART_VALUE;
            int extraBonus = isAdButton ? Constance.RegardBonusAd.HEART_BONUS_AD : Constance.RegardBonusAd.HEART_NOT_BONUS;
            int buttonValueHeart = levelHeart * extraBonus;
            _regardValue = buttonValueHeart;
            buttonText.text = buttonValueHeart.ToString();
        }
    }

    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        DoWhenClicked();

    }

    protected virtual async void DoWhenClicked()
    {
        if (type == RegardType.coin)
        {
            // Get current coin count before adding
            int startValue = PlayerCurrencyManager.Instance.GetCoins();

            // Add coins to player's currency
            PlayerCurrencyManager.Instance.AddCoins(_regardValue);

            // Get new coin count
            int endValue = PlayerCurrencyManager.Instance.GetCoins();

            // Get WinUI and play animation
            WinUI winUI = UIManager.Instance.GetUI<WinUI>();
            winUI.RegardButtons.SetActive(false);
            winUI.IsRegardOn = false;
            await winUI.AnimateCoinIncrease(startValue, endValue);

            // Hide regard buttons and show function buttons
            winUI.FunctionButtons.SetActive(true);
        }
        else
        {
            UIManager.Instance.GetUI<CatRegardUI>().RegardButtons.SetActive(false);
            int startValue = PlayerCurrencyManager.Instance.GetHearts();

            PlayerCurrencyManager.Instance.AddHearts(_regardValue);
            int endValue = PlayerCurrencyManager.Instance.GetHearts();

            CatRegardUI catRegardUI = UIManager.Instance.GetUI<CatRegardUI>();
            await catRegardUI.AnimateHeartIncrease(startValue, endValue);

            UIManager.Instance.CloseAll();

            UIManager.Instance.OpenUI<LoadingUI>();
            UIManager.Instance.CloseUI<LoadingUI>(2f);

            await UniTask.Delay(TimeSpan.FromSeconds(2));
            UIManager.Instance.GetUI<CatGamePlayUI>().Open();
        }
    }

}

