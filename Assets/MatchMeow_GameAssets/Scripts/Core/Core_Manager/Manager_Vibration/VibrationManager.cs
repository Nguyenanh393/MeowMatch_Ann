using System;
using UnityEngine;

public class VibrationManager : Singleton<VibrationManager>
{
    private bool _isVibrationOn = true;

    private void Start()
    {
        _isVibrationOn = PlayerPrefs.GetInt(Constance.PlayerPref.IS_VIBRATION_ON, 1) == 1 ? true : false;
    }

    /// <summary>
    /// Rung mặc định
    /// </summary>
    public void Vibrate()
    {
        if (_isVibrationOn)
        {
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
        }
    }

    /// <summary>
    /// Bật/tắt rung
    /// </summary>
    public void ToggleVibration(bool isOn)
    {
        _isVibrationOn = isOn;
        PlayerPrefs.SetInt(Constance.PlayerPref.IS_VIBRATION_ON, _isVibrationOn ? 1 : 0);
    }

    /// <summary>
    /// Kiểm tra trạng thái rung hiện tại
    /// </summary>
    public bool IsVibrationOn()
    {
        return _isVibrationOn;
    }
}
