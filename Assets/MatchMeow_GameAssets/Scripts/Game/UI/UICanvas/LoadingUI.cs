using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LoadingUI : UICanvas
{
    [SerializeField] private Transform[] dots;
    [SerializeField] private Vector3[] dotPositions;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float jumpDuration = 0.2f;
    [SerializeField] private float jumpDelay = 0.1f;

    private Sequence _sequence;
    private void Awake()
    {
        dotPositions = GetDotsPositions();
    }

    private void OnEnable()
    {
        StartLoadingAnimation();
    }

    private void StartLoadingAnimation()
    {
        _sequence?.Kill();

        _sequence = DOTween.Sequence();

        for (int i = 0; i < dots.Length; i++)
        {
            Transform dot = dots[i];
            _sequence.Append(dot.DOJump(dotPositions[i], jumpHeight, 1, jumpDuration))
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
        SetDotsPositions(dotPositions);
    }

    private Vector3[] GetDotsPositions()
    {
        Vector3[] positions = new Vector3[dots.Length];

        for (int i = 0; i < dots.Length; i++)
        {
            positions[i] = dots[i].position;
        }

        return positions;
    }

    private void SetDotsPositions(Vector3[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
        {
            dots[i].position = positions[i];
        }
    }
}
