using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private bool _isMineControl;
    [SerializeField] private GameObject _bagBox;
    [SerializeField] private Transform _weaponPlaceTransform;
    [SerializeField] private GameObject _attackTargetObject;
    [SerializeField] private int _boxBagContentAmount;
    [SerializeField] private Transform _orderCollectsTransform;
    [SerializeField] private GameObject _orderCollectPrefab;

    [Space]
    [SerializeField] private float _maxRayDistance;
    [SerializeField] private LayerMask _layerMask;
    [Space]

    private PlayerMover _playerMover;
    private CapsuleCollider _capsuleCollider;

    public static Player instance;

    public bool IsMineControl { get => _isMineControl; }
    public GameObject BagBox { get => _bagBox; }
    public GameObject AttackTargetObject { get => _attackTargetObject; set => _attackTargetObject = value; }
    public Transform WeaponPlaceTransform { get => _weaponPlaceTransform; set => _weaponPlaceTransform = value; }
    public int BoxBagContentAmount { get => _boxBagContentAmount; set => _boxBagContentAmount = value; }

    private void Awake()
    {
        _playerMover = GetComponent<PlayerMover>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _photonView = GetComponent<PhotonView>();

        if (_photonView == null)
        {
            _isMineControl = true;
        }
        else
        {
            _isMineControl = _photonView.IsMine;
        }

        instance = this;
    }

    private void Start()
    {

    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.up, out hit, _maxRayDistance))
        {
            Debug.DrawRay(transform.position, Vector3.up * _maxRayDistance, Color.red);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.up * _maxRayDistance, Color.green);
            Debug.Log("Did not Hit");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Finish>())
        {
            GameOptions.instance.LevelComplete();
        }

        if (collision.gameObject.GetComponent<Obstacle>())
        {
            StartCoroutine(UIOptions.instance.NegativeEffect());

            if (collision.gameObject.GetComponent<Obstacle>().IsDestructible)
            {
                CollectLost();
                Destroy(collision.gameObject);
            }
            else
            {
                GameOptions.instance.LoseLevel();
            }
        }

        if (collision.gameObject.GetComponent<Roof>())
        {

        }

        if (collision.gameObject.GetComponent<PlatformCollider>())
        {
            if (collision.gameObject.GetComponent<PlatformCollider>().Platform.IsStartPlatform)
            {
                _playerMover.IsStartZone = true;
            }

            else
            {
                _playerMover.AirJumpAmount = _playerMover.DefaultAirJumpAmount;
                _playerMover.IsStartZone = false;
            }

            if (collision.gameObject.GetComponent<PlatformCollider>().Platform.IsFinalPlatform)
            {
                _playerMover.MaxVelocitySpeed = 100f;
            }
        }

        if (collision.gameObject.GetComponent<PlatformCollider>())
        {
            if (!collision.gameObject.GetComponent<PlatformCollider>().Platform.IsStartPlatform)
            {
                PlayerMover.instance.CurrentPlatform = collision.gameObject.GetComponent<PlatformCollider>().Platform;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlatformCollider>())
        {
            if (!collision.gameObject.GetComponent<PlatformCollider>().Platform.IsStartPlatform)
            {
                PlayerMover.instance.LatestPlatform = collision.gameObject.GetComponent<PlatformCollider>().Platform;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CollectableFood>())
        {
            StartCoroutine(UIOptions.instance.PositiveEffect());

            other.gameObject.GetComponent<CollectableFood>().PickUp(gameObject);
        }

        if (other.gameObject.GetComponent<FallTrigger>())
        {
            GameOptions.instance.LoseLevel();
        }

        if (other.gameObject.GetComponent<CollectableWeapon>())
        {
            Destroy(other.gameObject.GetComponent<CollectableWeapon>().gameObject);
            _weaponPlaceTransform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void SetEnableCollider(bool status)
    {
        _capsuleCollider.enabled = status;
    }

    public void CollectLost()
    {
        Instantiate(_orderCollectPrefab, _orderCollectsTransform);
        UIOptions.instance.CollectLost(-2);
    }
}
