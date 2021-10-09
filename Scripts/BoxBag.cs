using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoxBag : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private float _filledDuration;
    [SerializeField] private TMP_Text _content_amount_TMP_Text;
    [SerializeField] private GameObject _ownerObject;


    private void Awake()
    {
        _transform = transform;
    }

    private void Start()
    {
        if (IsPlayerCheck())
        {
            _content_amount_TMP_Text.text = Player.instance.BoxBagContentAmount.ToString();
        }
        else
        {
            //_content_amount_TMP_Text.text = _ownerObject.GetComponent<Opponent>().BoxBagContentAmount.ToString();
            _content_amount_TMP_Text.text = Random.Range(0, 6).ToString();
        }
    }

    private bool IsPlayerCheck()
    {
        if (_ownerObject.GetComponent<Player>())
        {
            return true;
        }
        else if (_ownerObject.GetComponent<Opponent>())
        {
            return false;
        }
        else
        {
            return false;
        }
    }

    public void SetOwner(GameObject ownerObject)
    {
        _ownerObject = ownerObject;
    }

    public void BoxBagSet(int value = 1)
    {
        if (IsPlayerCheck())
        {
            Player.instance.BoxBagContentAmount += value;
            _content_amount_TMP_Text.text = Player.instance.BoxBagContentAmount.ToString();
        }
        else
        {
            _ownerObject.GetComponent<Opponent>().BoxBagContentAmount += value;
            _content_amount_TMP_Text.text = _ownerObject.GetComponent<Opponent>().BoxBagContentAmount.ToString();
        }

        StartCoroutine(BoxBagSetCoroutine());
    }

    private IEnumerator BoxBagSetCoroutine()
    {
        yield return _transform.DOScale(_transform.localScale * 1.3f, _filledDuration).WaitForCompletion();
        yield return _transform.DOScale(_transform.localScale / 1.3f, _filledDuration);
    }
}
