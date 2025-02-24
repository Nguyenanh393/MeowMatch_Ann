using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using Sequence = DG.Tweening.Sequence;

public class MainMenuLogoMcat : GameUnit
{
    private Vector3 _originalPos;
    private bool _isStarted;


    private void Start()
    {
        _originalPos = TF.position;
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
        float jumpPower = 0.5f; // Độ cao nhảy
        int numJumps = 1;       // Số lần nhảy
        float duration = 0.4f;  // Tổng thời gian nhảy

        Sequence jumpSequence = DOTween.Sequence();

        // Hiệu ứng nhảy
        jumpSequence.Append(TF.DOJump(_originalPos, jumpPower, numJumps, duration)
            .SetEase(Ease.OutQuad));

        // Hiệu ứng scale khi bắt đầu nhảy
        jumpSequence.Join(TF.DOScale(new Vector3(1.2f, 0.8f, 1f), duration / 2));

        // Hiệu ứng scale khi lên đỉnh nhảy
        jumpSequence.Append(TF.DOScale(Vector3.one, duration / 4));

        // Hiệu ứng scale khi rơi xuống
        jumpSequence.Append(TF.DOScale(new Vector3(0.8f, 1.2f, 1f), duration / 4));

        // Hiệu ứng scale khi chạm đất
        jumpSequence.Append(TF.DOScale(Vector3.one, duration / 4));

        await jumpSequence.ToUniTask();

        TF.localScale = new Vector3(1f, 1f, 1f);
        TF.DORotate(new Vector3(0, 0, 4), 0.6f) // Xoay lên 10 độ trong 0.5 giây
            .SetLoops(-1, LoopType.Yoyo) // Lặp lại lên xuống
            .SetEase(Ease.InOutSine); // Làm mượt hiệu ứng

        TF.DOScale(1.1f, 0.6f) // Scale lên 1.1 trong 0.5 giây
            .SetLoops(-1, LoopType.Yoyo) // Lặp vô hạn kiểu Yoyo (tăng rồi giảm)
            .SetEase(Ease.InOutSine); // Làm mượt hiệu ứng

    }
}

