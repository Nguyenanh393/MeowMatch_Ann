using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using _UI.Scripts;
using _UI.Scripts.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    //[SerializeField] UserData userData;
    //[SerializeField] CSVData csv;
    private static GameState gameState = GameState.MainMenu;
    private bool isPause = false;
    // Start is called before the first frame update
    protected void Awake()
    {
        //base.Awake();
        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        int maxScreenHeight = 1280;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }

        //csv.OnInit();
        //userData?.OnInitData();

        // ChangeState(GameState.MainMenu);
     }

    // public static void ChangeState(GameState state)
    // {
    //     gameState = state;
    // }
    //
    // public static bool IsState(GameState state)
    // {
    //     return gameState == state;
    // }
    //
    // public bool IsPause
    // {
    //     get => isPause;
    //     set => isPause = value;
    // }
    private void Start()
    {
        UIManager.Instance.OpenUI<MainMenuUI>();
    }

    public async UniTaskVoid OnWinState()
    {
        UIManager.Instance.GetUI<GamePlayUI>().CountDownText.PauseCountdown();
        SoundManager.Instance.PlayWinSound();
        UIManager.Instance.OpenUI<WinUI>();
        UIManager.Instance.GamePlayObject.SetActive(false);
    }

    public async UniTaskVoid OnLoseState()
    {
        UIManager.Instance.GetUI<GamePlayUI>().CountDownText.PauseCountdown();
        SoundManager.Instance.PlayLoseSound();
        UIManager.Instance.OpenUI<LoseUI>();
        UIManager.Instance.GamePlayObject.SetActive(false);
    }

    public async UniTaskVoid OnLoadNextLevel()
    {
        UIManager.Instance.GamePlayObject.SetActive(false);
        LevelManager.Instance.LoadNextLevel();
        UIManager.Instance.CloseUI<GamePlayUI>();
        UIManager.Instance.OpenUI<LoadingUI>();
        UIManager.Instance.CloseUI<LoadingUI>(2f);
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        UIManager.Instance.OpenUI<GamePlayUI>();
        UIManager.Instance.GetUI<GamePlayUI>().CountDownText.ResetCountdown();
    }

    public async UniTaskVoid OnReloadGame()
    {
        UIManager.Instance.GamePlayObject.SetActive(false);
        LevelManager.Instance.ReloadLevel();
        UIManager.Instance.CloseUI<GamePlayUI>();
        UIManager.Instance.OpenUI<LoadingUI>();
        UIManager.Instance.CloseUI<LoadingUI>(2f);
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        UIManager.Instance.OpenUI<GamePlayUI>();
        UIManager.Instance.GetUI<GamePlayUI>().CountDownText.ResetCountdown();
    }


}
