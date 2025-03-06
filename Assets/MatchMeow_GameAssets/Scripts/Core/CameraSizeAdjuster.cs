using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSizeAdjuster : MonoBehaviour
{
    [SerializeField] private float defaultWidth = 1080f;
    [SerializeField] private float defaultHeight = 1920f;// Chiều rộng mặc định
    private Camera mainCamera;
    private bool isInitialized = false;

    public float plusSize = 100;
    private void Start()
    {
        mainCamera = Camera.main;
        AdjustCameraSize();
    }

    private void AdjustCameraSize()
    {
        if (mainCamera == null || !mainCamera.orthographic || isInitialized)
            return;

        // Tính toán tỷ lệ màn hình
        float screenAspect = (float)Screen.width / Screen.height;
        float targetAspect = defaultWidth / defaultHeight;

        // Điều chỉnh size dựa trên tỷ lệ
        float newOrthographicSize = 5 / (screenAspect / targetAspect) + plusSize;
        mainCamera.orthographicSize = newOrthographicSize;

        isInitialized = true; // Đánh dấu là đã khởi tạo xong
        Debug.Log($"Camera size adjusted to: {newOrthographicSize} based on screen width {Screen.width} and height {Screen.height}");


        // float ratio = 1f * Screen.width / Screen.height;
        //
        // float realWidth = cameraLockWidth * 0.5f / ratio;
        // mainCamera.orthographicSize = realWidth;

    }

    private void OnValidate()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        AdjustCameraSize();
    }
}
