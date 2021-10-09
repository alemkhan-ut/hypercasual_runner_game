using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderCard : MonoBehaviour
{
    [SerializeField] private Button _orderButton;
    [Space]
    [SerializeField] private Image _orderAvatarImage;
    [Space]
    [SerializeField] private GameObject _orderTargetLess;
    [Space]
    [SerializeField] private OrderTarget _orderTargetPrefab;
    [SerializeField] private Transform _orderTargetsTransform;
    [SerializeField] private List<OrderTargetSettings> _orderTargetSettings = new List<OrderTargetSettings>();
    [Space]
    [SerializeField] private Text _orderCollectValueText;
    [SerializeField] private int _orderCollectValue;
    [Space]
    [SerializeField] private Text _orderRewardText;
    [SerializeField] private int _orderReward;
    [SerializeField] private Text _orderRatingText;
    [SerializeField] private float _orderRating;
    [Space]
    [SerializeField] private GameObject _orderBlockImage;
    [SerializeField] private Text _orderBlockRequiredRaitingText;
    [SerializeField] private int _orderBlockRequiredRaitingValue;
    [SerializeField] private Image _orderBlockRequiredRaitingImage;

    [SerializeField] private Sprite[] _orderAvatarImages;

    private void Awake()
    {
    }

    void Start()
    {
        _orderButton.onClick.AddListener(ButtonClickAction);

        CardUpdateUI();

        // OrderTargetsInstantiate(); // OLD VERSION

        _orderBlockImage.SetActive(!(GameData.GetResourcePrefs(GameData.ResourceType.Rating) >= (float)_orderBlockRequiredRaitingValue));
    }

    private void CardUpdateUI()
    {

        // OLD VERSION
        //_orderAvatarImage.sprite = GetRandomAvatarSprite();

        //_orderRewardText.text = _orderReward.ToString();
        //_orderRatingText.text = _orderRating.ToString();
        //_orderBlockRequiredRaitingText.text = _orderBlockRequiredRaitingValue.ToString();

        _orderAvatarImage.sprite = GetRandomAvatarSprite();

        _orderCollectValue = LevelOptions.instance.GetOrderLevelAmount()[transform.GetSiblingIndex()];
        _orderCollectValueText.text = _orderCollectValue.ToString();
        _orderRewardText.text = (_orderCollectValue * LevelOptions.instance.RewardPerCollect).ToString();
        _orderRatingText.text = Math.Round((_orderCollectValue * LevelOptions.instance.RaitingPerCollect), 2).ToString();
    }

    private void OrderTargetsInstantiate()
    {
        _orderTargetLess.SetActive(false);

        if (_orderTargetsTransform.childCount > 0)
        {
            for (int i = 0; i < _orderTargetsTransform.childCount; i++)
            {
                Destroy(_orderTargetsTransform.GetChild(i).gameObject);
            }
        }

        if (_orderTargetSettings.Count > 0)
        {
            for (int i = 0; i < _orderTargetSettings.Count; i++)
            {
                if (_orderTargetSettings[i].targetAmount > 0)
                {
                    OrderTarget orderTarget = Instantiate(_orderTargetPrefab, _orderTargetsTransform);

                    orderTarget.SetOrderTargetAmount(_orderTargetSettings[i].targetAmount);
                    orderTarget.SetOrderTargetSprite(GameData.instance.GetOrderTargetSprite(_orderTargetSettings[i].orderTargetType));
                }
            }
        }
    }

    private void ButtonClickAction()
    {
        GameOptions.instance.StartLevel();
    }

    public Sprite GetRandomAvatarSprite()
    {
        return _orderAvatarImages[UnityEngine.Random.Range(0, _orderAvatarImages.Length)];
    }
    public int GetOrderReward()
    {
        return _orderReward;
    }
    public float GetOrderRaiting()
    {
        return _orderRating;
    }
    public List<OrderTargetSettings> GetOrderTargetSettings()
    {
        return _orderTargetSettings;
    }
}

[System.Serializable]
public class OrderTargetSettings
{
    public string targetName;
    public GameData.OrderTargetType orderTargetType;
    public int targetAmount;
}
