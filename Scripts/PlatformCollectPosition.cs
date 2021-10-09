using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformCollectPosition : MonoBehaviour
{
    [SerializeField] private bool _isBusy;

    public bool IsBusy { get => _isBusy; set => _isBusy = value; }
}
