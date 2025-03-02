using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : PopUpUI
{
    [SerializeField] private GameObject regardButtons;
    [SerializeField] private GameObject functionButtons;
    [SerializeField] private Text coinText;
    private bool _isRegardOn = true;

    [SerializeField] private float coinAnimationDuration = 1.0f;
    [SerializeField] private float coinAnimationDelay = 0.5f;

    public bool IsRegardOn
    {
        get => _isRegardOn;
        set
        {
            _isRegardOn = value;
            // Save state when changed
            SaveRegardState();
        }
    }

    public GameObject RegardButtons => regardButtons;
    public GameObject FunctionButtons => functionButtons;

    protected override void OnEnable()
    {
        base.OnEnable();
        UIManager.Instance.GetUI<GamePlayUI>().CoinPanel.SetActive(false);

        // Load the saved state for current level
        LoadRegardState();

        regardButtons.SetActive(_isRegardOn);
        functionButtons.SetActive(!_isRegardOn);
        coinText.text = PlayerCurrencyManager.Instance.GetCoins().ToString();
    }

    private void SaveRegardState()
    {
        int currentLevel = LevelManager.Instance.CurrentLevelId;
        PlayerPrefs.SetInt(Constance.PlayerPref.REWARD_COLLECTED_KEY + currentLevel, _isRegardOn ? 0 : 1);
        PlayerPrefs.Save();
    }

    private void LoadRegardState()
    {
        int currentLevel = LevelManager.Instance.CurrentLevelId;
        // If the value is 1, it means reward was collected (so isRegardOn should be false)
        int rewardCollected = PlayerPrefs.GetInt(Constance.PlayerPref.REWARD_COLLECTED_KEY + currentLevel, 0);
        _isRegardOn = (rewardCollected == 0);
    }

    public async UniTask AnimateCoinIncrease(int startValue, int endValue)
    {
        // Existing implementation...
        float elapsedTime = 0;
        int currentCoinValue;

        await UniTask.Delay(TimeSpan.FromSeconds(coinAnimationDelay));
        while (elapsedTime < coinAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / coinAnimationDuration);
            float smoothT = Mathf.SmoothStep(0, 1, t);
            currentCoinValue = startValue + Mathf.FloorToInt((endValue - startValue) * smoothT);
            coinText.text = currentCoinValue.ToString();
            await UniTask.Yield();
        }
        coinText.text = endValue.ToString();
    }
}
