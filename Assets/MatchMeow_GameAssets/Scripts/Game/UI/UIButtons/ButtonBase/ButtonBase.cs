using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;


public class ButtonBase : GameUnit
{
    // [SerializeField] private Transform starSpawnPoint;
    // [SerializeField] private Button _button;
    private float _originalScale;
    private bool _isSpawning = false;

    private void Start()
    {
        _originalScale = TF.localScale.x;
    }

    public void OnSoundOn()
    {
        SoundManager.Instance.PlayButtonSound();
    }
    private void StartClicked()
    {
        TF.DOScale(_originalScale * 1.5f, 0.2f);
    }

    private void EndClicked()
    {
        TF.DOScale(_originalScale, 0.2f);
    }

    protected virtual async UniTask OnClickUniTask()
    {
        StartClicked();
        EndClicked();
    }

    public void OnClickVoid()
    {
        OnClickUniTask().Forget();
    }

}
