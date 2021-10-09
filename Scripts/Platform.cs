using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Platform : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    [SerializeField] private bool _isStartPlatform;
    [SerializeField] private bool _isFinalPlatform;

    [SerializeField] private float _playerDistance;
    [SerializeField] private float _playerDistanceValue;
    [Space]
    [SerializeField] private GameObject _plaformBase;
    [SerializeField] private GameObject _platformObstacles;
    [SerializeField] private GameObject _platformCollectables;
    [SerializeField] private GameObject[] _platformCollectablePositions;
    [SerializeField] private GameObject[] _platformObstaclePositions;

    [SerializeField] private GameObject _reincarnationPosition;

    [SerializeField] private int _lineNumber;

    public bool IsStartPlatform { get => _isStartPlatform; set => _isStartPlatform = value; }
    public bool IsFinalPlatform { get => _isFinalPlatform; set => _isFinalPlatform = value; }
    public GameObject[] PlatformCollectablePositions { get => _platformCollectablePositions; }
    public GameObject[] PlatformObstaclePositions { get => _platformObstaclePositions; set => _platformObstaclePositions = value; }
    public GameObject ReincarnationPosition { get => _reincarnationPosition; }
    public int LineNumber { get => _lineNumber; }

    private void Start()
    {
        if (!IsStartPlatform)
        {
            StartCoroutine(SetActivePlatform(false));
        }

        _lineNumber = GameOptions.instance.GetLineNumber(transform.position);
    }
    private void Update()
    {
        if (Player.instance != null)
        {
            _playerDistance = Vector3.Distance(transform.position, Player.instance.transform.position);

            if (_playerDistance <= _playerDistanceValue)
            {
                StartCoroutine(SetActivePlatform(true));
            }
        }
    }


    private IEnumerator SetActivePlatform(bool isActive)
    {
        _plaformBase.SetActive(isActive);

        if (isActive)
        {
            yield return _meshRenderer.material.DOFade(1, .5f).WaitForCompletion();
        }
        else
        {
            yield return _meshRenderer.material.DOFade(0, 0.1f).WaitForCompletion();
        }
    }
}
