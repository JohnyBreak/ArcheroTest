using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private int _move = Animator.StringToHash("IsMove");
    private int _idle = Animator.StringToHash("IsIdle");
    private int _attack = Animator.StringToHash("IsAttack");
    private int _movementSpeedMultiplier = Animator.StringToHash("MoveSpeedMultiplier");
    private int _attackSpeedMultiplier = Animator.StringToHash("AttackSpeedMultiplier");

    public void SetMoveSpeedMultiplier(float speed)
    {
        _animator.SetFloat(_movementSpeedMultiplier, speed);
    }

    public void SetAttackSpeedMultiplier(float speed)
    {
        _animator.SetFloat(_attackSpeedMultiplier, speed);
    }

    public void TriggerMove() 
    {
        _animator.SetBool(_attack, false);
        _animator.SetBool(_idle, false);
        _animator.SetBool(_move, true); 
    }
    public void TriggerIdle() 
    {
        _animator.SetBool(_attack, false);
        _animator.SetBool(_move, false);
        _animator.SetBool(_idle, true);
    }

    public void TriggerAttack() 
    {
        _animator.SetBool(_move, false);
        _animator.SetBool(_idle, true);
        _animator.SetBool(_attack, true);
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
