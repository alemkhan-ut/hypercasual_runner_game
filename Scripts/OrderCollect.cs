using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderCollect : MonoBehaviour
{
    [SerializeField] private float _force;

    private void OnEnable()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * _force, ForceMode.Impulse);

        Destroy(gameObject, 5f);
    }
}
