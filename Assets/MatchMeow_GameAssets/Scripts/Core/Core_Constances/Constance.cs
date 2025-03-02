using UnityEngine;

public class Constance
{
    public class PlayerPref
    {
        public static string CURRENT_LEVEL_ID = "CurrentLevelId";
        public static string IS_SOUND_ON = "IsSoundOn";
        public static string IS_VIBRATION_ON = "IsVibrationOn";
        public static string COIN_VALUE = "CoinValue";
        public static string HEART_VALUE = "HeartValue";
        public static string REWARD_COLLECTED_KEY = "RewardCollected_Level_";
    }

    public class LevelLoader
    {
        public static string LEVEL_PATH = "Levels/";
    }

    public class JsonKey
    {
        public static string ID = "id";
        public static string WIDTH = "width";
        public static string HEIGHT = "height";
        public static string MAP = "map";
        public static string MAX_VALUE = "max_value";
        public static string COIN = "coin";
    }

    public class InGameObject
    {
        public static float BLOCK_SIZE = 1f;
        public static Vector2 BOARD_CENTER = new Vector2(0f, 0f);
    }

    public class ScreenInfo
    {
        public static float SCREEN_HEIGHT = 1920f;
        public static float SCREEN_WIDTH = 1080f;
    }

    public class RegardBonusAd
    {
        public static int COIN_NOT_BONUS = 1;
        public static int HEART_NOT_BONUS = 1;
        public static int COIN_BONUS_AD = 2;
        public static int HEART_BONUS_AD = 3;
    }

    public class RegardHeart
    {
        public static int HEART_VALUE = 8;
    }
}
