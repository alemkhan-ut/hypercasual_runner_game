using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
    [SerializeField] private PhysicsMovement _physicsMovement;

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis(Axis.Horizontal);
        float vertical = Input.GetAxis(Axis.Vertical);

        _physicsMovement.Move(new Vector3(horizontal, 0, vertical));
    }
}
