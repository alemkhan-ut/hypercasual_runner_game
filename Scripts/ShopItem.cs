using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{

    public enum ItemType
    {
        FIRST_SKIN,
        SECOND_SKIN
    }

    [SerializeField] private ItemType type_;
    [SerializeField] private Button buyButton_;
    [SerializeField] private Button activateButton_;
    [SerializeField] private bool isBought_;
    [SerializeField] private int cost_;

    private bool isActive_
    {
        get
        {
            return type_ == ShopManager_.ActiveSkinType_;
        }
    }

    private ShopManager shopManager_;
    public ShopManager ShopManager_ { get => shopManager_; set => shopManager_ = value; }


    public void Inititialize()
    {
        ShopManager_ = FindObjectOfType<ShopManager>();
    }

    public void CheckButton()
    {
        buyButton_.gameObject.SetActive(!isBought_);

        activateButton_.gameObject.SetActive(isBought_);
        activateButton_.interactable = !isActive_;
    }

    public void BuyItem()
    {
        isBought_ = true;

        CheckButton();
    }

    public void ActiveItem()
    {
        shopManager_.ActiveSkinType_ = type_;
        shopManager_.CheckItemButtons();

        switch (type_)
        {
            case ItemType.FIRST_SKIN:
                break;
            case ItemType.SECOND_SKIN:
                break;
            default:
                break;
        }

    }
}
