using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    [SerializeField] private GameData.ResourceType _resourceType;
    [SerializeField] private Text _resourceText;
    [SerializeField] private Image _resourceFocusImage;

    public void ResourceFill(float startValue, float endvalue, bool isAdded)
    {
        StartCoroutine(ResourceFillCoroutine(startValue, endvalue, isAdded));
    }

    private IEnumerator ResourceFillCoroutine(float startValue, float value, bool isAdded)
    {
        if (isAdded)
        {
            _resourceFocusImage.color = Color.green;
        }
        else
        {
            _resourceFocusImage.color = Color.red;
        }

        if (_resourceType == GameData.ResourceType.Rating)
        {
            TextAnimation.FloatNumberAnimation(_resourceText, startValue, value, GameData.DEFAULT_DURATION * 2);
        }
        else
        {
            TextAnimation.IntNumberAnimation(_resourceText, startValue, value, GameData.DEFAULT_DURATION * 2);
        }

        _resourceFocusImage.DOFade(0.5f, GameData.DEFAULT_DURATION);

        yield return new WaitForSeconds(GameData.DEFAULT_DURATION * 2);

        _resourceFocusImage.DOFade(0, GameData.DEFAULT_DURATION).WaitForCompletion();
    }
}
