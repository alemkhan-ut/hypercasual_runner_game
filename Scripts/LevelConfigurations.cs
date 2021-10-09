using System.Collections.Generic;
using UnityEngine;

public class LevelConfigurations : MonoBehaviour
{
    [SerializeField] private GameObject[] _platforms;
    [Space]
    [SerializeField] private List<OrderTargetCollect> _orderTargetCollects;

    public static LevelConfigurations instance;

    private void Awake()
    {
        instance = this;
    }
}
