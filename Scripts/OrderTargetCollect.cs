using UnityEngine;
using UnityEngine.UI;

public class OrderTargetCollect : MonoBehaviour
{
    [SerializeField] private Image _targetImage;
    [SerializeField] private GameData.OrderTargetType _orderTargetType;
    [SerializeField] private Text _targetAmountText;
    [SerializeField] private int _targetAmount;
    [Space]
    [SerializeField] private GameObject _targetComplete;

    public GameData.OrderTargetType OrderTargetType { get => _orderTargetType; }

    private void Awake()
    {
        CollectStart();
    }

    public void SetType(GameData.OrderTargetType orderTargetType)
    {
        _orderTargetType = orderTargetType;
        SetImage();
    }
    private void SetImage()
    {
        _targetImage.sprite = GameData.instance.GetOrderTargetSprite(OrderTargetType);
    }

    public void SetAmount(int amount)
    {
        _targetAmount += amount;
        RefreshText();

        if (_targetAmount <= 0)
        {
            CollectComplete();
        }
    }

    public void CollectStart()
    {
        _targetComplete.SetActive(false);
    }
    public void CollectComplete()
    {
        _targetComplete.SetActive(true);
    }

    public int GetAmount()
    {
        return _targetAmount;
    }

    private void RefreshText()
    {
        _targetAmountText.text = _targetAmount.ToString();
    }
}
