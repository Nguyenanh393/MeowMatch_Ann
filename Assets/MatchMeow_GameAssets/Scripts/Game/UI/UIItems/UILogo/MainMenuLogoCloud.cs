using System;
using DG.Tweening;
using UnityEngine;

public class MainMenuLogoCloud : GameUnit
{
    private void Start()
    {
        OnEnableEffect();
    }

    private void OnEnableEffect()
    {
        TF.localScale = new Vector3(1f, 1f, 1f);
        // TF.DOMoveY(_originalPos.y + 2f, 5f).From(_originalPos)
        //     .SetLoops(-1, LoopType.Yoyo) // Lặp lại lên xuống
        //     .SetEase(Ease.InOutSine); // Làm mượt hiệu ứng;
        TF.DORotate(new Vector3(0, 0, 4), 5f) // Xoay lên 10 độ trong 0.5 giây
            .SetLoops(-1, LoopType.Yoyo) // Lặp lại lên xuống
            .SetEase(Ease.InOutSine); // Làm mượt hiệu ứng

        TF.DOScale(1.1f, 5f) // Scale lên 1.1 trong 0.5 giây
            .SetLoops(-1, LoopType.Yoyo) // Lặp vô hạn kiểu Yoyo (tăng rồi giảm)
            .SetEase(Ease.InOutSine); // Làm mượt hiệu ứng
    }
}
