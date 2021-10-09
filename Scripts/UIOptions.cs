using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class UIOptions : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _startWindow;
    [SerializeField] private GameObject _firstStartWindow;
    [SerializeField] private GameObject _playersWaitWindow;

    [SerializeField] private OrderCheckWindow _orderCheckWindow;
    [SerializeField] private OrderFailedWindow _orderFailedWindow;
    [SerializeField] private GameObject _mainMenuWindow;
    [SerializeField] private GameObject _ordersWindow;
    [SerializeField] private GameObject _shopWindow;
    [SerializeField] private GameObject _lobbyWindow;
    [Space]
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _mainMenuUI;
    [SerializeField] private GameObject _centralUI;
    [SerializeField] private Text _roomInfoText;
    [SerializeField] private GameObject _negativeEffect;
    [SerializeField] private GameObject _positiveEffect;

    [Header("GAME UI ELEMENTS")]
    [SerializeField] private Image _airJumpFill;
    [Space]
    [SerializeField] private GameObject _orderTargetCollectPrefab;
    [SerializeField] private GameObject _orderTargetCollectContent;
    [SerializeField] private bool _isOrderTargetCoroutineStart;
    [Space]
    [Header("MENU UI ELEMENTS")]
    [SerializeField] private Text _playerNameText;
    [Space]
    [SerializeField] private Image _phoneLoadingFade;
    [SerializeField] private Image _phoneLoadingImage;
    [SerializeField] private Sprite[] _phoneLoadingSprites;
    [SerializeField] private float _phoneLoadingDuration;
    [SerializeField] private GameObject _phoneContent;
    [SerializeField] private GameObject _phoneStartBGImage;
    [Space]
    [SerializeField] private Text _orderCheckRewardText;
    [SerializeField] private Text _orderCheckRaitingText;
    [Space]
    [SerializeField] private Text _orderFailedRewardText;
    [SerializeField] private Text _orderFailedRaitingText;
    [Space]
    [SerializeField] private GameObject _inPhoneResoureces;
    [Space]
    [SerializeField] private Resource _raitingResource;
    [SerializeField] private float _raitingAmountValue;
    [SerializeField] private float _ratingAmountBasicValue;
    [Space]
    [SerializeField] private Resource _moneyResource;
    [SerializeField] private float _moneyAmountValue;
    [SerializeField] private float _moneyAmountBasicValue;
    [Space]
    [SerializeField] private Resource _keyResource;
    [SerializeField] private float _keyAmountValue;
    [SerializeField] private float _keyAmountBasicValue;
    [Space]
    [SerializeField] private Image _raitingProgressBarImage;
    [SerializeField] private Image[] _raitingProgressStars;
    [Space]
    [SerializeField] private float _fillAnimationDuration;
    [Space]

    public static UIOptions instance;

    public float FillAnimationDuration { get => _fillAnimationDuration; }
    public enum WindowType
    {
        OrderCheck,
        OrderFailed,
        MainMenu,
        Shop,
        Orders,
        Start,
        FirstStart,
        Lobby,
        PlayersWait
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        UpdateUI(false);

        if (GameOptions.instance.IsFirstStart)
        {
            SetWindow(WindowType.FirstStart, true);
        }
        else
        {
            SetWindow(WindowType.Start, true);
        }

    }

    private void Update()
    {
        if (_roomInfoText != null && PhotonNetwork.InRoom)
        {
            _roomInfoText.text = "ROOM NAME: " + PhotonNetwork.CurrentRoom.Name + "\nPlayers: " + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
        }
    }

    public void SetOrderTargetCollectUI(OrderCard orderCard)
    {
        int orderTargetCollectAmount = orderCard.GetOrderTargetSettings().Count;


        if (_orderTargetCollectContent.transform.childCount > 0)
        {
            for (int i = 0; i < _orderTargetCollectContent.transform.childCount; i++)
            {
                Destroy(_orderTargetCollectContent.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < orderTargetCollectAmount; i++)
        {
            if (orderCard.GetOrderTargetSettings()[i].targetAmount > 0)
            {
                GameObject _orderTargetCollect = Instantiate(_orderTargetCollectPrefab, _orderTargetCollectContent.transform);

                OrderTargetCollectContent.instance.AddOrderTargetCollect(_orderTargetCollect.GetComponent<OrderTargetCollect>());

                _orderTargetCollect.GetComponent<OrderTargetCollect>().SetType(orderCard.GetOrderTargetSettings()[i].orderTargetType);
                _orderTargetCollect.GetComponent<OrderTargetCollect>().SetAmount(orderCard.GetOrderTargetSettings()[i].targetAmount);
            }
        }
    }

    public void SetUIResources(GameData.ResourceType resourceType, float value = 0)
    {
        switch (resourceType)
        {
            case GameData.ResourceType.Rating:

                if (GameData.CheckResourcePrefs(GameData.ResourceType.Rating))
                {
                    _raitingAmountValue = GameData.GetResourcePrefs(GameData.ResourceType.Rating);
                }
                else
                {
                    GameData.SetResourcePrefs(resourceType, _ratingAmountBasicValue);
                    _raitingAmountValue = GameData.GetResourcePrefs(GameData.ResourceType.Rating);
                }


                _raitingAmountValue = (float)Math.Round((double)_raitingAmountValue, 2) + (float)Math.Round((double)value, 2);

                if (value > 0)
                {
                    Debug.Log("Заработано: " + value + " рейтинга. Всего рейтинга: " + _raitingAmountValue);

                    RaitingResourceFill(GameData.GetResourcePrefs(GameData.ResourceType.Rating), _raitingAmountValue, true);
                }
                else if (value < 0)
                {
                    Debug.Log("Потеряно: " + value + " рейтинга. Всего рейтинга: " + _raitingAmountValue);
                    RaitingResourceFill(GameData.GetResourcePrefs(GameData.ResourceType.Rating), _raitingAmountValue, false);
                }
                else
                {
                    Debug.Log("Рейтинг обновлён и равен: " + _raitingAmountValue);
                }

                GameData.SetResourcePrefs(GameData.ResourceType.Rating, _raitingAmountValue);

                //_ratingAmountText.text = GameData.GetResourcePrefs(GameData.ResourceType.Rating).ToString();
                break;

            case GameData.ResourceType.Money:

                if (GameData.CheckResourcePrefs(GameData.ResourceType.Money))
                {
                    _moneyAmountValue = GameData.GetResourcePrefs(GameData.ResourceType.Money);
                }
                else
                {
                    GameData.SetResourcePrefs(resourceType, _moneyAmountBasicValue);
                    _moneyAmountValue = GameData.GetResourcePrefs(GameData.ResourceType.Money);
                }

                _moneyAmountValue += value;

                if (value > 0)
                {
                    Debug.Log("Заработано: " + value + " денег. Всего денег: " + _moneyAmountValue);

                    MoneyResourceFill(GameData.GetResourcePrefs(GameData.ResourceType.Money), _moneyAmountValue, true);
                }
                else if (value < 0)
                {
                    Debug.Log("Потеряно: " + value + " денег. Всего денег: " + _moneyAmountValue);
                    MoneyResourceFill(GameData.GetResourcePrefs(GameData.ResourceType.Money), _moneyAmountValue, false);
                }
                else
                {
                    Debug.Log("Деньги обновлён и равны: " + _moneyAmountValue);
                }


                GameData.SetResourcePrefs(GameData.ResourceType.Money, _moneyAmountValue);

                //_moneyAmountText.text = GameData.GetResourcePrefs(GameData.ResourceType.Money).ToString();
                break;
            case GameData.ResourceType.Key:

                if (GameData.CheckResourcePrefs(GameData.ResourceType.Key))
                {
                    _keyAmountValue = GameData.GetResourcePrefs(GameData.ResourceType.Key);
                }
                else
                {
                    GameData.SetResourcePrefs(resourceType, _keyAmountBasicValue);
                    _keyAmountValue = GameData.GetResourcePrefs(GameData.ResourceType.Key);
                }

                _keyAmountValue += value;

                GameData.SetResourcePrefs(GameData.ResourceType.Key, _keyAmountValue);

                //_keyAmountText.text = GameData.GetResourcePrefs(GameData.ResourceType.Key).ToString();
                break;
            default:
                break;
        }
    }

    private IEnumerator PhoneLoadingCoroutine()
    {
        _phoneLoadingImage.sprite = _phoneLoadingSprites[UnityEngine.Random.Range(0, _phoneLoadingSprites.Length)];

        yield return _phoneLoadingImage.transform.DORotate(new Vector3(0, 0, 180), _phoneLoadingDuration).WaitForCompletion();
        _phoneLoadingImage.DOFade(0, _phoneLoadingDuration / 2);
        yield return _phoneLoadingFade.DOFade(0, _phoneLoadingDuration / 2).WaitForCompletion();
        _phoneStartBGImage.SetActive(false);

        _phoneContent.SetActive(true);

        RefreshInPhoneResources();

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SetWindow(WindowType.Lobby, true);
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            SetWindow(WindowType.PlayersWait, true);
        }
        else
        {
            SetWindow(WindowType.MainMenu, true);
        }
    }

    public IEnumerator NegativeEffect()
    {
        _negativeEffect.SetActive(true);

        yield return _negativeEffect.GetComponent<Image>().DOFade(0.15f, 0).WaitForCompletion();
        yield return _negativeEffect.GetComponent<Image>().DOFade(0f, 1f).WaitForCompletion();

        _negativeEffect.SetActive(false);
    }
    public IEnumerator PositiveEffect()
    {
        _positiveEffect.SetActive(true);

        yield return _positiveEffect.GetComponent<Image>().DOFade(0.15f, 0).WaitForCompletion();
        yield return _positiveEffect.GetComponent<Image>().DOFade(0f, 0.5f).WaitForCompletion();

        _positiveEffect.SetActive(false);
    }

    public void SetWindow(WindowType window, bool isOpen)
    {
        CloseAllWindow();

        if (isOpen)
        {
            GameOptions.instance.SetGamePlayStatus(false);
            Debug.Log("Игра приостановлена из за открытия окна " + window.ToString());
        }

        switch (window)
        {
            case WindowType.OrderCheck:
                _orderCheckWindow.gameObject.SetActive(isOpen);

                _orderCheckWindow.SetOrderCheckCollectFood(Player.instance.BoxBagContentAmount);

                _orderCheckWindow.CollectFoodProgressBar.SetFill(Player.instance.BoxBagContentAmount);

                if (isOpen)
                {
                    Debug.Log("Открыто окно победы");
                }
                break;
            case WindowType.OrderFailed:

                _orderFailedWindow.SetOrderCheckCollectFood(Player.instance.BoxBagContentAmount);

                _orderFailedWindow.gameObject.SetActive(isOpen);
                if (isOpen)
                {
                    Debug.Log("Открыто окно поражения");
                }
                break;
            case WindowType.MainMenu:
                _mainMenuWindow.SetActive(isOpen);

                if (isOpen)
                {
                    Debug.Log("Открыто окно главного меню");
                    RefreshMenu();
                }
                break;
            case WindowType.Shop:
                _shopWindow.SetActive(isOpen);
                if (isOpen)
                {
                    Debug.Log("Открыто окно магазина");
                }
                break;
            case WindowType.Orders:
                _ordersWindow.SetActive(isOpen);
                if (isOpen)
                {
                    Debug.Log("Открыто окно заказов");
                }
                break;
            case WindowType.Start:
                _startWindow.SetActive(true);

                StartCoroutine(PhoneLoadingCoroutine());

                if (isOpen)
                {
                    Debug.Log("Открыто окно старта");
                }
                break;
            case WindowType.FirstStart:

                _firstStartWindow.SetActive(true);

                if (isOpen)
                {
                    Debug.Log("Открыто окно первого запуска");
                }
                break;
            case WindowType.Lobby:
                _lobbyWindow.SetActive(true);

                if (isOpen)
                {
                    Debug.Log("Открыто окно лобби");
                }
                break;
            case WindowType.PlayersWait:
                _playersWaitWindow.SetActive(true);

                if (isOpen)
                {
                    Debug.Log("Открыто окно ожидания игроков");
                }
                break;
            default:
                break;
        }
    }

    public void CloseAllWindow() // TO DO: OPTIMIZE
    {
        _orderCheckWindow.gameObject.SetActive(false);
        _orderFailedWindow.gameObject.SetActive(false);
        _mainMenuWindow.SetActive(false);
        _startWindow.SetActive(false);
        _ordersWindow.SetActive(false);
        _shopWindow.SetActive(false);
        _firstStartWindow.SetActive(false);
        _lobbyWindow.SetActive(false);
    }

    public CollectFoodProgressBar GetCollectFoodProgressBar()
    {
        return _orderCheckWindow.CollectFoodProgressBar;
    }

    // ALL

    private void ResfreshAllUI()
    {
        if (_mainMenuWindow.activeSelf)
        {
            RefreshMenu();
        }
    }

    //

    // PHONE

    private void RefreshInPhoneResources()
    {
        _inPhoneResoureces.SetActive(true);

        MoneyResourceFill(0, GameData.GetResourcePrefs(GameData.ResourceType.Money), true);
        RaitingResourceFill(0, GameData.GetResourcePrefs(GameData.ResourceType.Rating), true);
        KeyResourceFill(0, GameData.GetResourcePrefs(GameData.ResourceType.Key), true);
    }

    public void MoneyResourceFill(float startValue, float endValue, bool isAdded)
    {
        _moneyResource.ResourceFill(startValue, endValue, isAdded);
    }

    public void RaitingResourceFill(float startValue, float endValue, bool isAdded)
    {
        _raitingResource.ResourceFill(startValue, endValue, isAdded);
    }

    public void KeyResourceFill(float startValue, float endValue, bool isAdded)
    {
        _keyResource.ResourceFill(startValue, endValue, isAdded);
    }

    // END PHONE


    // MAIN MENU

    private void RefreshMenu()
    {
        _playerNameText.text = GameData.GetPlayerName();
        StartCoroutine(RefreshRaitingProgressBar());
        Statistics.instanse.SetStatistics(GameData.StatisticsType.TotalTimeInGame);
        Debug.Log("Разница в секундах между началом и сейчас: " + (Mathf.Abs(GameData.instance.GameSessionStartDate.Second - DateTime.UtcNow.Second)).ToString());
        Statistics.instanse.RefreshStatistics(FillAnimationDuration);
        SoundOptions.instane.RefreshSoundOptions();
    }


    private IEnumerator RefreshRaitingProgressBar()
    {
        for (int i = 0; i < 5; i++)
        {
            _raitingProgressStars[i].color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        _raitingProgressBarImage.fillAmount = 0;

        float fillAmount = GameData.CheckResourcePrefs(GameData.ResourceType.Rating) == true ? GameData.GetResourcePrefs(GameData.ResourceType.Rating) / 5 : 0;

        yield return _raitingProgressBarImage.DOFillAmount(fillAmount, FillAnimationDuration);

        ProgressStarActivator(fillAmount);
    }

    private void ProgressStarActivator(float fillAmount)
    {
        switch (_raitingProgressBarImage.fillAmount)
        {
            case 0.2f:
                for (int i = 0; i < 1; i++)
                {
                    _raitingProgressStars[i].color = Color.white;
                }
                break;
            case 0.4f:
                for (int i = 0; i < 2; i++)
                {
                    _raitingProgressStars[i].color = Color.white;
                }
                break;
            case 0.6f:
                for (int i = 0; i < 3; i++)
                {
                    _raitingProgressStars[i].color = Color.white;
                }
                break;
            case 0.8f:
                for (int i = 0; i < 4; i++)
                {
                    _raitingProgressStars[i].color = Color.white;
                }
                break;
            case 1f:
                for (int i = 0; i < 5; i++)
                {
                    _raitingProgressStars[i].color = Color.white;
                }
                break;
            default:
                break;
        }
    }

    // END MAIN MENU

    public IEnumerator AddOrderTarget()
    {
        _isOrderTargetCoroutineStart = true;


        yield return new WaitForSeconds(0.2f);
        _isOrderTargetCoroutineStart = false;
    }

    public int GetCurrentOrderTargetValue()
    {
        return 0;
    }

    public bool CheckOrderTargetCoroutine()
    {
        return _isOrderTargetCoroutineStart;
    }

    // GAME UI

    public void SetBatteryEnergy(int value)
    {
        if (Battery.instance.GetEnergy() <= 0)
        {
            GameOptions.instance.LoseLevel();
        }
        else
        {
            Battery.instance.SetEnergy(value);
        }
    }

    public void CollectLost(int value)
    {
        if (Player.instance.BoxBagContentAmount > 0)
        {
            Player.instance.BagBox.GetComponent<BoxBag>().BoxBagSet(value);
        }
    }

    // END GAME UI

    public void UpdateUI(bool isGame)
    {
        _gameUI.SetActive(isGame);
        _mainMenuUI.SetActive(!isGame);

        ResfreshAllUI();
    }

    public void AirJumpFillChange(float fillValue, bool isRefill = false)
    {
        //_airJumpFill.fillAmount -= fillValue;

        //if (isRefill)
        //{
        //    _airJumpFill.fillAmount = 1f;
        //}
    }
}
