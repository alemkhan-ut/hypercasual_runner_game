using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<ShopItem> items_;
    [SerializeField] private ShopItem.ItemType activeSkinType_;

    public ShopItem.ItemType ActiveSkinType_ { get => activeSkinType_; set => activeSkinType_ = value; }

    public void OpenShop()
    {
        CheckItemButtons();
        gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        gameObject.SetActive(false);
    }

    public void CheckItemButtons()
    {
        foreach (ShopItem item in items_)
        {
            item.ShopManager_ = this;
            item.Inititialize();
            item.CheckButton();
        }
    }
}
