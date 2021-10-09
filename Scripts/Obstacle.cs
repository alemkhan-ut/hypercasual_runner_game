using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Obstacle : MonoBehaviour
{
    [SerializeField] private bool _isDestructible;
    [SerializeField] private bool _isBorder;

    public bool IsDestructible { get => _isDestructible; }
    public bool IsBorder { get => _isBorder;    }

    public void Destrusct()
    {

    }
}
