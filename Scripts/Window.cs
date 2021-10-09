using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Window : MonoBehaviour
{
    [SerializeField] private bool isCustomStartPosition_;
    [SerializeField] private float startXPosition_;
    [SerializeField] private float endXPosition_;
    [SerializeField] private float moveDuration_;
    [SerializeField] private bool isAwakeOpen_;

    private Transform transform_;

    private void Start()
    {
        transform_ = GetComponent<Transform>();

        if (isCustomStartPosition_)
        {
            transform.DOLocalMoveX(startXPosition_, 0);
        }

        if (isAwakeOpen_)
        {
            OpenWindow();
        }
    }

    public void OpenWindow()
    {
        transform_.DOLocalMoveX(endXPosition_, moveDuration_).SetEase(Ease.InOutBack);
    }
    public void CloseWindow()
    {
        transform_.DOLocalMoveX(startXPosition_, moveDuration_).SetEase(Ease.InOutBack);
    }
}
