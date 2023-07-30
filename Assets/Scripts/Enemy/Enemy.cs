using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EventBus _eventBus;
    [SerializeField] private EnemyConfig _config;
    [SerializeField] private BulletConfig _bulletConfig;
    [SerializeField] private UnitStateMachine _stateMachine;
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private UnitAnimations _animations;

    private void Awake()
    {
        _healthSystem.SetMaxHealth(_config.MaxHealth);
        _healthSystem.DeathEvent += OnDeath;
        _stateMachine.Config = _config;
        _stateMachine.BulletConfig = _bulletConfig;
        _stateMachine.SetAnimations(_animations);
        _stateMachine.SetNavMeshAgent(_agent);
        _agent.speed = _config.MoveSpeed;
    }

    private void OnDestroy()
    {
        _healthSystem.DeathEvent -= OnDeath;
    }

    public void SetTarget(Transform target)
    {
        _stateMachine.SetTarget(target);
    }

    private void OnDeath() 
    {
        _eventBus.Invoke(new EnemyDeathSignal(this));
    }
}
