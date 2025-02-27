using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ButtonHomeCat : ButtonBase
{
    protected override async UniTask OnClickUniTask()
    {
        await base.OnClickUniTask();
        // UIManager.Instance.OpenUI<GamePlayUI>();
        // GameManager.ChangeState(GameState.GamePlay);
        UIManager.Instance.CloseUI<MainMenuUI>();
        // UIManager.Instance.GamePlayObjectCanvas.gameObject.SetActive(true);

        UIManager.Instance.OpenUI<CatLoadingUI>();
        UIManager.Instance.CloseUI<CatLoadingUI>(2f);

        await UniTask.Delay(TimeSpan.FromSeconds(2));
        // UIManager.Instance.OpenUI<GamePlayUI>();

        GameManager.Instance.ChangeState(GameState.CatGamePlay);
        // UIManager.Instance.GamePlayObject.SetActive(true);
    }

    private void OnEnable()
    {
        SetRepeatEffect();
    }

    private void SetRepeatEffect()
    {
        TF.localScale = Vector3.one;
        // Bước 1: Phóng to (1.2, 1.2, 1) rồi thu nhỏ về (1,1,1)
        TF.DOScale(new Vector3(1.2f, 1.2f, 1f), 0.3f).SetEase(Ease.OutQuad).SetLoops(-1, LoopType.Yoyo);

    }

}

