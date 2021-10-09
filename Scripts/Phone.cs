using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    public static Phone instance;

    private Window _window;

    public Window Window { get => _window; }

    private void Awake()
    {
        instance = this;

        _window = GetComponent<Window>();
    }
}
