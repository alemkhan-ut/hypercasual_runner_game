using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    public static Statistics instanse;

    [SerializeField] private Text _ordersCompletedText;
    [SerializeField] private float _ordersCompleted;
    [SerializeField] private Text _ordersFailedText;
    [SerializeField] private float _ordersFailed;
    [Space]
    [SerializeField] private Text _earnedCashText;
    [SerializeField] private float _earnedCash;
    [Space]
    [SerializeField] private Text _totalTimeInGameText;
    [SerializeField] private TimeSpan _totalTimeInGame;

    private void Awake()
    {
        instanse = this;
    }

    private void Start()
    {

    }

    public void RefreshStatistics(float animationDuration)
    {
        _ordersCompleted = GetStatistics(GameData.StatisticsType.OrderCompleted);
        TextAnimation.IntNumberAnimation(_ordersCompletedText, 0, _ordersCompleted, animationDuration);
        //_ordersCompletedText.text = _ordersCompleted.ToString();

        _ordersFailed = GetStatistics(GameData.StatisticsType.OrderFailed);
        TextAnimation.IntNumberAnimation(_ordersFailedText, 0, _ordersFailed, animationDuration);
        //_ordersFailedText.text = _ordersFailed.ToString();

        _earnedCash = GetStatistics(GameData.StatisticsType.EarnedCash);
        TextAnimation.IntNumberAnimation(_earnedCashText, 0, _earnedCash, animationDuration);
        //_earnedCashText.text = _earnedCash.ToString();

        _totalTimeInGame = TimeSpan.FromSeconds(GetStatistics(GameData.StatisticsType.TotalTimeInGame));
        _totalTimeInGameText.text = (Math.Round((float)_totalTimeInGame.Hours, 0) + " час. " + Math.Round((float)_totalTimeInGame.Minutes, 0)).ToString() + " мин.";
        //DOTAnimation.NumberAnimation(_totalTimeInGameText, 0, _totalTimeInGame, _statisticsAnimationFillDuration);
        //_totalTimeInGameText.text = _totalTimeInGame.ToString();
    }

    public void SetStatistics(GameData.StatisticsType statisticsType) // СПЕЦИАЛЬНО ДЛЯ СТАТИСТИКИ TOTAL TIME
    {
        if (statisticsType == GameData.StatisticsType.TotalTimeInGame)
        {
            GameData.SetStatisticsPrefs(statisticsType, Mathf.Abs(GameData.instance.GameSessionStartDate.Second - DateTime.UtcNow.Second));
            RefreshStatistics(UIOptions.instance.FillAnimationDuration);
        }
    }

    public void SetStatistics(GameData.StatisticsType statisticsType, float value)
    {
        GameData.SetStatisticsPrefs(statisticsType, value);
        RefreshStatistics(UIOptions.instance.FillAnimationDuration);
    }
    public float GetStatistics(GameData.StatisticsType statisticsType)
    {
        return GameData.GetStatisticsPrefs(statisticsType);
    }

    public void DeleteStatistics(GameData.StatisticsType statisticsType)
    {
        GameData.DeleteStatisticsPrefs(statisticsType);
        RefreshStatistics(UIOptions.instance.FillAnimationDuration);
    }

    public void DeleteAllStatisctics()
    {
        GameData.DeleteAllStatisticsPrefs();

        GameOptions.instance.RestartLevel();
    }
}
