using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    [SerializeField] private GameData.IconSpriteType _spriteType;
    [SerializeField] private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        _image.sprite = GameData.instance.GetIconSprite(_spriteType);
    }
}
