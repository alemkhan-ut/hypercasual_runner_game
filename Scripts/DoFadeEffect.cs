using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoFadeEffect : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Start()
    {
        meshRenderer.material.DOFade(0, 0);

        meshRenderer.material.DOFade(1, Random.Range(5, 10f));
    }
}
