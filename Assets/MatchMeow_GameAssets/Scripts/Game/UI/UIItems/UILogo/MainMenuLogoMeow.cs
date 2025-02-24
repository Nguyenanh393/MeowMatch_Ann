using System;
using DG.Tweening;
using UnityEngine;

namespace MatchMeow_GameAssets.Scripts.Game.UI.UIItems.UILogo
{
    public class LogoMeowMainMenu : GameUnit
    {
        private Vector3 _originalPos;
        private bool _isStarted;

        private void Start()
        {
            _originalPos = TF.position;
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
            _originalPos = TF.position;
            Vector3 startPos = TF.position + new Vector3(0f, 2f, 0f);
            TF.position = startPos;
            float duration = 0.4f;  // Tổng thời gian nhảy

            Sequence jumpSequence = DOTween.Sequence();

            jumpSequence.Append(TF.DOMoveY(_originalPos.y, duration));

            jumpSequence.Join(TF.DOScale(new Vector3(1.2f, 1.2f, 1f), duration / 2));

            // Hiệu ứng scale khi lên đỉnh nhảy
            jumpSequence.Append(TF.DOScale(Vector3.one, duration / 4));

            // Hiệu ứng scale khi rơi xuống
            jumpSequence.Append(TF.DOScale(new Vector3(0.8f, 0.8f, 1f), duration / 4));

            // Hiệu ứng scale khi chạm đất
            jumpSequence.Append(TF.DOScale(Vector3.one, duration / 4));

            TF.localScale = new Vector3(1f, 1f, 1f);
            TF.DOScale(1.1f, 0.6f) // Scale lên 1.1 trong 0.5 giây
                .SetLoops(-1, LoopType.Yoyo) // Lặp vô hạn kiểu Yoyo (tăng rồi giảm)
                .SetEase(Ease.InOutSine); // Làm mượt hiệu ứng
        }
    }
}
