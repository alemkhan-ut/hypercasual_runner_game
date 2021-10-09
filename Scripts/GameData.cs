using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;


    public const string RATING_AMOUNT_PREFS = "Raiting_Amount";
    public const string MONEY_AMOUNT_PREFS = "Money_Amount";
    public const string KEY_AMOUNT_PREFS = "Key_Amount";

    public const string STATS_FIRST_START_VALUE_PREFS = "Stats_First_Start_Value";
    public const string STATS_FIRST_START_DATE_PREFS = "Stats_First_Start_Date";
    public const string STATS_ORDER_COMPLETED_PREFS = "Stats_Order_Completed";
    public const string STATS_ORDER_FAILED_PREFS = "Stats_Order_Failed";
    public const string STATS_EARNED_CASH_PREFS = "Stats_Earned_Cashs";
    public const string STATS_TOTAL_TIME_IN_GAME_PREFS = "Stats_Total_Time_In_Game";

    public const string PLAYER_NAME_PREFS = "Player_Name";
    public const string DEFAULT_PLAYER_NAME = "КУРЬЕР";

    public const string STATE_MUSIC = "State_Music";
    public const string STATE_SOUND = "State_Sound";

    public const string ANIMATOR_RUN_TRIGGER = "Running";
    public const string ANIMATOR_JUMP_TRIGGER = "Jump";
    public const string ANIMATOR_FALL_TRIGGER = "Fall";
    public const string ANIMATOR_SLIDE_TRIGGER = "Sliding";
    public const string ANIMATOR_DEFEAT_TRIGGER = "Defeat";
    public const string ANIMATOR_VICTORY_TRIGGER = "Victory";
    public const string ANIMATOR_ROOF_RUNNING_TRIGGER = "RoofRun";
    public const string ANIMATOR_ATTACK_TRIGGER = "Attack";

    public const float DEFAULT_DURATION = 1f;


    [SerializeField] private DateTime _gameSessionStartDate;

    [Header("Order Target Sprites")]
    [SerializeField] private Sprite[] _orderTargetSprites;

    [Header("Resources Sprites")]
    [SerializeField] private Sprite[] _iconSprites;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] _orderCollectPrefabs;

    public Sprite[] OrderTargetSprites { get => _orderTargetSprites; }
    public DateTime GameSessionStartDate { get => _gameSessionStartDate; }

    public enum ResourceType
    {
        Rating,
        Money,
        Key
    }

    public enum IconSpriteType
    {
        Raiting,
        Money
    }
    public enum OrderTargetType
    {
        Fries,
        Hamburger,
        Hotdog,
        Fish,
        Pizza,
        Prawn,
        Sandwich,
        SushiRoll
    }
    public enum StatisticsType
    {
        OrderCompleted,
        OrderFailed,
        EarnedCash,
        TotalTimeInGame,
        FirstStartValue,
        FirstStartDate
    }
    public enum SettingsType
    {
        MusicState,
        SoundState
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        _gameSessionStartDate = DateTime.UtcNow;
        Debug.Log("Game Launch Data: " + GameSessionStartDate);
    }

    private void OnApplicationPause(bool pause)
    {
        Statistics.instanse.SetStatistics(StatisticsType.TotalTimeInGame);
    }

    private void OnApplicationQuit()
    {
        Statistics.instanse.SetStatistics(StatisticsType.TotalTimeInGame);
    }

    #region RESOURCES

    public static bool CheckResourcePrefs(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Rating:
                if (PlayerPrefs.HasKey(RATING_AMOUNT_PREFS))
                {
                    return true;
                }
                break;
            case ResourceType.Money:
                if (PlayerPrefs.HasKey(MONEY_AMOUNT_PREFS))
                {
                    return true;
                }
                break;
            case ResourceType.Key:
                if (PlayerPrefs.HasKey(KEY_AMOUNT_PREFS))
                {
                    return true;
                }
                break;
            default:
                break;
        }

        return false;
    }

    public static float GetResourcePrefs(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Rating:
                return PlayerPrefs.GetFloat(RATING_AMOUNT_PREFS);
            case ResourceType.Money:
                return PlayerPrefs.GetFloat(MONEY_AMOUNT_PREFS);
            case ResourceType.Key:
                return PlayerPrefs.GetFloat(KEY_AMOUNT_PREFS);
            default:
                break;
        }

        return 0;
    }

    public static void SetResourcePrefs(ResourceType resourceType, float resourceValue)
    {
        switch (resourceType)
        {
            case ResourceType.Rating:

                if (resourceValue > 5)
                {
                    resourceValue = 5;
                }
                if (resourceValue < 0)
                {
                    resourceValue = 0;
                }

                PlayerPrefs.SetFloat(RATING_AMOUNT_PREFS, resourceValue);
                break;
            case ResourceType.Money:

                if (resourceValue < 0)
                {
                    resourceValue = 0;
                }

                PlayerPrefs.SetFloat(MONEY_AMOUNT_PREFS, resourceValue);
                break;
            case ResourceType.Key:

                if (resourceValue < 0)
                {
                    resourceValue = 0;
                }

                PlayerPrefs.SetFloat(KEY_AMOUNT_PREFS, resourceValue);
                break;
            default:
                break;
        }
    }

    #endregion RESOURCES

    #region DEBUG

    public void DebugSetRaitingResourcePrefs(float resourceValue)
    {
        PlayerPrefs.SetFloat(RATING_AMOUNT_PREFS, PlayerPrefs.GetFloat(RATING_AMOUNT_PREFS) + resourceValue);
        UIOptions.instance.UpdateUI(false);
    }

    public void DebugSetStatistics(float value)
    {
        Statistics.instanse.SetStatistics(StatisticsType.EarnedCash, value);
        Statistics.instanse.SetStatistics(StatisticsType.OrderCompleted, value);
        Statistics.instanse.SetStatistics(StatisticsType.OrderFailed, value);
        Statistics.instanse.SetStatistics(StatisticsType.TotalTimeInGame);
    }

    #endregion DEBUG

    #region SPRITES

    public Sprite GetOrderTargetSprite(OrderTargetType orderTargetType)
    {
        return OrderTargetSprites[(int)orderTargetType];
    }

    public Sprite GetIconSprite(IconSpriteType iconSpriteType)
    {
        return _iconSprites[(int)iconSpriteType];
    }

    #endregion SPRITES

    #region STATISTICS

    public static bool CheckStatisticsPrefs(StatisticsType statisticsType)
    {
        switch (statisticsType)
        {
            case StatisticsType.OrderCompleted:
                return PlayerPrefs.HasKey(STATS_ORDER_COMPLETED_PREFS);
            case StatisticsType.OrderFailed:
                return PlayerPrefs.HasKey(STATS_ORDER_FAILED_PREFS);
            case StatisticsType.EarnedCash:
                return PlayerPrefs.HasKey(STATS_EARNED_CASH_PREFS);
            case StatisticsType.TotalTimeInGame:
                return PlayerPrefs.HasKey(STATS_TOTAL_TIME_IN_GAME_PREFS);
            case StatisticsType.FirstStartValue:
                return PlayerPrefs.HasKey(STATS_FIRST_START_VALUE_PREFS);
            case StatisticsType.FirstStartDate:
                return PlayerPrefs.HasKey(STATS_FIRST_START_DATE_PREFS);
            default:
                return false;
        }
    }
    public static void SetStatisticsPrefs(StatisticsType statisticsType, float statisticsValue)
    {
        switch (statisticsType)
        {
            case StatisticsType.OrderCompleted:
                Debug.Log("Запрос на установку ORDER_COMPLETED_STATS_PREFS, значение: " + (PlayerPrefs.GetFloat(STATS_ORDER_COMPLETED_PREFS) + statisticsValue).ToString());
                PlayerPrefs.SetFloat(STATS_ORDER_COMPLETED_PREFS, PlayerPrefs.GetFloat(STATS_ORDER_COMPLETED_PREFS) + statisticsValue);
                break;
            case StatisticsType.OrderFailed:
                Debug.Log("Запрос на установку ORDER_FAILED_STATS_PREFS, значение: " + (PlayerPrefs.GetFloat(STATS_ORDER_FAILED_PREFS) + statisticsValue).ToString());
                PlayerPrefs.SetFloat(STATS_ORDER_FAILED_PREFS, PlayerPrefs.GetFloat(STATS_ORDER_FAILED_PREFS) + statisticsValue);
                break;
            case StatisticsType.EarnedCash:
                Debug.Log("Запрос на установку EARNED_CASH_STATS_PREFS, значение: " + (PlayerPrefs.GetFloat(STATS_EARNED_CASH_PREFS) + statisticsValue).ToString());
                PlayerPrefs.SetFloat(STATS_EARNED_CASH_PREFS, PlayerPrefs.GetFloat(STATS_EARNED_CASH_PREFS) + statisticsValue);
                break;
            case StatisticsType.TotalTimeInGame:
                Debug.Log("Запрос на установку TOTAL_TIME_IN_GAME_STATS_PREFS, значение: " + (PlayerPrefs.GetFloat(STATS_TOTAL_TIME_IN_GAME_PREFS) + statisticsValue).ToString());
                PlayerPrefs.SetFloat(STATS_TOTAL_TIME_IN_GAME_PREFS, PlayerPrefs.GetFloat(STATS_TOTAL_TIME_IN_GAME_PREFS) + statisticsValue);
                break;
            case StatisticsType.FirstStartValue:
                Debug.Log("Запрос на установку STATS_FIRST_START_VALUE_PREFS, значение: " + (PlayerPrefs.GetFloat(STATS_FIRST_START_VALUE_PREFS) + statisticsValue).ToString());
                PlayerPrefs.SetFloat(STATS_FIRST_START_VALUE_PREFS, PlayerPrefs.GetFloat(STATS_FIRST_START_VALUE_PREFS) + statisticsValue);
                break;
            case StatisticsType.FirstStartDate:
                Debug.Log("Запрос на установку STATS_FIRST_START_DATE_PREFS, значение: " + (PlayerPrefs.GetFloat(STATS_FIRST_START_DATE_PREFS) + statisticsValue).ToString());
                PlayerPrefs.SetFloat(STATS_FIRST_START_DATE_PREFS, PlayerPrefs.GetFloat(STATS_FIRST_START_DATE_PREFS) + statisticsValue);
                break;
            default:
                Debug.Log("Запрос на установку не обработан, значение: " + statisticsValue);
                break;
        }
    }

    public static float GetStatisticsPrefs(StatisticsType statisticsType)
    {
        if (CheckStatisticsPrefs(statisticsType))
        {
            switch (statisticsType)
            {
                case StatisticsType.OrderCompleted:
                    Debug.Log("Запрос на получении ORDER_COMPLETED_STATS_PREFS, значение: " + PlayerPrefs.GetFloat(STATS_ORDER_COMPLETED_PREFS));
                    return PlayerPrefs.GetFloat(STATS_ORDER_COMPLETED_PREFS);
                case StatisticsType.OrderFailed:
                    Debug.Log("Запрос на получении ORDER_FAILED_STATS_PREFS, значение: " + PlayerPrefs.GetFloat(STATS_ORDER_FAILED_PREFS));
                    return PlayerPrefs.GetFloat(STATS_ORDER_FAILED_PREFS);
                case StatisticsType.EarnedCash:
                    Debug.Log("Запрос на получении EARNED_CASH_STATS_PREFS, значение: " + PlayerPrefs.GetFloat(STATS_EARNED_CASH_PREFS));
                    return PlayerPrefs.GetFloat(STATS_EARNED_CASH_PREFS);
                case StatisticsType.TotalTimeInGame:
                    Debug.Log("Запрос на получении TOTAL_TIME_IN_GAME_STATS_PREFS, значение: " + PlayerPrefs.GetFloat(STATS_TOTAL_TIME_IN_GAME_PREFS));
                    return PlayerPrefs.GetFloat(STATS_TOTAL_TIME_IN_GAME_PREFS);
                case StatisticsType.FirstStartValue:
                    Debug.Log("Запрос на получении STATS_FIRST_START_VALUE_PREFS, значение: " + PlayerPrefs.GetFloat(STATS_FIRST_START_VALUE_PREFS));
                    return PlayerPrefs.GetFloat(STATS_FIRST_START_VALUE_PREFS);
                case StatisticsType.FirstStartDate:
                    Debug.Log("Запрос на получении STATS_FIRST_START_DATE_PREFS, значение: " + PlayerPrefs.GetFloat(STATS_FIRST_START_DATE_PREFS));
                    return PlayerPrefs.GetFloat(STATS_FIRST_START_DATE_PREFS);
                default:
                    Debug.Log("Запрос на получение не обработан. Источник: " + statisticsType.ToString());
                    return 0;
            }
        }
        else
        {
            SetStatisticsPrefs(statisticsType, 0);
            return 0;
        }
    }

    public static void DeleteAllStatisticsPrefs()
    {
        foreach (StatisticsType statisticsType in Enum.GetValues(typeof(StatisticsType)))
        {
            DeleteStatisticsPrefs(statisticsType);
        }
    }

    public static void DeleteStatisticsPrefs(StatisticsType statisticsType)
    {
        switch (statisticsType)
        {
            case StatisticsType.OrderCompleted:
                PlayerPrefs.DeleteKey(STATS_ORDER_COMPLETED_PREFS);
                Debug.Log("Запрос на удаление: STATS_ORDER_COMPLETED_PREFS");
                ; break;
            case StatisticsType.OrderFailed:
                PlayerPrefs.DeleteKey(STATS_ORDER_FAILED_PREFS);
                Debug.Log("Запрос на удаление: STATS_ORDER_FAILED_PREFS");
                break;
            case StatisticsType.EarnedCash:
                PlayerPrefs.DeleteKey(STATS_EARNED_CASH_PREFS);
                Debug.Log("Запрос на удаление: STATS_EARNED_CASH_PREFS");
                break;
            case StatisticsType.TotalTimeInGame:
                PlayerPrefs.DeleteKey(STATS_TOTAL_TIME_IN_GAME_PREFS);
                Debug.Log("Запрос на удаление: STATS_TOTAL_TIME_IN_GAME_PREFS");
                break;
            case StatisticsType.FirstStartValue:
                PlayerPrefs.DeleteKey(STATS_FIRST_START_VALUE_PREFS);
                Debug.Log("Запрос на удаление: STATS_FIRST_START_VALUE_PREFS");
                break;
            case StatisticsType.FirstStartDate:
                PlayerPrefs.DeleteKey(STATS_FIRST_START_DATE_PREFS);
                Debug.Log("Запрос на удаление: STATS_FIRST_START_DATE_PREFS");
                break;
            default:
                break;
        }
    }

    #endregion STATISTICS

    #region SETTINGS

    public static bool CheckSettingsPrefs(SettingsType settingsType)
    {
        switch (settingsType)
        {
            case SettingsType.MusicState:
                return PlayerPrefs.HasKey(STATE_MUSIC);
            case SettingsType.SoundState:
                return PlayerPrefs.HasKey(STATE_SOUND);
            default:
                return false;
        }
    }
    public static void SetSettingsPrefs(SettingsType settingsType, float settingsValue)
    {
        switch (settingsType)
        {
            case SettingsType.MusicState:
                if (settingsValue > 1)
                {
                    settingsValue = 1;
                }
                if (settingsValue <= 0)
                {
                    settingsValue = 0;
                }
                Debug.Log("Запрос на установку STATE_MUSIC, значение: " + (settingsValue).ToString());
                PlayerPrefs.SetFloat(STATE_MUSIC, settingsValue);
                break;
            case SettingsType.SoundState:
                if (settingsValue > 1)
                {
                    settingsValue = 1;
                }
                if (settingsValue <= 0)
                {
                    settingsValue = 0;
                }
                Debug.Log("Запрос на установку STATE_SOUND, значение: " + (settingsValue).ToString());
                PlayerPrefs.SetFloat(STATE_SOUND, settingsValue);
                break;
            default:
                Debug.Log("Запрос на установку не обработан, значение: " + settingsValue);
                break;
        }
    }

    public static float GetSettingsPrefs(SettingsType settingsType)
    {
        if (CheckSettingsPrefs(settingsType))
        {
            switch (settingsType)
            {
                case SettingsType.MusicState:
                    Debug.Log("Запрос на получении STATE_MUSIC, значение: " + PlayerPrefs.GetFloat(STATE_MUSIC));
                    return PlayerPrefs.GetFloat(STATE_MUSIC);
                case SettingsType.SoundState:
                    Debug.Log("Запрос на получении STATE_SOUND, значение: " + PlayerPrefs.GetFloat(STATE_SOUND));
                    return PlayerPrefs.GetFloat(STATE_SOUND);
                default:
                    Debug.Log("Запрос на получение не обработан. Источник: " + settingsType.ToString());
                    return 0;
            }
        }
        else
        {
            SetSettingsPrefs(settingsType, 0);
            return 0;
        }
    }

    public static void DeleteSettingsPrefs(SettingsType settingsType)
    {
        switch (settingsType)
        {
            case SettingsType.MusicState:
                PlayerPrefs.DeleteKey(STATE_MUSIC);
                break;
            case SettingsType.SoundState:
                PlayerPrefs.DeleteKey(STATE_SOUND);
                break;
            default:
                break;
        }
    }
    #endregion SETTINGS

    #region PREFABS

    public GameObject GetOrderCollectPrefab(OrderTargetType orderTargetType)
    {
        return _orderCollectPrefabs[(int)orderTargetType];
    }

    #endregion PREFABS

    public static bool CheckPlayerName()
    {
        if (PlayerPrefs.HasKey(PLAYER_NAME_PREFS))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void SetPlayerName(string Name)
    {
        PlayerPrefs.SetString(PLAYER_NAME_PREFS, Name);
    }

    public static string GetPlayerName()
    {
        if (!CheckPlayerName())
        {
            SetPlayerName(DEFAULT_PLAYER_NAME);
        }

        return PlayerPrefs.GetString(PLAYER_NAME_PREFS);
    }

    public static void DeletePlayerName()
    {
        PlayerPrefs.DeleteKey(PLAYER_NAME_PREFS);
    }
}
