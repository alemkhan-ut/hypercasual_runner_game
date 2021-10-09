using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastTest : MonoBehaviour
{
    [SerializeField] private bool[] _bools;

    float NPCDecision = 0;

    private void Start()
    {
        Sleep();
    }

    private void OnMouseUp()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Input.mousePosition);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
        }
    }
    private void NPCThink()
    {
        if (NPCDecision == 1)
        {
            Debug.Log("Принял решение двигаться");

        }
        if (NPCDecision == 0)
        {
            Debug.Log("Принял решение отдохнуть");
        }
    }

    private void Sleep()
    {
        Invoke("NPCThink", 5f);
    }
}
