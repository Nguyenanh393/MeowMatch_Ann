using System;
using DG.Tweening;
using UnityEngine;

namespace MatchMeow_GameAssets.Scripts.Game.UI.UIItems.UILogo
{
    public class LogoMeowMainMenu : GameUnit
    {
        [SerializeField] private RectTransform startPos;
        [SerializeField] private RectTransform originalPos;
        [SerializeField] private RectTransform thisRect;
        private bool _isStarted;
        private Sequence _jumpSequence;

        private void Start()
        {
            TF.position = originalPos.position;
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
            float duration = 0.4f;  // Tổng thời gian nhảy

            _jumpSequence = DOTween.Sequence();

            _jumpSequence.Append(thisRect.DOAnchorPosY(originalPos.anchoredPosition.y, duration));

            _jumpSequence.Join(thisRect.DOScale(new Vector3(1.2f, 1.2f, 1f), duration / 2));

            // Hiệu ứng scale khi lên đỉnh nhảy
            _jumpSequence.Append(thisRect.DOScale(Vector3.one, duration / 4));

            // Hiệu ứng scale khi rơi xuống
            _jumpSequence.Append(thisRect.DOScale(new Vector3(0.8f, 0.8f, 1f), duration / 4));

            // Hiệu ứng scale khi chạm đất
            _jumpSequence.Append(thisRect.DOScale(Vector3.one, duration / 4));

            thisRect.localScale = new Vector3(1f, 1f, 1f);
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
}
