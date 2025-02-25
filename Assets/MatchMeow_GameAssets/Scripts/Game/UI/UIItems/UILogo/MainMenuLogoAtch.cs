using DG.Tweening;
using UnityEngine;


public class MainMenuLogoAtch : GameUnit
{
    [SerializeField] private RectTransform thisRect;
    [SerializeField] private RectTransform originalPos;
    [SerializeField] private RectTransform startPos;
    private bool _isStarted;
    private Sequence _jumpSequence;
    private void Start()
    {
        OnEnableEffect();
        _isStarted = true;
    }

    private void OnEnable()
    {
        if (!_isStarted) return;
        OnEnableEffect();
    }

    private void OnEnableEffect()
    {
        thisRect.anchoredPosition = startPos.anchoredPosition;
        float duration = 0.2f;  // Tổng thời gian nhảy

        _jumpSequence = DOTween.Sequence();

        _jumpSequence.Append(thisRect.DOAnchorPosX(originalPos.anchoredPosition.x, duration));

        _jumpSequence.Join(thisRect.DOScale(new Vector3(1.2f, 0.8f, 1f), duration / 2));

        // Hiệu ứng scale khi lên đỉnh nhảy
        _jumpSequence.Append(thisRect.DOScale(Vector3.one, duration / 4));

        // Hiệu ứng scale khi rơi xuống
        _jumpSequence.Append(thisRect.DOScale(new Vector3(0.8f, 1.2f, 1f), duration / 4));

        // Hiệu ứng scale khi chạm đất
        _jumpSequence.Append(thisRect.DOScale(Vector3.one, duration / 4));

        thisRect.localScale = Vector3.one;
        thisRect.DOScale(1.1f, 0.6f) // Scale lên 1.1 trong 0.5 giây
            .SetLoops(-1, LoopType.Yoyo) // Lặp vô hạn kiểu Yoyo (tăng rồi giảm)
            .SetEase(Ease.InOutSine); // Làm mượt hiệu ứng
    }

    private void OnDisable()
    {
        thisRect.localScale = Vector3.one;
        thisRect.DOKill();
    }
}
