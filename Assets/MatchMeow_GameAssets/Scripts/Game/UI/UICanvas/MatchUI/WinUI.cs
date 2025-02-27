using System;
using UnityEngine;

public class WinUI : PopUpUI
{
    [SerializeField] private GameObject regardButtons;
    [SerializeField] private GameObject functionButtons;

    private bool _isRegardOn = true;

    public bool IsRegardOn
    {
        get => _isRegardOn;
        set => _isRegardOn = value;
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        UIManager.Instance.GetUI<GamePlayUI>().CoinPanel.SetActive(false);
        regardButtons.SetActive(_isRegardOn);
        functionButtons.SetActive(!_isRegardOn);
    }

    // private void OnDisable()
    // {
    //     regardButtons.SetActive(false);
    //     functionButtons.SetActive(false);
    // }


}

