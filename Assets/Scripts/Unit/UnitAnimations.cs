using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimations : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private int _move = Animator.StringToHash("Move");
    private int _idle = Animator.StringToHash("Idle");
    private int _attack = Animator.StringToHash("Attack");

    public void TriggerMove() { _animator.SetTrigger(_move); }
    public void TriggerIdle() { _animator.SetTrigger(_idle); }
    public void TriggerAttack() { _animator.SetTrigger(_attack); }

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
