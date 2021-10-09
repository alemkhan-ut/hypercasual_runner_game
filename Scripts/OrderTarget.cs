using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderTarget : MonoBehaviour
{
    [SerializeField] private Text _orderTargetAmountText;
    [SerializeField] private Image _orderTargetImage;

    public void SetOrderTargetAmount(int value)
    {
        _orderTargetAmountText.text = value.ToString();
    }

    public void SetOrderTargetSprite(Sprite sprite)
    {
        _orderTargetImage.sprite = sprite;
    }
}
