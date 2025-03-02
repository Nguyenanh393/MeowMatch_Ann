using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CatRegardUI : PopUpUI
{
    [SerializeField] private Text coinText;
    [SerializeField] private Text heartText;
    [SerializeField] private float heartAnimationDuration = 1.0f;
    [SerializeField] private float heartAnimationDelay = 0.5f;
    [SerializeField] private GameObject regardButtons;

    public GameObject RegardButtons => regardButtons;
    protected override void OnEnable()
    {
        base.OnEnable();
        coinText.text = PlayerCurrencyManager.Instance.GetCoins().ToString();
        heartText.text = PlayerCurrencyManager.Instance.GetHearts().ToString();
        regardButtons.SetActive(true);
    }
    public async Task AnimateHeartIncrease(int startValue, int endValue)
    {
        float elapsedTime = 0;
        int currentCoinValue;

        await UniTask.Delay(TimeSpan.FromSeconds(heartAnimationDelay));
        while (elapsedTime < heartAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / heartAnimationDuration);
            float smoothT = Mathf.SmoothStep(0, 1, t);
            currentCoinValue = startValue + Mathf.FloorToInt((endValue - startValue) * smoothT);
            heartText.text = currentCoinValue.ToString();
            await UniTask.Yield();
        }
        heartText.text = endValue.ToString();

        await UniTask.Delay(TimeSpan.FromSeconds(heartAnimationDelay/5));
    }
}

