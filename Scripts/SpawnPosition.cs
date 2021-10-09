using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    [SerializeField] private int _lineNumber;

    public int LineNumber { get => _lineNumber; }
}
