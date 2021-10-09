using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private float _playerDistance;
    [SerializeField] private float _playerDistanceValue;

    [SerializeField] private Transform _afterFinishTranform;

    public Transform AfterFinishTranform { get => _afterFinishTranform; }
}
