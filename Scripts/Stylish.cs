using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Outline))]
public class Stylish : MonoBehaviour
{
    private Outline _outline;

    [SerializeField] private Color _effectColor = Color.black;
    [SerializeField] private Vector2 _effectDistance = new Vector2(2, 2);

    private void Awake()
    {
        _outline = GetComponent<Outline>();

        _outline.effectColor = _effectColor;
        _outline.effectDistance = _effectDistance;
    }
}
