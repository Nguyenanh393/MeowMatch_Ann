using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class LevelLoader
{
    public void LoadJson(int id)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(Constance.LevelLoader.LEVEL_PATH + id);

        if (textAsset != null)
        {
            Level level = ConvertTextAssetToLevel(textAsset);
            LevelManager.Instance.CurrentLevel = level;
            // Debug.Log("JSON data loaded successfully.");
            // Debug.Log(LevelManager.Instance.CurrentLevel);
        }
        else
        {
            Debug.LogError("Failed to load JSON file from Resources.");
        }
    }

    private Level ConvertTextAssetToLevel(TextAsset jsonFile)
    {
        var levelData = JObject.Parse(jsonFile.text);

        int levelId = (int) levelData[Constance.JsonKey.ID];
        int levelWidth = (int) levelData[Constance.JsonKey.WIDTH];
        int levelHeight = (int) levelData[Constance.JsonKey.HEIGHT];
        int[][] levelMap = levelData[Constance.JsonKey.MAP]!.ToObject<int[][]>();
        int levelMaxValue = (int)levelData[Constance.JsonKey.MAX_VALUE];
        int levelCoin = (int)levelData[Constance.JsonKey.COIN];

        return new Level(levelId, levelWidth, levelHeight, levelMap, levelMaxValue, levelCoin);
    }
}

