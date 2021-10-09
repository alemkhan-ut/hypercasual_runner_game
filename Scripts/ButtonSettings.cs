using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSettings : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Button _button;
    private Transform _tranform;

    [SerializeField] private ButtonType _buttonType;
    [SerializeField] private GameObject _target;

    [SerializeField] private bool _isResizable;

    [SerializeField] private bool _isWindowButton;
    [SerializeField] private UIOptions.WindowType _windowType;

    [SerializeField] private bool _isSettingsButton;
    [SerializeField] private GameData.SettingsType _settingsType;

    [SerializeField] private bool _isLanguageButton;
    [SerializeField] private GameObject[] _languagePoints;
    private bool _isLanguageButtonActive;

    private enum ButtonType
    {
        ChoiceButtonType,
        SetWindowButton,
        SettingsButton,
        LanguageButton,
        ChangeNameButton,
        HomeButton
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _tranform = GetComponent<Transform>();
    }

    private void Start()
    {
        _button.onClick.AddListener(ButtonAction);
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (_isResizable)
        {
            _tranform.DOScale(_tranform.localScale / 1.05f, 0);
        }
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (_isResizable)
        {
            _tranform.DOScale(_tranform.localScale * 1.05f, 0);
        }
    }

    private void ButtonAction()
    {
        switch (_buttonType)
        {
            case ButtonType.ChoiceButtonType:
                break;
            case ButtonType.SetWindowButton:
                break;
            case ButtonType.SettingsButton:
                break;
            case ButtonType.LanguageButton:
                break;
            case ButtonType.ChangeNameButton:
                break;
            case ButtonType.HomeButton:
                HomeButton();
                break;
            default:
                break;
        }

        if (_isWindowButton)
        {
            UIOptions.instance.SetWindow(_windowType, true);
        }
        else if (_isSettingsButton)
        {
            switch (_settingsType)
            {
                case GameData.SettingsType.MusicState:

                    if (SoundOptions.instane.IsMusicActive)
                    {
                        SoundOptions.instane.SetSettings(GameData.SettingsType.MusicState, 0);
                    }
                    else
                    {
                        SoundOptions.instane.SetSettings(GameData.SettingsType.MusicState, 1);
                    }

                    break;
                case GameData.SettingsType.SoundState:

                    if (SoundOptions.instane.IsSoundActive)
                    {
                        SoundOptions.instane.SetSettings(GameData.SettingsType.SoundState, 0);
                    }
                    else
                    {
                        SoundOptions.instane.SetSettings(GameData.SettingsType.SoundState, 1);
                    }

                    break;
                default:
                    break;
            }
        }
        else if (_isLanguageButton)
        {
            _isLanguageButtonActive = !_isLanguageButtonActive;

            for (int i = 0; i < _languagePoints.Length; i++)
            {
                _languagePoints[i].SetActive(_isLanguageButtonActive);
            }
        }
    }

    public void ChangeCourierName(TMP_InputField TMP_InputField)
    {
        GameData.SetPlayerName(TMP_InputField.text);
        _target.GetComponent<Text>().text = GameData.GetPlayerName();
    }

    public void GetReward()
    {
        GameOptions.instance.GetMoney();
        GameOptions.instance.GetRaiting();

        StartCoroutine(GetAnyRewardDelayCoroutine());
    }

    public void GetDoubleReward()
    {
        GameOptions.instance.GetDoubleMoney();

        StartCoroutine(GetAnyRewardDelayCoroutine());
    }

    private IEnumerator GetAnyRewardDelayCoroutine()
    {
        _button.interactable = false;
        yield return new WaitForSeconds(GameData.DEFAULT_DURATION * 2f);
        _target.SetActive(true);
        _button.gameObject.SetActive(false);
    }

    public void TakeResources()
    {
        GameOptions.instance.TakeMoney();
        GameOptions.instance.TakeRaiting();

        HomeButton();
    }

    public void HomeButton()
    {
        StartCoroutine(HomeButtonDelayCoroutine());
    }

    private IEnumerator HomeButtonDelayCoroutine()
    {
        _button.interactable = false;
        yield return new WaitForSeconds(GameData.DEFAULT_DURATION * 2f);
        GameOptions.instance.RestartLevel();
    }

    public void GetContinue()
    {
        // TO DO: ADD USE KEYS and SHOW ADs

        PlayerMover.instance.Reincarnation();
    }
}
