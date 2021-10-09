using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderFailedWindow : MonoBehaviour
{
    [SerializeField] private Image _orderFailedImage;
    [Space]
    [SerializeField] private Text _orderRewardText;
    [SerializeField] private int _orderRewardAmount;
    [Space]
    [SerializeField] private Image _orderRaitingImage;
    [SerializeField] private Text _orderRaitingText;
    [SerializeField] private float _orderRaitingAmount;
    [Space]
    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _getContinueButton;
    [Space]
    [SerializeField] private Text _collectFoodAmountText;
    [SerializeField] private int _collectFoodAmountAmount;

    public void SetOrderCheckRaiting()
    {
        _orderRaitingAmount = _collectFoodAmountAmount * LevelOptions.instance.RaitingPerCollect;

        GameOptions.instance.CurrentOrderRaitingCollectValue = _orderRaitingAmount;

        TextAnimation.FloatNumberAnimation(_orderRaitingText, 0, _orderRaitingAmount, GameData.DEFAULT_DURATION);
    }

    public void SetOrderCheckReward()
    {
        _orderRewardAmount = _collectFoodAmountAmount * LevelOptions.instance.RewardPerCollect;

        GameOptions.instance.CurrentOrderRewardCollectValue = _orderRewardAmount;

        TextAnimation.IntNumberAnimation(_orderRewardText, 0, _orderRewardAmount, GameData.DEFAULT_DURATION);
    }

    public void SetOrderCheckCollectFood(int value)
    {
        _collectFoodAmountAmount = value;
        TextAnimation.IntNumberAnimation(_collectFoodAmountText, 0, _collectFoodAmountAmount, GameData.DEFAULT_DURATION);

        SetOrderCheckRaiting();
        SetOrderCheckReward();
    }
}
