using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] private bool _isFollowed;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Vector3 _cameraOffset = new Vector3(0, 3.5f, -8);
    [SerializeField] private bool _cameraDefaultPosition;

    [SerializeField] private bool[] _cameraPositionVariants;
    [SerializeField] private Vector3[] _cameraPositions;

    public static PlayerCamera instance;

    private void Awake()
    {
        instance = this;

        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        if (_cameraDefaultPosition && _isFollowed)
        {
            _mainCamera.transform.position = _transform.position + _cameraOffset;
        }

        for (int i = 0; i < _cameraPositionVariants.Length; i++)
        {
            if (_cameraPositionVariants[i])
            {
                _mainCamera.transform.position = _cameraPositions[i];
                return;
            }
        }
    }

    public void SetFollow(bool status)
    {
        _isFollowed = status;
    }
}
