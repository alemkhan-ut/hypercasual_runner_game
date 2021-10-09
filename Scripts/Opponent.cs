using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

#region NAVMESH VERSION
/*
 * [RequiredComponent(NavMeshAgent)]
public class Opponent : MonoBehaviour
{

    [SerializeField] private Player _player;
    [Space]
    [SerializeField] private TMP_Text _name;
    [SerializeField] private string[] _nameArrays;
    [Space]
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _finishTarget;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _bagBox;
    [SerializeField] private int _boxBagContentAmount;

    [SerializeField] private float _nearestDistance;
    [SerializeField] private float _averageDistance;
    [SerializeField] private float _farDistance;

    [SerializeField] private float _regularMoveSpeed;
    [SerializeField] private float _slowMoveSpeed;

    public GameObject BagBox { get => _bagBox; }
    public int BoxBagContentAmount { get => _boxBagContentAmount; set => _boxBagContentAmount = value; }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _bagBox.GetComponent<BoxBag>().SetOwner(gameObject);

        _name.text = _nameArrays[Random.Range(0, _nameArrays.Length)];
    }

    private void Start()
    {
        _animator.SetBool(GameData.ANIMATOR_RUN_TRIGGER, true);

        _navMeshAgent.SetDestination(_finishTarget.position);

        StartCoroutine(StartNavMeshSpeedCoroutine());
    }

    private IEnumerator StartNavMeshSpeedCoroutine()
    {
        if (GameOptions.instance.GetGamePlayStatus())
        {
            _navMeshAgent.isStopped = false;
        }
        else
        {
            _navMeshAgent.isStopped = true;
        }

        yield return new WaitForSeconds(1f);

        if (Vector3.Distance(transform.position, _player.transform.position) > _farDistance) // 100
        {
            _navMeshAgent.speed = _slowMoveSpeed;
        }
        else if (Vector3.Distance(transform.position, _player.transform.position) <= _nearestDistance) // 5
        {
            _navMeshAgent.speed = _regularMoveSpeed + _regularMoveSpeed / 2;
        }
        else if (Vector3.Distance(transform.position, _player.transform.position) >= _averageDistance) // 50
        {
            _navMeshAgent.speed = _regularMoveSpeed;
        }

        StartCoroutine(StartNavMeshSpeedCoroutine());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Finish>())
        {
            _navMeshAgent.SetDestination(collision.gameObject.GetComponent<Finish>().AfterFinishTranform.position);
        }
    }
}
*/
#endregion

