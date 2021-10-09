using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderLevel : MonoBehaviour
{
    [SerializeField] private int _orderLevelValue;

    [SerializeField] private Image _orderLevelImage;
    [SerializeField] private Text _orderLevelText;

    [SerializeField] private Color _orderLevelCompleteColor;

    public void OrderLevelCompleted()
    {
        StartCoroutine(OrderLevelCompletedCoroutine());
    }

    public void SetOrderLevelValue(int value)
    {
        _orderLevelValue = value;
        _orderLevelText.text = _orderLevelValue.ToString();
    }

    private IEnumerator OrderLevelCompletedCoroutine()
    {
        yield return _orderLevelImage.transform.DOScale(1.5f, GameData.DEFAULT_DURATION / 2).WaitForCompletion();
        _orderLevelImage.transform.DOScale(1f, GameData.DEFAULT_DURATION / 2);
        _orderLevelImage.DOColor(_orderLevelCompleteColor, GameData.DEFAULT_DURATION / 2);
    }


}
