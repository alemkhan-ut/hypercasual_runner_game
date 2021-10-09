using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectFoodProgressBar : MonoBehaviour
{
    [SerializeField] private Image _progressBarFillImage;
    [SerializeField] private int[] _orderLevelAmounts;
    [SerializeField] private OrderLevels _orderLevels;
    [Space]
    [SerializeField] private GameObject _orderLevelPrefab;

    public void SetLevelAmounts(int[] values)
    {
        _orderLevelAmounts = values;

        if (_orderLevels.transform.childCount > 0)
        {
            for (int i = 0; i < _orderLevels.transform.childCount; i++)
            {
                Destroy(_orderLevels.transform.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < _orderLevelAmounts.Length; i++)
        {
            OrderLevel orderLevel = Instantiate(_orderLevelPrefab, _orderLevels.transform).GetComponent<OrderLevel>();
            orderLevel.SetOrderLevelValue(_orderLevelAmounts[i]);
        }
    }

    public void SetFill(int value)
    {
        StartCoroutine(SetFillCoroutine(value));
    }

    private IEnumerator SetFillCoroutine(int value)
    {
        float endValue;
        float maxValue = _orderLevelAmounts[_orderLevelAmounts.Length - 1];

        if (value > (_orderLevelAmounts[_orderLevelAmounts.Length - 1]))
        {
            endValue = _orderLevelAmounts[_orderLevelAmounts.Length - 1] / maxValue;
        }
        else
        {
            endValue = value / maxValue;
        }

        yield return _progressBarFillImage.DOFillAmount(endValue, GameData.DEFAULT_DURATION).WaitForCompletion();

        for (int i = 0; i < _orderLevelAmounts.Length; i++)
        {
            if (_progressBarFillImage.fillAmount >= _orderLevelAmounts[i] / maxValue)
            {
                _orderLevels.transform.GetChild(i).GetComponent<OrderLevel>().OrderLevelCompleted();
            }

            yield return new WaitForSeconds(GameData.DEFAULT_DURATION);
        }
    }

}
