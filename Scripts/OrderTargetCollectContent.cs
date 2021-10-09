using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class OrderTargetCollectContent : MonoBehaviour
{
    public static OrderTargetCollectContent instance;

    [SerializeField] private List<OrderTargetCollect> _orderTargetCollects;
    private bool _isPickUpDelay;
    [SerializeField] private float _isPickUpDelayDuration;
    [SerializeField] private float _isPickUpAnimationDuration;
    [SerializeField] private GameObject _orderTargetCollectEffectImagePrefab;

    public List<OrderTargetCollect> OrderTargetCollects { get => _orderTargetCollects; }

    private void Awake()
    {
        instance = this;
    }

    public void AddOrderTargetCollect(OrderTargetCollect orderTargetCollect)
    {
        OrderTargetCollects.Add(orderTargetCollect);
    }

    public void PickUpOrderTarget(CollectableFood collectableFood, GameObject pickUpOwner)
    {
        if (!_isPickUpDelay)
        {
            StartCoroutine(PickUpDelay());

            StartCoroutine(PickUpOrderTargetEffect(collectableFood, pickUpOwner));


            #region OLD_VERSION
            //StartCoroutine(PickUpDelay());

            //OrderTargetCollect orderTargetCollect = FindOrderTargetCollect(collectableFood.OrderTargetType);

            //if (orderTargetCollect != null)
            //{
            //    if (orderTargetCollect.GetAmount() > 0)
            //    {
            //        StartCoroutine(PickUpOrderTargetEffect(orderTargetCollect));
            //    }
            //    else
            //    {
            //        orderTargetCollect.CollectComplete();
            //    }
            //}
            //else
            //{
            //    StartCoroutine(UIOptions.instance.NegativeEffect());
            //}
            #endregion
        }
    }
    private IEnumerator PickUpDelay()
    {
        _isPickUpDelay = true;

        yield return new WaitForSeconds(_isPickUpDelayDuration);

        _isPickUpDelay = false;
    }

    private IEnumerator PickUpOrderTargetEffect(CollectableFood collectableFood, GameObject pickUpOwner)
    {
        yield return collectableFood.transform.DOMove(transform.position - (-transform.right * 2), _isPickUpAnimationDuration).WaitForCompletion();

        if (pickUpOwner.GetComponent<Player>())
        {
            yield return collectableFood.transform.DOMove(pickUpOwner.GetComponent<Player>().BagBox.transform.position, _isPickUpAnimationDuration).WaitForCompletion();
        }

        if (pickUpOwner.GetComponent<Opponent>())
        {
            yield return collectableFood.transform.DOMove(pickUpOwner.GetComponent<Opponent>().BagBox.transform.position, _isPickUpAnimationDuration).WaitForCompletion();
        }

        Destroy(collectableFood.gameObject);
    }

    public OrderTargetCollect FindOrderTargetCollect(GameData.OrderTargetType orderTargetType)
    {
        for (int i = 0; i < OrderTargetCollects.Count; i++)
        {
            if (OrderTargetCollects[i].OrderTargetType == orderTargetType)
            {
                return OrderTargetCollects[i];
            }
        }

        return null;
    }
}
