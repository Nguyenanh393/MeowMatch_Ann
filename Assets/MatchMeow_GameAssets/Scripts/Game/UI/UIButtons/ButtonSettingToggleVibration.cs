public class ButtonSettingToggleVibration : ButtonBaseToggle
{
    protected override void DoWhenClicked()
    {
        base.DoWhenClicked(); // Gọi hàm từ class cha để xử lý chuyển đổi toggle

        // Tắt/bật rung thông qua VibrationManager
        VibrationManager.Instance.ToggleVibration(IsOn);

        // Phát âm thanh click (nếu âm thanh chưa tắt)
        if (SoundManager.Instance.IsSoundOn())
        {
            SoundManager.Instance.PlayButtonSound();
        }
    }
}
