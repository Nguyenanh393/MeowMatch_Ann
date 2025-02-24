using System;
using DG.Tweening;
using UnityEngine;

public class LoadingLogoCat : GameUnit
{
    private Vector3 _originalScale;
    private Vector3 _originalPosition;
    private Sequence _sequence;

    private void Awake()
    {
        _originalScale = TF.localScale;
        _originalPosition = TF.position;
    }

    private void OnEnable()
    {
        StartAnimation();
    }

    private void StartAnimation()
    {
        _sequence?.Kill(); // Hủy sequence nếu có
        _sequence = DOTween.Sequence();

        TF.localScale = _originalScale;
        TF.position = _originalPosition;

        // Di chuyển lên (y * 1.2) và scale (0.8, 1.2, 1)
        _sequence.Append(TF.DOMoveY(_originalPosition.y + 1f, 0.2f))
            .Join(TF.DOScale(new Vector3(0.9f, 1.1f, 1), 0.2f))

            // Di chuyển xuống (y * 0.8) và scale (1.2, 0.8, 1)
            .Append(TF.DOMoveY(_originalPosition.y, 0.2f))
            .Join(TF.DOScale(new Vector3(1.1f, 0.9f, 1), 0.2f));

        _sequence.SetLoops(-1, LoopType.Yoyo); // Lặp vô hạn với Yoyo để đảo ngược động tác
    }

    private void OnDisable()
    {
        _sequence?.Kill(); // Hủy sequence khi disable

        // Reset về trạng thái ban đầu
        transform.localScale = _originalScale;
        transform.position = _originalPosition;
    }
}
