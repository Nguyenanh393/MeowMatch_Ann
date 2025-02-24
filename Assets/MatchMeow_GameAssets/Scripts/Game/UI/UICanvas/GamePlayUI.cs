using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class GamePlayUI : UICanvas
{
    [SerializeField] private Transform topBarCanvas;
    [SerializeField] private Transform gamePlayObject;
    [SerializeField] private CountDownText countDownText;
    private Vector3 _topBarPosition;
    private Vector3 _gamePlayObjectCanvasPos;
    private bool _canSetTimer;

    public Transform TopBarCanvas => topBarCanvas;
    public CountDownText CountDownText => countDownText;
    private void Awake()
    {
        _topBarPosition = topBarCanvas.position;
        gamePlayObject = UIManager.Instance.GamePlayObject.transform;
        _gamePlayObjectCanvasPos = Vector3.zero;
    }

    private void OnEnable()
    {
        MakeObjectMoveIn().Forget();
    }

    private void Update()
    {
        if (_canSetTimer)
        {

        }
    }

    // private void OnDisable()
    // {
    //     UIManager.Instance.GamePlayObjectCanvas.gameObject.SetActive(false);
    // }

    private async UniTask MakeObjectMoveIn ()
    {
        _canSetTimer = false;
        UIManager.Instance.GamePlayObject.SetActive(true);
        topBarCanvas.position = _topBarPosition + Vector3.up;
        gamePlayObject.localPosition = _gamePlayObjectCanvasPos + Vector3.down * 1f;

        List<UniTask> tasks = new List<UniTask>();

        var tween1 = topBarCanvas.DOMoveY(_topBarPosition.y, 0.4f).SetEase(Ease.OutQuad);
        var tween2 = gamePlayObject.DOMoveY(_gamePlayObjectCanvasPos.y, 0.4f).SetEase(Ease.OutQuad);

        tasks.Add(tween1.ToUniTask());
        tasks.Add(tween2.ToUniTask());

        await UniTask.WhenAll(tasks);

        _canSetTimer = true;

    }
}

