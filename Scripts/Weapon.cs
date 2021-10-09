using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private bool _isReady;
    public bool IsReady { get => _isReady; set => _isReady = value; }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Opponent>() && _isReady)
        {
            Debug.Log("Ударил");

            collision.gameObject.GetComponent<Opponent>().BagBox.GetComponent<BoxBag>().BoxBagSet(-1);
            _isReady = false;
        }
    }
}
