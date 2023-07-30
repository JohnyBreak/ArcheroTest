
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    #region Fields

    [SerializeField] private PlayerStateFactory.PlayerStates _currentRootStateName;
    [SerializeField] private PlayerStateFactory.PlayerStates _currentSubStateName;

    [SerializeField] private PlayerMobileInput _input;
    [SerializeField] private FieldOfView _fieldOfView;
    //[SerializeField] 
    private Transform _cameraTransform;
    [SerializeField] private float _movementSpeed = 7.5f;

    private float _shootDelay;
    private PlayerMovement _controller;
    private PlayerAnimation _playerAnimation;
    private Vector2 _currentMovementInput;
    private Vector3 _currentMovement;
    private Vector3 _appliedMovement;

    private Vector3 _cameraRelativeMovement;

    private float _turnSmoothTime = 0.03f;

    private bool _isMovementPressed;

    //state
    protected PlayerBaseState _currentState;
    protected PlayerStateFactory _states;
    
#endregion

    #region Properties
    // getters & setters
    public PlayerAnimation PlayerAnimation => _playerAnimation;
    public float CurrentMovementY { get { return _currentMovement.y; } set { _currentMovement.y = value; } }
    public float AppliedMovementY { get { return _appliedMovement.y; } set { _appliedMovement.y = value; } }
    public float AppliedMovementZ { get { return _appliedMovement.z; } set { _appliedMovement.z = value; } }
    public float AppliedMovementX { get { return _appliedMovement.x; } set { _appliedMovement.x = value; } }
    public Vector3 CurrentMovement { get { return _currentMovement; } set { _currentMovement = value; } }

    public float MagnitudedMovement => GetMagnitudedMoveVectorForAnimation();
    public float MovementSpeed => _movementSpeed;
    public Transform CameraTransform => _cameraTransform;
    public float TurnSmoothTime => _turnSmoothTime;
    public bool IsMovementPressed => _isMovementPressed;

    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    public Vector3 CameraRelativeMovement => _cameraRelativeMovement;
    public FieldOfView FieldOfView => _fieldOfView;
    public float ShootDelay => _shootDelay;
    public CancellationTokenSource ShootCancelToken;
    [HideInInspector] public BulletConfig BulletConfig;

    #endregion

    public void SetShootDelay(float delay) 
    {
        if (delay < 0.1f) delay = 0.1f;

        _shootDelay = delay;
    }

    private void Awake()
    {
        _controller = GetComponent<PlayerMovement>();
        
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.EnterState();

    }

    void Update()
    {
        if (GameStateManager.CurrentGameState != GameStateManager.GameState.GamePlay) return;

        _currentMovement.x = _input.GetNormalizedMoveInput().x;
        _currentMovement.z = _input.GetNormalizedMoveInput().y;
        _isMovementPressed = _input.GetNormalizedMoveInput() != Vector2.zero;


        _cameraRelativeMovement = GetCameraRelativeMoveDirection();

        _currentState.UpdateStates();
        _currentRootStateName = _states.RootState;
        _currentSubStateName = _states.SubState;
        _playerAnimation.SetMovementAnimation(GetMagnitudedMoveVectorForAnimation());
        _controller.SimpleMove(_cameraRelativeMovement);
    }

    private float GetMagnitudedMoveVectorForAnimation() 
    {
        Vector3 animMoveVectorMagnitude = _currentMovement;
        animMoveVectorMagnitude.y = 0;
        return animMoveVectorMagnitude.magnitude;
    }

    private void OnDisable()
    {
        if(ShootCancelToken != null) ShootCancelToken.Cancel();
    }

    public void SetCameraTransform(Transform tr) 
    {
        _cameraTransform = tr;
    }

    public void SetAnimations(PlayerAnimation anim) 
    {
        _playerAnimation = anim;
    }

    private Vector3 GetCameraRelativeMoveDirection() 
    {
        Vector3 cameraForward = _cameraTransform.forward;
        Vector3 cameraRight = _cameraTransform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;

        Vector3 forwardCameraRelativeMovement = cameraForward * _currentMovement.z;
        Vector3 rightCameraRelativeMovement = cameraRight * _currentMovement.x;
        Vector3 cameraRelativeMovement = forwardCameraRelativeMovement + rightCameraRelativeMovement;

        Debug.DrawRay(transform.position, cameraRelativeMovement, Color.black);
        return cameraRelativeMovement.normalized;
    }
}