public class Opponent : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private Transform _transform;

    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _skins;
    [SerializeField] private GameObject _bagBox;
    [SerializeField] private int _boxBagContentAmount;

    [Header("Speed Properties")]
    [SerializeField] private float _maxVelocitySpeed;
    [SerializeField] private float _basicVelocitySpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _basicMoveSpeed;

    [Header("Jump Properties")]
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _defaultAirJumpAmount = 2f;

    [Header("Gravity Properties")]
    [SerializeField] private float _gravityCheckRayDistance = 1.15f;
    [SerializeField] private bool _isGrounded;

    [SerializeField] private bool _isRoofZone = true;

    [SerializeField] private int _lineNumber;
    [SerializeField] private int _linesAmount = 3;

    [SerializeField] private float firstLinePosition_; // Позиция нулевой линии
    [SerializeField] private float lineDistance_; // Расстояние между линиями
    [SerializeField] private float sideSpeed_; // Скорость между линиями

    [SerializeField] private OpponentMoveTrigger[] _opponentMoveTrigger;

    public GameObject Skins { get => _skins; }
    public float MaxVelocitySpeed { get => _maxVelocitySpeed; set => _maxVelocitySpeed = value; }
    public Animator Animator { get => _animator; set => _animator = value; }
    public int LineNumber { get => _lineNumber; set => _lineNumber = value; }
    public int BoxBagContentAmount { get => _boxBagContentAmount; set => _boxBagContentAmount = value; }
    public GameObject BagBox { get => _bagBox; set => _bagBox = value; }

    public static PlayerMover instance;

    public enum AnimationType
    {
        Run,
        Jump,
        Slide,
        Defeat,
        Victory
    }

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();

        _basicMoveSpeed = _moveSpeed;
        _basicVelocitySpeed = MaxVelocitySpeed;

    }

    private void Start()
    {
        _lineNumber = GameOptions.instance.GetLineNumber(transform.position);
    }

    void Update()
    {
        IsGrounded();

        if (GameOptions.instance.GetGamePlayStatus())
        {
            _rigidBody.useGravity = true;

            _rigidBody.velocity = _rigidBody.velocity.normalized * MaxVelocitySpeed;

            CheckInput();

            Vector3 newPosition = _transform.position;
            newPosition.x = Mathf.Lerp(newPosition.x, firstLinePosition_ + (_lineNumber * lineDistance_), Time.fixedDeltaTime * sideSpeed_);
            _transform.position = newPosition;
        }
        else
        {
            _rigidBody.useGravity = false;

            _rigidBody.velocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (GameOptions.instance.GetGamePlayStatus())
        {
            _rigidBody.AddForce(_transform.forward * _moveSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);
        }
    }

    private void IsGrounded()
    {
        if (GameOptions.instance.GetGamePlayStatus())
        {
            if (Physics.Raycast(_transform.position + Vector3.up, Vector3.down, _gravityCheckRayDistance))
            {
                Animator.ResetTrigger(GameData.ANIMATOR_JUMP_TRIGGER);
                Animator.ResetTrigger(GameData.ANIMATOR_FALL_TRIGGER);

                _isGrounded = true;

                Animator.SetBool(GameData.ANIMATOR_RUN_TRIGGER, true);

                //if (IsStartZone)
                //{
                //    //PlayerAnimator.SetBool(GameData.ANIMATOR_RUN_TRIGGER, true);
                //    MaxVelocitySpeed = 30;
                //}
                //else
                //{
                //    //PlayerAnimator.SetBool(GameData.ANIMATOR_RUN_TRIGGER, false);
                //    //SetTriggerAnimation(AnimationType.Slide);
                //    MaxVelocitySpeed = _basicVelocitySpeed;
                //}
            }
            else
            {
                Animator.SetBool(GameData.ANIMATOR_RUN_TRIGGER, false);

                _isGrounded = false;

                Animator.SetTrigger(GameData.ANIMATOR_FALL_TRIGGER);
            }
        }
        else
        {
            ResetMoveAnimationTriggers();
        }
    }

    private void CheckInput()
    {
        int sign = 0;

        // TO DO: ДОБАВИТЬ ФУНКЦИЮ ПОВОРОТА ВЛЕВО

        // sign = -1;

        // TO DO: ДОБАВИТЬ ФУНКЦИЮ ПОВОРОТА ВПРАВО

        // sign = 1;

        _lineNumber += sign;
        _lineNumber = Mathf.Clamp(_lineNumber, 0, _linesAmount); // Ограничение по количеству
    }
    public void SetTriggerAnimation(AnimationType animationType)
    {
        ResetMoveAnimationTriggers();

        switch (animationType)
        {
            case AnimationType.Run:
                Animator.SetTrigger(GameData.ANIMATOR_RUN_TRIGGER);
                break;
            case AnimationType.Jump:
                Animator.SetTrigger(GameData.ANIMATOR_JUMP_TRIGGER);
                break;
            case AnimationType.Slide:
                Animator.SetTrigger(GameData.ANIMATOR_SLIDE_TRIGGER);
                break;
            case AnimationType.Defeat:
                Animator.SetTrigger(GameData.ANIMATOR_DEFEAT_TRIGGER);
                break;
            case AnimationType.Victory:
                Animator.SetTrigger(GameData.ANIMATOR_VICTORY_TRIGGER);
                break;
            default:
                break;
        }
    }

    public void ResetMoveAnimationTriggers()
    {
        Animator.ResetTrigger(GameData.ANIMATOR_RUN_TRIGGER);
        Animator.ResetTrigger(GameData.ANIMATOR_JUMP_TRIGGER);
        Animator.ResetTrigger(GameData.ANIMATOR_SLIDE_TRIGGER);
    }
}

