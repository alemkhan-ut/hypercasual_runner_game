using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderCheckWindow : MonoBehaviour
{
    [SerializeField] private Image _orderCheckImage;
    [SerializeField] private Text _orderCheckRewardText;
    [SerializeField] private int _orderCheckRewardAmount;
    [Space]
    [SerializeField] private Image _orderRaitingImage;
    [SerializeField] private Text _orderRaitingText;
    [SerializeField] private float _orderRaitingAmount;
    [Space]
    [SerializeField] private Button _orderGetRewardButton;
    [SerializeField] private Button _orderGetDoubleRewardButton;
    [SerializeField] private Button _orderHomeButton;
    [Space]
    [SerializeField] private CollectFoodProgressBar _collectFoodProgressBar;
    [SerializeField] private Text _collectFoodAmountText;
    [SerializeField] private int _collectFoodAmountAmount;

    public CollectFoodProgressBar CollectFoodProgressBar { get => _collectFoodProgressBar; }

    public void SetOrderCheckRaiting()
    {
        _orderRaitingAmount = _collectFoodAmountAmount * LevelOptions.instance.RaitingPerCollect;

        GameOptions.instance.CurrentOrderRaitingCollectValue = _orderRaitingAmount;

        TextAnimation.FloatNumberAnimation(_orderRaitingText, 0, _orderRaitingAmount, GameData.DEFAULT_DURATION);
    }

    public void SetOrderCheckReward()
    {
        _orderCheckRewardAmount = _collectFoodAmountAmount * LevelOptions.instance.RewardPerCollect;

        GameOptions.instance.CurrentOrderRewardCollectValue = _orderCheckRewardAmount;

        TextAnimation.IntNumberAnimation(_orderCheckRewardText, 0, _orderCheckRewardAmount, GameData.DEFAULT_DURATION);
    }

    public void SetOrderCheckCollectFood(int value)
    {
        _collectFoodAmountAmount = value;
        TextAnimation.IntNumberAnimation(_collectFoodAmountText, 0, _collectFoodAmountAmount, GameData.DEFAULT_DURATION);

        SetOrderCheckRaiting();
        SetOrderCheckReward();
    }
}
