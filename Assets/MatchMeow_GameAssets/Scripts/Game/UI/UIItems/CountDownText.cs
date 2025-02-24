using MyUtils;
using UnityEngine;
using UnityEngine.UI;

public class CountDownText : MonoBehaviour
{
    [SerializeField] private Text countdownText;
    [SerializeField] private float countdownTime = 10f; // Thời gian đếm ngược

    private TimeCounterSecond timer = new TimeCounterSecond();
    private ValueObserver<int> remainingTime = new ValueObserver<int>();

    private void Start()
    {
        // Khởi tạo bộ đếm thời gian
        timer.Init(countdownTime, OnCountdownComplete, false);

        // Đăng ký sự kiện khi thời gian thay đổi
        remainingTime.OnChanged += UpdateCountdownUI;

        // Gán giá trị ban đầu
        remainingTime.Value = Mathf.CeilToInt(countdownTime);

        // Bắt đầu đếm
        StartCountdown();
    }

    private void Update()
    {
        timer.Execute();

        // Cập nhật thời gian còn lại
        int newTime = Mathf.CeilToInt(timer.CurrentSeconds);
        if (newTime != remainingTime.Value)
        {
            remainingTime.Value = newTime;
        }
    }

    private void StartCountdown()
    {
        timer.Resume();
    }

    private void UpdateCountdownUI(int timeLeft)
    {
        int minutes = timeLeft / 60; // Lấy số phút
        int seconds = timeLeft % 60; // Lấy số giây

        // Đảm bảo rằng số phút và giây luôn có 2 chữ số (ví dụ: 02:00 thay vì 2:0)
        countdownText.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }


    private void OnCountdownComplete()
    {
        // Debug.Log("Countdown finished!");
        // countdownText.text = "0";
        GameManager.Instance.OnLoseState().Forget();
        ;

    }

    // Tạm dừng bộ đếm
    public void PauseCountdown()
    {
        timer.Pause();
    }

    // Tiếp tục bộ đếm
    public void ResumeCountdown()
    {
        timer.Resume();
    }

    // Đếm lại từ đầu
    public void ResetCountdown()
    {
        timer.Reset();
        remainingTime.Value = Mathf.CeilToInt(countdownTime);
    }
}
