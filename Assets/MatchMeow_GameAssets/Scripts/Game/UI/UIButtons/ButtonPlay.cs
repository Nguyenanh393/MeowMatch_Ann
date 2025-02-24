using System;
using _UI.Scripts.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ButtonPlay : ButtonBase
{
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

        // GameManager.ChangeState(GameState.GamePlay);
        UIManager.Instance.GamePlayObject.SetActive(true);
    }

    private void OnEnable()
    {
        SetRepeatEffect();
    }

    private void SetRepeatEffect()
    {
        Sequence sequence = DOTween.Sequence();

        // Bước 1: Phóng to (1.2, 1.2, 1) rồi thu nhỏ về (1,1,1)
        sequence.Append(transform.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.3f).SetEase(Ease.OutQuad));
        sequence.Append(transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InQuad)).SetDelay(0.3f);

        // Bước 2: Lắc góc 5 độ lên xuống (2 lần)
        sequence.Append(transform.DORotate(new Vector3(0, 0, 5), 0.1f).SetEase(Ease.InOutSine));
        sequence.Append(transform.DORotate(new Vector3(0, 0, -5), 0.1f).SetEase(Ease.InOutSine));
        sequence.Append(transform.DORotate(Vector3.zero, 0.1f).SetEase(Ease.InOutSine));

        // Lặp lại toàn bộ hiệu ứng
        sequence.SetLoops(-1, LoopType.Restart);
    }

}

