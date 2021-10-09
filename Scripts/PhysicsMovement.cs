using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private SurfaceSlider _surfaceSlider;
    [SerializeField] private float _speed;
    private Vector3 _normal;

    public void Move(Vector3 direction)
    {
        Vector3 directionALongSurface = Vector3.ProjectOnPlane(direction.normalized, _normal);
        Vector3 offset = directionALongSurface * (_speed * Time.deltaTime);

        _rigidbody.MovePosition(_rigidbody.position + offset);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _normal = collision.contacts[0].normal;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + _normal * 3);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.ProjectOnPlane(transform.forward, _normal) * 3);
    }
}
