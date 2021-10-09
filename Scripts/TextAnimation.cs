using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class TextAnimation : MonoBehaviour
{
    private Text _textComponent;

    [SerializeField] private bool _isAwakeAnimation;

    [Header("NUMBER ANIMATION")]
    [SerializeField] private bool _isNumberAnimation;
    [SerializeField] private float _fromValue;
    [SerializeField] private float _toValue;
    [Header("STRING ANIMATION")]
    [SerializeField] private bool _isStringAnimation;
    [SerializeField] private bool isAwakeAnimation;
    [SerializeField] private string _textTo;

    [SerializeField] private float _aniamtionDuration;

    private void Awake()
    {
        if (GetComponent<Text>())
        {
            _textComponent = GetComponent<Text>();
        }
    }

    private void Start()
    {
        if (_isAwakeAnimation)
        {
            if (_isNumberAnimation)
            {
                IntNumberAnimation(_textComponent, _fromValue, _toValue, _aniamtionDuration);
            }
            else
            if (_isStringAnimation)
            {
                StringAnimation(_textComponent, _textTo, _aniamtionDuration);
            }
        }
    }

    public static void IntNumberAnimation(Text textComponent, float from, float to, float duration)
    {
        textComponent.text = from.ToString();
        DOVirtual.Float(from, to, duration, (x) => textComponent.text = Mathf.Floor(x).ToString());
    }
    public static void FloatNumberAnimation(Text textComponent, float from, float to, float duration)
    {
        textComponent.text = from.ToString();
        DOVirtual.Float(from, to, duration, (x) => textComponent.text = Math.Round(x, 2).ToString());
    }

    public static void StringAnimation(Text textComponent, string textTo, float duration)
    {
        textComponent.text = "";
        textComponent.DOText(textTo, duration);
    }
}
