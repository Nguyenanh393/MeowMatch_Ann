using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonBaseToggle : MonoBehaviour
{
    [SerializeField] private RectTransform knop;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image iconImage;
    [SerializeField] private Sprite iconImageOn;
    [SerializeField] private Sprite iconImageOff;
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;
    [SerializeField] private RectTransform leftTarget;
    [SerializeField] private RectTransform rightTarget;

    private bool _isOn = true;

    public bool IsOn
    {
        get => _isOn;
        set => _isOn = value;
    }

    protected virtual void Start()
    {
        UpdateVisuals();
    }

    public void OnClickVoid()
    {
        // await base.OnClickUniTask();

        DoWhenClicked();

    }

    protected virtual void DoWhenClicked()
    {
        _isOn = !_isOn;
        UpdateVisuals();

    }

    private void UpdateVisuals()
    {
        backgroundImage.DOColor(_isOn? onColor : offColor, 0.1f);

        iconImage.sprite = _isOn ? iconImageOn : iconImageOff;
        RectTransform target = _isOn ? rightTarget : leftTarget;
        knop.DOAnchorPos(target.anchoredPosition, 0.1f);
    }
}

