using System;
// using _UI.Scripts.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ButtonPlay : ButtonBase
{
    private Sequence _sequence;
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();
        // UIManager.Instance.OpenUI<GamePlayUI>();
        // GameManager.ChangeState(GameState.GamePlay);
        UIManager.Instance.CloseUI<MainMenuUI>();
        // UIManager.Instance.GamePlayObjectCanvas.gameObject.SetActive(true);

        UIManager.Instance.OpenUI<LoadingUI>();
        UIManager.Instance.CloseUI<LoadingUI>(2f);

        await UniTask.Delay(TimeSpan.FromSeconds(2));
        UIManager.Instance.OpenUI<GamePlayUI>();

        GameManager.Instance.ChangeState(GameState.GamePlay);
        UIManager.Instance.GamePlayObject.SetActive(true);
    }

    private void OnEnable()
    {
        SetRepeatEffect();
    }

    private void SetRepeatEffect()
    {
        _sequence = DOTween.Sequence();

        // Bước 1: Phóng to (1.2, 1.2, 1) rồi thu nhỏ về (1,1,1)
        _sequence.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.3f).SetEase(Ease.OutQuad)).SetLoops(-1, LoopType.Yoyo);

        // Bước 2: Lắc góc 5 độ lên xuống (2 lần)
        _sequence.Append(transform.DORotate(new Vector3(0, 0, 5), 0.1f).SetEase(Ease.InOutSine));
        _sequence.Append(transform.DORotate(new Vector3(0, 0, -5), 0.1f).SetEase(Ease.InOutSine));
        _sequence.Append(transform.DORotate(Vector3.zero, 0.1f).SetEase(Ease.InOutSine));

        _sequence.Append(transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InQuad));

        // Lặp lại toàn bộ hiệu ứng
        _sequence.SetLoops(-1, LoopType.Restart);
    }

    private void OnDisable()
    {
        _sequence?.Kill();
    }
}

