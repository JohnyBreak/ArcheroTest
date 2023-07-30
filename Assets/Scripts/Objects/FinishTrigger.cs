using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FinishTrigger : MonoBehaviour
{
    //public Action FinishTriggeredEvent;
    [SerializeField] private EventBus _eventBus;
    [SerializeField] private LayerMask _playerLayer;
    private Renderer _renderer;
    private Collider _collider;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
        _renderer.enabled = false;
        _collider.enabled = false;
        _eventBus.Subscribe<SetupLevelSignal>(DisableTrigger);
        _eventBus.Subscribe<AllEnemyDeadSignal>(EnableTrigger);
    }
    
    private void OnDestroy()
    {
        _eventBus.Unsubscribe<SetupLevelSignal>(DisableTrigger);
        _eventBus.Unsubscribe<AllEnemyDeadSignal>(EnableTrigger);
    }

    private void DisableTrigger(SetupLevelSignal signal)
    {
        _renderer.enabled = false;
        _collider.enabled = false;
    }
    private void EnableTrigger(AllEnemyDeadSignal signal) 
    {
        _renderer.enabled = true;
        _collider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & _playerLayer) == 0) return;
        _eventBus.Invoke(new FinishTriggerSignal());
    }
}
