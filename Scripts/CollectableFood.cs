using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Animator))]
public class CollectableFood : MonoBehaviour
{
    [SerializeField] private GameData.OrderTargetType _orderTargetType;
    private Transform _transform;
    private Vector3 _startLocalPosition;

    [Range(0.1f, 1f)] [SerializeField] private float _flyPower;
    [Range(0.1f, 1f)] [SerializeField] private float _flyDuration;

    [Range(0.1f, 1f)] [SerializeField] private float _rotateDuration;

    [SerializeField] private GameObject _pickUpOwner;
    [SerializeField] private GameObject _pickUpTarget;
    [SerializeField] private bool _isPickUped;
    [SerializeField] private float _pickUpAnimateDuration;

    public GameData.OrderTargetType OrderTargetType { get => _orderTargetType; }

    void Start()
    {
        _transform = GetComponent<Transform>();
        _startLocalPosition = _transform.localPosition;
    }

    private IEnumerator foodFlyAnimationCoroutine() // TO DO
    {
        yield return new WaitForSeconds(0.5f);

        yield return _transform.DOLocalMoveY(_startLocalPosition.y + _flyPower, _flyDuration).WaitForCompletion();
        yield return _transform.DOLocalMoveY(_startLocalPosition.y, _flyDuration).WaitForCompletion();
        yield return _transform.DOLocalMoveY(_startLocalPosition.y - _flyPower, _flyDuration).WaitForCompletion();
        yield return _transform.DOLocalMoveY(_startLocalPosition.y, _flyDuration).WaitForCompletion();

        StartCoroutine(foodFlyAnimationCoroutine());
    }

    public void PickUp(GameObject owner)
    {
        GetComponent<Animator>().enabled = false;

        _pickUpOwner = owner;

        StartCoroutine(PickUpDelay());
    }

    private IEnumerator PickUpDelay()
    {
        yield return _transform.DOMove(_transform.position + (Vector3.up * 4), 0.2f).WaitForCompletion();

        _isPickUped = true;
    }

    private void Update()
    {

        if (_pickUpOwner != null)
        {
            if (_pickUpOwner.GetComponent<Player>())
            {
                _pickUpTarget = _pickUpOwner.GetComponent<Player>().BagBox.gameObject;
                Debug.Log("Установлен таргет для обьекта" + _pickUpTarget.transform.position);
            }
            else
            {
                _pickUpTarget = _pickUpOwner.GetComponent<Opponent>().BagBox.gameObject;
                Debug.Log("Установлен таргет для обьекта" + _pickUpTarget.transform.position);
            }
        }

        if (_isPickUped)
        {
            Debug.Log("Объект подбирается");
            transform.position = Vector3.Lerp(transform.position, _pickUpTarget.transform.position, _pickUpAnimateDuration * Time.deltaTime);

            if (Vector3.Distance(transform.position, _pickUpTarget.transform.position) <= 2)
            {
                _pickUpTarget.GetComponent<BoxBag>().BoxBagSet();

                Destroy(gameObject);
            }
        }
    }
}
