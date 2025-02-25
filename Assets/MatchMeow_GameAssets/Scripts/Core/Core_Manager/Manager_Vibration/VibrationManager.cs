using UnityEngine;

public class VibrationManager : Singleton<VibrationManager>
{
    private bool _isVibrationOn = true;

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
    }

    /// <summary>
    /// Kiểm tra trạng thái rung hiện tại
    /// </summary>
    public bool IsVibrationOn()
    {
        return _isVibrationOn;
    }
}
