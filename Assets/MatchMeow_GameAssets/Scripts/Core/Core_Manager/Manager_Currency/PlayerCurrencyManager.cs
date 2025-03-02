using System;
using UnityEngine;

public class PlayerCurrencyManager : Singleton<PlayerCurrencyManager>
{

    private int _coins;
    private int _hearts;

    private void Awake()
    {
        LoadCurrency();
    }

    public bool HasEnoughCoins(int amount)
    {
        return _coins >= amount;
    }

    public void SpendCoins(int amount)
    {
        _coins -= amount;
        SaveCurrency();
    }

    public void AddCoins(int amount)
    {
        _coins += amount;
        SaveCurrency();
    }

    public int GetCoins()
    {
        return _coins;
    }

    // Hearts
    public bool HasEnoughHearts(int amount) => _hearts >= amount;

    public void SpendHearts(int amount)
    {
        _hearts -= amount;
        SaveCurrency();
    }

    public void AddHearts(int amount)
    {
        _hearts += amount;
        SaveCurrency();
    }

    public int GetHearts() => _hearts;

    private void LoadCurrency()
    {
        _coins = PlayerPrefs.GetInt(Constance.PlayerPref.COIN_VALUE, 0);
        _hearts = PlayerPrefs.GetInt(Constance.PlayerPref.HEART_VALUE, 0);
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(Constance.PlayerPref.COIN_VALUE, _coins);
        PlayerPrefs.SetInt(Constance.PlayerPref.HEART_VALUE, _hearts);
        PlayerPrefs.Save();
    }

    // public void UpdateRegardValueCoin(int regardValue)
    // {
    //     int currentCoinsValue = PlayerPrefs.GetInt(Constance.PlayerPref.COIN_VALUE, 0);
    //     int updatedLevelCoin = currentCoinsValue + regardValue;
    //     PlayerPrefs.SetInt(Constance.PlayerPref.COIN_VALUE, updatedLevelCoin);
    //     Debug.Log("Update Player Coin Complete" + updatedLevelCoin);
    // }
}
