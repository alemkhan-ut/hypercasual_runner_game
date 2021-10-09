using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMover : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private Transform _transform;
    private PlayerCamera _playerCamera;

    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private GameObject _reincarnationPosition;
    [SerializeField] private float _reincarnationDuration;
    [SerializeField] private GameObject _skins;

    [SerializeField] private Platform _currentPlatform;
    [SerializeField] private Platform _latestPlatform;

    [Header("Speed Properties")]
    [SerializeField] private float _maxVelocitySpeed;
    [SerializeField] private float _basicVelocitySpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _basicMoveSpeed;

    [Header("Jump Properties")]
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _defaultAirJumpAmount = 2f;
    [SerializeField] private float _airJumpAmount;

    [Header("Gravity Properties")]
    [SerializeField] private float _gravityCheckRayDistance = 1.15f;
    [SerializeField] private bool _isGrounded;

    [Header("Markers Properties")]
    [SerializeField] private bool _wannaJump = false;
    [SerializeField] private bool _isJumped = false;


    [SerializeField] private bool _wannaFall = false;
    [SerializeField] private bool _isStartZone = true;
    [SerializeField] private bool _isRoofZone = true;

    [Header("Animation Properties")]
    [SerializeField] private float _startRotateAnimationDuration;

    [SerializeField] private int _lineNumber;
    [SerializeField] private int _linesAmount = 3;

    [SerializeField] private float firstLinePosition_; // Позиция нулевой линии
    [SerializeField] private float lineDistance_; // Расстояние между линиями
    [SerializeField] private float sideSpeed_; // Скорость между линиями

    public bool IsStartZone { get => _isStartZone; set => _isStartZone = value; }
    public float AirJumpAmount { get => _airJumpAmount; set => _airJumpAmount = value; }
    public float DefaultAirJumpAmount { get => _defaultAirJumpAmount; set => _defaultAirJumpAmount = value; }

    public GameObject Skins { get => _skins; }
    public float MaxVelocitySpeed { get => _maxVelocitySpeed; set => _maxVelocitySpeed = value; }
    public bool IsRoofZone { get => _isRoofZone; set => _isRoofZone = value; }
    public Animator PlayerAnimator { get => _playerAnimator; set => _playerAnimator = value; }
    public Platform LatestPlatform { get => _latestPlatform; set => _latestPlatform = value; }
    public Platform CurrentPlatform { get => _currentPlatform; set => _currentPlatform = value; }
    public int LineNumber { get => _lineNumber; set => _lineNumber = value; }

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
        instance = this;

        _rigidBody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _playerCamera = GetComponent<PlayerCamera>();

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

        if (GameOptions.instance.GetGamePlayStatus() && Player.instance.IsMineControl)
        {
            _rigidBody.useGravity = true;

            if ((Input.GetKeyDown(KeyCode.W) ||
                Input.GetKeyDown(KeyCode.UpArrow)) &&
                AirJumpAmount > 0)
            {
                _wannaJump = true;
            }

            if (!_isGrounded)
            {
                if ((Input.GetKeyDown(KeyCode.S) ||
                Input.GetKeyDown(KeyCode.DownArrow)))
                {
                    _wannaFall = true;
                }
            }

            //if (_wannaJump)
            //{
            //    Jump();
            //}
            //if (_wannaFall)
            //{
            //    Fall();
            //}

            //if (_rigidBody.velocity.magnitude >= MaxVelocitySpeed)
            //{
            //    _rigidBody.velocity = _rigidBody.velocity.normalized * MaxVelocitySpeed;
            //}

            //CheckInput();

            //Vector3 newPosition = _transform.position;
            //newPosition.x = Mathf.Lerp(newPosition.x, firstLinePosition_ + (_lineNumber * lineDistance_), Time.fixedDeltaTime * sideSpeed_);
            //_transform.position = newPosition;
        }
        else
        {
            _rigidBody.useGravity = false;

            _rigidBody.velocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        //if (GameOptions.instance.GetGamePlayStatus())
        //{
        //    _rigidBody.AddForce(_transform.forward * _moveSpeed * Time.fixedDeltaTime, ForceMode.Acceleration);
        //}
    }

    private void Jump()
    {
        _isJumped = true;
        SetTriggerAnimation(AnimationType.Jump);

        _rigidBody.velocity = (_rigidBody.velocity.normalized * MaxVelocitySpeed) / 10;

        AirJumpAmount -= 1;
        UIOptions.instance.AirJumpFillChange(1f / DefaultAirJumpAmount);
        _wannaJump = false;

        _rigidBody.AddForce(_transform.up * _jumpPower * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    private void Fall()
    {
        _rigidBody.velocity = (_rigidBody.velocity.normalized * MaxVelocitySpeed) / 2;
        _rigidBody.AddForce(_transform.up * (-_jumpPower * 5) * Time.fixedDeltaTime, ForceMode.VelocityChange);
        _wannaFall = false;
    }

    public void Reincarnation()
    {
        if (_latestPlatform != null)
        {
            _skins.SetActive(true);

            Phone.instance.Window.CloseWindow();

            StartCoroutine(ReincarnationAnimation());
        }
    }

    private IEnumerator ReincarnationAnimation()
    {
        Player.instance.SetEnableCollider(false);

        PlayerAnimator.SetTrigger(GameData.ANIMATOR_JUMP_TRIGGER);

        yield return new WaitForSeconds(.25f);

        _playerCamera.SetFollow(true);

        _lineNumber = _latestPlatform.LineNumber;

        yield return _transform.DOMove(_latestPlatform.ReincarnationPosition.transform.position, _reincarnationDuration).WaitForCompletion();

        yield return new WaitForSeconds(.25f);

        GameOptions.instance.SetGamePlayStatus(true);

        Player.instance.SetEnableCollider(true);
    }

    private void IsGrounded()
    {
        if (GameOptions.instance.GetGamePlayStatus() && Player.instance.IsMineControl)
        {
            if (Physics.Raycast(_transform.position + Vector3.up, Vector3.down, _gravityCheckRayDistance))
            {
                _isJumped = false;
                PlayerAnimator.ResetTrigger(GameData.ANIMATOR_JUMP_TRIGGER);
                PlayerAnimator.ResetTrigger(GameData.ANIMATOR_FALL_TRIGGER);

                UIOptions.instance.AirJumpFillChange(DefaultAirJumpAmount, true);
                _isGrounded = true;

                PlayerAnimator.SetBool(GameData.ANIMATOR_RUN_TRIGGER, true);

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
                PlayerAnimator.SetBool(GameData.ANIMATOR_RUN_TRIGGER, false);

                _isGrounded = false;

                if (!_isJumped)
                {
                    PlayerAnimator.SetTrigger(GameData.ANIMATOR_FALL_TRIGGER);
                }
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

        if (Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.LeftArrow))
        {
            sign = -1;
        }
        else
        if (Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.RightArrow))
        {
            sign = 1;
        }
        else
        {
            return;
        }

        if (!_isGrounded || !IsStartZone)
        {
            _lineNumber += sign;
            _lineNumber = Mathf.Clamp(_lineNumber, 0, _linesAmount); // Ограничение по количеству
        }
    }

    public void SetTriggerAnimation(AnimationType animationType)
    {
        ResetMoveAnimationTriggers();

        switch (animationType)
        {
            case AnimationType.Run:
                PlayerAnimator.SetTrigger(GameData.ANIMATOR_RUN_TRIGGER);
                break;
            case AnimationType.Jump:
                PlayerAnimator.SetTrigger(GameData.ANIMATOR_JUMP_TRIGGER);
                break;
            case AnimationType.Slide:
                PlayerAnimator.SetTrigger(GameData.ANIMATOR_SLIDE_TRIGGER);
                break;
            case AnimationType.Defeat:
                PlayerAnimator.SetTrigger(GameData.ANIMATOR_DEFEAT_TRIGGER);
                break;
            case AnimationType.Victory:
                PlayerAnimator.SetTrigger(GameData.ANIMATOR_VICTORY_TRIGGER);
                break;
            default:
                break;
        }
    }

    public void ResetMoveAnimationTriggers()
    {
        PlayerAnimator.ResetTrigger(GameData.ANIMATOR_RUN_TRIGGER);
        PlayerAnimator.ResetTrigger(GameData.ANIMATOR_JUMP_TRIGGER);
        PlayerAnimator.ResetTrigger(GameData.ANIMATOR_SLIDE_TRIGGER);
    }

    public void StartRotateAnimation(Vector3 value)
    {
        _skins.transform.DORotate(value, _startRotateAnimationDuration);
    }
}
