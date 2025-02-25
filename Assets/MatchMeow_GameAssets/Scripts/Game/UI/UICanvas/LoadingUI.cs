using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class LoadingUI : UICanvas
{
    [SerializeField] private RectTransform[] dots;
    [SerializeField] private RectTransform[] dotOriginalPositions;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float jumpDuration = 0.2f;
    [SerializeField] private float jumpDelay = 0.1f;

    private Sequence _sequence;

    private bool _isStarted;
    private Sequence _jumpSequence;

    private void Start()
    {
        // TF.position = originalPos.anchoredPosition;
        StartLoadingAnimation();
        _isStarted = true;
    }

    private void OnEnable()
    {
        if(!_isStarted) return;
        StartLoadingAnimation();
    }
    // private void OnEnable()
    // {
    //     StartLoadingAnimation();
    // }

    private void StartLoadingAnimation()
    {
        _sequence?.Kill();

        _sequence = DOTween.Sequence();

        for (int i = 0; i < dots.Length; i++)
        {
            RectTransform dot = dots[i];
            _sequence.Append(dot.DOJumpAnchorPos(dotOriginalPositions[i].anchoredPosition, jumpHeight, 1, jumpDuration))
                .SetEase(Ease.OutQuad)
                .SetDelay(jumpDelay);
        }

        _sequence.SetLoops(-1, LoopType.Restart); // Lặp lại vô hạn
    }

    private void OnDisable()
    {
        // Hủy sequence để dừng animation
        _sequence?.Kill();

        // Reset vị trí của các dots
        SetDotsPositions(dotOriginalPositions);
    }

    private void SetDotsPositions(RectTransform[] originalPositions)
    {
        for (int i = 0; i < originalPositions.Length; i++)
        {
            dots[i].anchoredPosition = originalPositions[i].anchoredPosition;
        }
    }
}
