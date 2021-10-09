using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOptions : MonoBehaviour
{
    private PlayerMover _playerMover;

    [SerializeField] private bool _isFirstStart;

    [SerializeField] private bool _isGamePlay;

    [SerializeField] private int _currentOrderRewardCollectValue;

    [SerializeField] private float _currentOrderRaitingCollectValue;

    public static GameOptions instance;

    public bool IsFirstStart { get => _isFirstStart; }

    public int CurrentOrderRewardCollectValue { get => _currentOrderRewardCollectValue; set => _currentOrderRewardCollectValue = value; }
    public float CurrentOrderRaitingCollectValue { get => _currentOrderRaitingCollectValue; set => _currentOrderRaitingCollectValue = value; }

    private void Awake()
    {
        instance = this;

        _playerMover = FindObjectOfType<PlayerMover>();

        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        _isFirstStart = Statistics.instanse.GetStatistics(GameData.StatisticsType.FirstStartValue) == 0 ? true : false;

        if (_isFirstStart)
        {
            Statistics.instanse.SetStatistics(GameData.StatisticsType.FirstStartValue, 1);
            Debug.Log("Первый заход в игру!");
        }

        UIOptions.instance.UpdateUI(false);
    }

    public bool GetGamePlayStatus()
    {
        return _isGamePlay;
    }

    public void SetGamePlayStatus(bool status)
    {
        _isGamePlay = status;

        PlayerCamera.instance.SetFollow(status);

        if (Battery.instance != null)
        {
            Battery.instance.gameObject.SetActive(status);
        }

        if (_isGamePlay)
        {

        }
        else
        {

        }
    }

    public int GetLineNumber(Vector3 objectPosition)
    {
        switch (objectPosition.x)
        {
            case -9:
                return 0;

            case -6:
                return 1;

            case -3:
                return 2;

            case 0:
                return 3;

            case 3:
                return 4;

            case 6:
                return 5;

            case 9:
                return 6;

            default:
                Debug.Log("<color=red>ОБЪЕКТ НЕ ПЕРЕМЕЩЕН НА ЛИНИЮ. НЕ ВХОДИТ В ДИАПАЗОН</color>. Положение <color=yellow>x</color> равен: " + objectPosition.x);
                return 100; // Выдает значение не входящее в диапазон
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartLevel()
    {
        //_currentOrderRewardCollectValue = orderCard.GetOrderReward();
        //_currentOrderRaitingCollectValue = orderCard.GetOrderRaiting();

        //UIOptions.instance.SetOrderTargetCollectUI(orderCard);

        UIOptions.instance.UpdateUI(true);
        StartCoroutine(CameraMovement.instance.SwitchCameraPosition(CameraMovement.CameraPositionType.Game));
        Phone.instance.Window.CloseWindow();
        _playerMover.StartRotateAnimation(Vector3.zero);

        if (IsFirstStart)
        {
            LevelOptions.instance.ShowLevel(0);
        }
        else
        {
            LevelOptions.instance.ShowRandomLevel();
        }

        UIOptions.instance.GetCollectFoodProgressBar().SetLevelAmounts(LevelOptions.instance.GetOrderLevelAmount());
    }

    public void LoseLevel()
    {
        if (GetGamePlayStatus())
        {
            StartCoroutine(CameraMovement.instance.SwitchCameraPosition(CameraMovement.CameraPositionType.Menu));
            Phone.instance.Window.OpenWindow();

            UIOptions.instance.SetWindow(UIOptions.WindowType.OrderFailed, true);
            _playerMover.SetTriggerAnimation(PlayerMover.AnimationType.Defeat);

            Statistics.instanse.SetStatistics(GameData.StatisticsType.OrderFailed, 1);
        }
    }

    public void TakeMoney()
    {
        UIOptions.instance.SetUIResources(GameData.ResourceType.Money, _currentOrderRewardCollectValue * -1);
    }

    public void TakeRaiting()
    {
        UIOptions.instance.SetUIResources(GameData.ResourceType.Rating, _currentOrderRaitingCollectValue * -1);
    }

    public void WinLevel()
    {
        _playerMover.StartRotateAnimation(new Vector3(0, 180, 0));

        StartCoroutine(CameraMovement.instance.SwitchCameraPosition(CameraMovement.CameraPositionType.Menu));
        Phone.instance.Window.OpenWindow();

        UIOptions.instance.SetWindow(UIOptions.WindowType.OrderCheck, true);
        _playerMover.SetTriggerAnimation(PlayerMover.AnimationType.Victory);

        Statistics.instanse.SetStatistics(GameData.StatisticsType.OrderCompleted, 1);
    }

    public void GetMoney(int multiply = 1)
    {
        if (multiply < 1)
        {
            multiply = 1;
        }

        UIOptions.instance.SetUIResources(GameData.ResourceType.Money, _currentOrderRewardCollectValue * multiply);

        Statistics.instanse.SetStatistics(GameData.StatisticsType.EarnedCash, _currentOrderRewardCollectValue * multiply);
    }
    public void GetRaiting(int multiply = 1)
    {
        if (multiply < 1)
        {
            multiply = 1;
        }

        UIOptions.instance.SetUIResources(GameData.ResourceType.Rating, _currentOrderRaitingCollectValue * multiply);
    }

    public void GetDoubleMoney()
    {
        GetMoney(2);
    }

    public void LevelComplete()
    {
        if (GetGamePlayStatus())
        {
            SetGamePlayStatus(false);

            WinLevel();
        }
    }

}
