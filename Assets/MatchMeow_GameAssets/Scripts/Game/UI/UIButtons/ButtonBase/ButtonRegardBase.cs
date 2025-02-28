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

            buttonText.text = buttonValueCoin.ToString();
        }
    }

    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();

        DoWhenClicked();

    }

    protected virtual void DoWhenClicked()
    {
        // Luu PlayerPrefs
        // hien Function + danh dau _isRegardOn = false
        WinUI winUI = UIManager.Instance.GetUI<WinUI>();
        winUI.RegardButtons.SetActive(false);
        winUI.IsRegardOn = false;
        winUI.FunctionButtons.SetActive(true);
    }

}

