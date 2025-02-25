using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    private int _currentLevelId = 1;
    private Level _currentLevel;

    public int CurrentLevelId => _currentLevelId;
    public Level CurrentLevel
    {
        get => _currentLevel;
        set => _currentLevel = value;
    }

    private void Awake()
    {
        // PlayerPrefs.DeleteAll();
        _currentLevelId = PlayerPrefs.GetInt(Constance.PlayerPref.CURRENT_LEVEL_ID, 1);
        LevelLoader levelLoader = new LevelLoader();
        levelLoader.LoadJson(_currentLevelId);
        PlayerPrefs.SetInt(Constance.PlayerPref.CURRENT_LEVEL_ID, _currentLevelId);

    }

    public void LoadLevel(int currentLevelId)
    {
        if (_currentLevelId != currentLevelId)
        {
            _currentLevelId = currentLevelId;
            LevelLoader levelLoader = new LevelLoader();
            levelLoader.LoadJson(_currentLevelId);
            // PlayerPrefs.SetInt(Constance.PlayerPref.CURRENT_LEVEL_ID, _currentLevelId);
            // ng choi chi co the load nhung level da choi
        }
        Debug.Log(BlockManager.Instance.MatrixToString(LevelManager.Instance.CurrentLevel.Map));
        BlockManager.Instance.RemoveAllMapBoard();
        BlockManager.Instance.OnInit();
        BlockManager.Instance.LoadMapBoard();
        Debug.Log(BlockManager.Instance.MatrixToString(BlockManager.Instance.CurrentMap));
    }

    [ContextMenu("LoadNextLevel")]
    public void LoadNextLevel()
    {
        LoadLevel(_currentLevelId + 1);
        PlayerPrefs.SetInt(Constance.PlayerPref.CURRENT_LEVEL_ID, _currentLevelId);
    }

    [ContextMenu("ReloadLevel")]
    public void ReloadLevel()
    {
        LoadLevel(_currentLevelId);
    }

}

