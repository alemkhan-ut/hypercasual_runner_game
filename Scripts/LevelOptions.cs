using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOptions : MonoBehaviour
{
    [SerializeField] private GameObject[] _levelObjects;

    [SerializeField] private int[] _orderLevelAmounts;

    [SerializeField] private int _rewardPerCollect;
    [SerializeField] private float _raitingPerCollect;
    public static LevelOptions instance;

    public int RewardPerCollect { get => _rewardPerCollect; }
    public float RaitingPerCollect { get => _raitingPerCollect; }

    private void Awake()
    {
        instance = this;
    }

    private void HideAllLevels()
    {
        foreach (GameObject levelObject in _levelObjects)
        {
            levelObject.SetActive(false);
        }
    }

    public void ShowLevel(int levelIndex = 0)
    {
        HideAllLevels();
        Debug.Log("Раскрыт первый уровень");
        _levelObjects[levelIndex].SetActive(true);
    }

    public void ShowRandomLevel()
    {
        HideAllLevels();
        int levelIndex = Random.Range(0, _levelObjects.Length);
        Debug.Log("Раскрыт случайный уровень " + levelIndex.ToString());
        _levelObjects[levelIndex].SetActive(true);
    }

    public int[] GetOrderLevelAmount()
    {
        return _orderLevelAmounts;
    }

    private void Start()
    {

    }
}
