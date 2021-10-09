using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentMoveTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _targetObject;
    [SerializeField] private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material.color = Color.gray;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            _targetObject = other.gameObject;
            _meshRenderer.material.color = Color.green;
        }
        if (other.gameObject.GetComponent<Opponent>())
        {
            _targetObject = other.gameObject;
            _meshRenderer.material.color = Color.black;
        }
        else
        if (other.gameObject.GetComponent<Obstacle>() && !other.gameObject.GetComponent<Obstacle>().IsBorder)
        {
            _targetObject = other.gameObject;
            _meshRenderer.material.color = Color.red;
        }
        else
        if (other.gameObject.GetComponent<PlatformCollider>())
        {
            _targetObject = other.gameObject;
            _meshRenderer.material.color = Color.blue;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _targetObject = null;
        _meshRenderer.material.color = Color.gray;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            _targetObject = other.gameObject;
            _meshRenderer.material.color = Color.green;
        }
        else
        if (other.gameObject.GetComponent<Obstacle>() && !other.gameObject.GetComponent<Obstacle>().IsBorder)
        {
            _targetObject = other.gameObject;
            _meshRenderer.material.color = Color.red;
        }
        else
        if (other.gameObject.GetComponent<PlatformCollider>())
        {
            _targetObject = other.gameObject;
            _meshRenderer.material.color = Color.blue;
        }
    }
}
