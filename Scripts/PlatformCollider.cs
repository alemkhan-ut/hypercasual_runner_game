using UnityEngine;

public class PlatformCollider : MonoBehaviour
{
    [SerializeField] private Platform _platform;

    public Platform Platform { get => _platform; }
}
