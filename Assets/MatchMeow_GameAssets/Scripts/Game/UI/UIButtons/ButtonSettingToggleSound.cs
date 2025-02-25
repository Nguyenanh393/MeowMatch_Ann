 public class ButtonSettingToggleSound : ButtonBaseToggle
{
    protected override void DoWhenClicked()
    {
        base.DoWhenClicked(); // Gọi hàm từ class cha để xử lý chuyển đổi toggle

        // Tắt/bật âm thanh thông qua SoundManager
        SoundManager.Instance.ToggleSound(IsOn);

        // Phát âm thanh click (nếu âm thanh chưa tắt)
        if (SoundManager.Instance.IsSoundOn())
        {
            SoundManager.Instance.PlayButtonSound();
        }
    }
}

