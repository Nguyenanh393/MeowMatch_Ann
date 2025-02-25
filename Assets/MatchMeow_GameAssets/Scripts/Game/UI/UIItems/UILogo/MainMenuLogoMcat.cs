using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Sequence = DG.Tweening.Sequence;

public class MainMenuLogoMcat : GameUnit
{
    [SerializeField] private RectTransform thisRect;
    [SerializeField] private RectTransform originalPos;
    private bool _isStarted;
    private Sequence _jumpSequence;

    private void Start()
    {
        // TF.position = originalPos.anchoredPosition;
        OnInit().Forget();
        _isStarted = true;
    }

    private void OnEnable()
    {
        if(!_isStarted) return;
        OnInit().Forget();
    }

    private async UniTaskVoid OnInit()
    {
        float jumpPower = 200f; // Độ cao nhảy
        int numJumps = 1;       // Số lần nhảy
        float duration = 0.4f;  // Tổng thời gian nhảy

        _jumpSequence = DOTween.Sequence();

        // Hiệu ứng nhảy
        _jumpSequence.Append(thisRect.DOJumpAnchorPos(originalPos.anchoredPosition, jumpPower, numJumps, duration)
            .SetEase(Ease.OutQuad));

        // Hiệu ứng scale khi bắt đầu nhảy
        _jumpSequence.Join(thisRect.DOScale(new Vector3(1.2f, 0.8f, 1f), duration / 2));

        // Hiệu ứng scale khi lên đỉnh nhảy
        _jumpSequence.Append(thisRect.DOScale(Vector3.one, duration / 4));

        // Hiệu ứng scale khi rơi xuống
        _jumpSequence.Append(thisRect.DOScale(new Vector3(0.8f, 1.2f, 1f), duration / 4));

        // Hiệu ứng scale khi chạm đất
        _jumpSequence.Append(thisRect.DOScale(Vector3.one, duration / 4));

        await _jumpSequence.ToUniTask();

        TF.localScale = Vector3.one;
        TF.rotation = Quaternion.Euler(0f, 0f, 0f);
        thisRect.DORotate(new Vector3(0, 0, 4), 0.6f) // Xoay lên 10 độ trong 0.5 giây
            .SetLoops(-1, LoopType.Yoyo) // Lặp lại lên xuống
            .SetEase(Ease.InOutSine); // Làm mượt hiệu ứng

        thisRect.DOScale(1.2f, 0.6f) // Scale lên 1.1 trong 0.5 giây
            .SetLoops(-1, LoopType.Yoyo) // Lặp vô hạn kiểu Yoyo (tăng rồi giảm)
            .SetEase(Ease.InOutSine); // Làm mượt hiệu ứng

    }

    private void OnDisable()
    {
        TF.localScale = Vector3.one;
        TF.rotation = Quaternion.Euler(0f, 0f, 0f);
        thisRect.DOKill();
    }
}

