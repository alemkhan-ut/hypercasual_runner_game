using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    [SerializeField] private int _batteryCapacity;
    [SerializeField] private int _batteryEnergy;
    [SerializeField] private Image _imageFill;

    [SerializeField] private bool _isSetEnergyDelay;
    [SerializeField] private float _setEnergyDelayDuraton;

    public static Battery instance;

    public int BatteryEnergy { get => _batteryEnergy; set => _batteryEnergy = value; }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        BatteryEnergy = _batteryCapacity;
    }

    public void SetEnergy(int value)
    {
        if (!_isSetEnergyDelay)
        {
            StartCoroutine(SetEnergyDelay());

            if (value > 0)
            {
                _imageFill.fillAmount -= ((float)value / (float)_batteryCapacity);
                BatteryEnergy += Mathf.Abs(value);
            }
            else
            {
                _imageFill.fillAmount += ((float)value / (float)_batteryCapacity);
                BatteryEnergy -= Mathf.Abs(value);
            }


            Debug.Log("Изменения Энергии на " + ((float)value / (float)_batteryCapacity) + ". Текущее значение бара: " + _imageFill.fillAmount + ". Текущее значение батереи: " + BatteryEnergy);
        }
    }

    public int GetEnergy()
    {
        return BatteryEnergy;
    }

    private IEnumerator SetEnergyDelay()
    {
        _isSetEnergyDelay = true;
        yield return new WaitForSeconds(_setEnergyDelayDuraton);
        _isSetEnergyDelay = false;
    }
}
