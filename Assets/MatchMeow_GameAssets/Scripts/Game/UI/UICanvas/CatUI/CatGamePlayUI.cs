using System;
using UnityEngine;
using UnityEngine.UI;

public class CatGamePlayUI : UICanvas
{
    [SerializeField] private Text coinText;
    [SerializeField] private Text heartText;


    private void OnEnable()
    {
        SetCoinText();
        SetHeartText();
    }

    private void SetCoinText()
    {
        int currentCoin = PlayerCurrencyManager.Instance.GetCoins();
        coinText.text = currentCoin.ToString();
    }

    private void SetHeartText()
    {
        int currentHeart = PlayerCurrencyManager.Instance.GetHearts();
        heartText.text = currentHeart.ToString();
    }
}

