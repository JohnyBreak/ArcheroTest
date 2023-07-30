using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public enum Stance { None, Idle, Walk, Run }
    [SerializeField] private Animator _animator;
    [SerializeField] private float _smoothBlend = 0.1f;

    private Coroutine _movementSpeedRoutine;


    private int _movementSpeed = Animator.StringToHash("MovementSpeed");
    private int _isAttacking = Animator.StringToHash("IsAttacking");

    public Animator Animator => _animator;

    public void SetMovementAnimation(float currentMovement)
    {
        _animator.SetFloat(_movementSpeed, currentMovement, _smoothBlend, Time.deltaTime);
    }

    public void SetStance(Stance stance)
    {
        switch (stance)
        {
            case Stance.Idle:

                StartMovementCoroutine(0f);
                //SetMovementSpeed(0);
                break;
            case Stance.Walk:

                StartMovementCoroutine(.5f);
                //SetMovementSpeed(0.5f);
                break;
            case Stance.Run:
                StartMovementCoroutine(1f);
                //SetMovementSpeed(1);
                //_animator.SetFloat("MovementSpeed", 1f, _smoothBlend, Time.deltaTime);
                break;
        }
    }

    private void StartMovementCoroutine(float value)
    {
        if (_movementSpeedRoutine != null)
        {
            StopCoroutine(_movementSpeedRoutine);
            _movementSpeedRoutine = null;
        }
        _movementSpeedRoutine = StartCoroutine(SetMovementSpeed(value));
    }

    private IEnumerator SetMovementSpeed(float value)
    {
        while (_animator.GetFloat(_movementSpeed) < value - 0.05f || _animator.GetFloat(_movementSpeed) > value + 0.05f)
        {
            yield return null;
            _animator.SetFloat(_movementSpeed, value, _smoothBlend, Time.deltaTime);
        }
        _animator.SetFloat(_movementSpeed, value);
    }

    public void ToggleAttack(bool value)
    {
        _animator.SetBool(_isAttacking, value);
    }

    private void Awake()
    {
        GameStateManager.GameStateChangedEvent += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.GameStateChangedEvent -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateManager.GameState state)
    {
        if (state != GameStateManager.GameState.GamePlay)
            _animator.speed = 0;
        else
            _animator.speed = 1;
    }
}
