using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class PopUpUI : UICanvas
{
    [SerializeField] private float animationDuration = 0.3f; // Thời gian chuyển đổi
    private Vector3 _hiddenScale = Vector3.zero;  // Scale khi ẩn
    private Vector3 _visibleScale = Vector3.one;  // Scale khi hiện

    protected virtual void OnEnable()
    {
        // Đặt scale ban đầu là 0
        transform.localScale = _hiddenScale;

        // Hiệu ứng scale từ 0 → 1
        transform.DOScale(_visibleScale, animationDuration)
            .SetEase(Ease.OutBack); // Hiệu ứng mượt
    }

    public async UniTask ClosePopup()
    {
        // Hiệu ứng scale từ 1 → 0 và tắt UI sau khi thu nhỏ
        transform.DOScale(_hiddenScale, animationDuration)
            .SetEase(Ease.InBack);
        await UniTask.Delay(TimeSpan.FromSeconds(animationDuration));
    }
    // private void OnDisable()
    // {
    //     ClosePopup().Forget();
    // }
}
