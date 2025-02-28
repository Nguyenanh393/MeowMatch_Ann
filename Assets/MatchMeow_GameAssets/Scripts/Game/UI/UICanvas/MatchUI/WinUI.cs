using System;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : PopUpUI
{
    [SerializeField] private GameObject regardButtons;
    [SerializeField] private GameObject functionButtons;
    [SerializeField] private Text coinText;
    private bool _isRegardOn = true;

    public bool IsRegardOn
    {
        get => _isRegardOn;
        set => _isRegardOn = value;
    }
    public GameObject RegardButtons => regardButtons;
    public GameObject FunctionButtons => functionButtons;
    protected override void OnEnable()
    {
        base.OnEnable();
        UIManager.Instance.GetUI<GamePlayUI>().CoinPanel.SetActive(false);
        regardButtons.SetActive(_isRegardOn);
        functionButtons.SetActive(!_isRegardOn);
        coinText.text = PlayerPrefs.GetInt(Constance.PlayerPref.COIN_VALUE, 0).ToString();
    }
}

