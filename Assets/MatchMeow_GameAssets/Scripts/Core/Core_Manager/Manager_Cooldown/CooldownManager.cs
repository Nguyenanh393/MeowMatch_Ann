using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MyUtils;
using UnityEngine;
using UnityEngine.UI;

public class CooldownManager : Singleton<CooldownManager>
{
    [SerializeField] private GameObject cooldownPanel;
    [SerializeField] private Text cooldownText;

    // Cooldown times in seconds
    private const int LITTER_BOX_COOLDOWN = 10800; // 3 hours (3 * 60 * 60)
    private const int FEEDING_COOLDOWN = 7200;    // 2 hours

    // PlayerPrefs keys
    public const string LITTER_BOX_KEY = "NextLitterBoxTime";
    public const string FEEDING_BOWL_KEY = "NextFeedingTime";

    public void SetNextAvailableTime(string key, int cooldownSeconds)
    {
        long nextTime = TimeUtility.GetCurrentTimestamp() + cooldownSeconds;
        PlayerPrefs.SetString(key, nextTime.ToString());
        PlayerPrefs.Save();
    }

    public void SetLitterBoxCooldown()
    {
        SetNextAvailableTime(LITTER_BOX_KEY, LITTER_BOX_COOLDOWN);
    }

    public void SetFeedingCooldown()
    {
        SetNextAvailableTime(FEEDING_BOWL_KEY, FEEDING_COOLDOWN);
    }

    public bool IsActionAvailable(string key)
    {
        string savedTimeStr = PlayerPrefs.GetString(key, "0");
        long nextAvailableTime = long.Parse(savedTimeStr);
        long currentTime = TimeUtility.GetCurrentTimestamp();

        return currentTime >= nextAvailableTime;
    }

    public bool IsLitterBoxAvailable()
    {
        return IsActionAvailable(LITTER_BOX_KEY);
    }

    public bool IsFeedingAvailable()
    {
        return IsActionAvailable(FEEDING_BOWL_KEY);
    }

    public int GetRemainingSeconds(string key)
    {
        string savedTimeStr = PlayerPrefs.GetString(key, "0");
        long nextAvailableTime = long.Parse(savedTimeStr);
        long currentTime = TimeUtility.GetCurrentTimestamp();

        int remainingTime = (int)(nextAvailableTime - currentTime);
        return Math.Max(0, remainingTime);
    }

    public string GetFormattedTimeRemaining(string key)
    {
        int seconds = GetRemainingSeconds(key);

        int hours = seconds / 3600;
        int minutes = (seconds % 3600) / 60;
        int remainingSeconds = seconds % 60;

        return $"{hours:D2}:{minutes:D2}:{remainingSeconds:D2}";
    }

    public async UniTask ShowCooldownMessage(string key, float duration = 0f)
    {
        if (cooldownPanel == null || cooldownText == null) return;

        // Get or add a CanvasGroup component to control opacity
        CanvasGroup canvasGroup = cooldownPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = cooldownPanel.AddComponent<CanvasGroup>();

        // Set initial state
        canvasGroup.alpha = 0f;
        cooldownPanel.SetActive(true);

        string timeRemaining = GetFormattedTimeRemaining(key);
        cooldownText.text = $"Available in: {timeRemaining}";

        // Fade in animation
        await canvasGroup.DOFade(1f, 0.3f).SetEase(Ease.OutQuad).ToUniTask();

        // Wait for the desired duration
        await UniTask.Delay(TimeSpan.FromSeconds(duration));

        // Fade out animation
        await canvasGroup.DOFade(0f, 0.3f).SetEase(Ease.InQuad).ToUniTask();

        // Hide the panel after fade out
        cooldownPanel.SetActive(false);
    }

    private void HideCooldownPanel()
    {
        if (cooldownPanel != null)
            cooldownPanel.SetActive(false);
    }
}
