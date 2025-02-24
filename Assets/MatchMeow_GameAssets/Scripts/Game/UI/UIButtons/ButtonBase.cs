using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;


public class ButtonBase : GameUnit
{
    [SerializeField] private Transform starSpawnPoint;
    // [SerializeField] private Button _button;
    private float _originalScale;
    private bool _isSpawning = false;

    private void Start()
    {
        _originalScale = TF.localScale.x;
        // _button.onClick.AddListener(OnClickVoid);
    }

    private void StartClicked()
    {
        TF.DOScale(_originalScale * 1.5f, 0.2f);
        SoundManager.Instance.PlayButtonSound();
    }

    private void EndClicked()
    {
        TF.DOScale(_originalScale, 0.2f);
    }

    protected virtual async UniTask OnClickUniTask()
    {
        StartClicked();
        EndClicked();
        await SpawnStars();
    }

    public void OnClickVoid()
    {
        OnClickUniTask().Forget();
    }

    private async UniTask SpawnStars()
    {
        if (_isSpawning) return; // Nếu đang chạy, không thực hiện tiếp
        _isSpawning = true;

        List<UniTask> starTasks = new List<UniTask>();
        for (int i = 0; i < 20; i++)
        {
            StarButton starButton = SimplePool.Spawn<StarButton>(PoolType.POOLTYPE_BUTTON_STAR,
                starSpawnPoint.position,
                Quaternion.identity);
            starButton.OnInit();
            Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized * 1.5f;
            var tween1 = starButton.TF.DOScale(0.3f, 0.35f).OnComplete(() => DeSpawnStar(starButton));
            var tween2 = starButton.TF.DOMove((Vector2)starSpawnPoint.position + randomDirection,
                    0.6f)
                .SetEase(Ease.OutQuad);
            starTasks.Add(tween1.ToUniTask());
            starTasks.Add(tween2.ToUniTask());
        }
        await UniTask.WhenAll(starTasks);
        _isSpawning = false;
    }

    private void DeSpawnStar(StarButton starButton)
    {
        SimplePool.Despawn(starButton);
    }
}
