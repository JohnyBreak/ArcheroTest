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
        _stateMachine.SetAnimations(_animations);

        _animations.SetMoveSpeedMultiplier(1 + (_config.MoveSpeed / 100.0f));
        float atkSpeedTemp = 1.0f + (_config.AttackSpeed / 100f);
        float timeBeforeAttack = _config.AnimationSettings.BeforeAttackTime / atkSpeedTemp;
        float timeAfterAttack = _config.AnimationSettings.AfterAttackTime / atkSpeedTemp;
        float totalAttackSpeed = timeBeforeAttack + timeAfterAttack;
        _animations.SetAttackSpeedMultiplier(atkSpeedTemp);
        _stateMachine.SetAttackSpeed(totalAttackSpeed);
        _stateMachine.SetConfigs(_config, _bulletConfig);
        _stateMachine.SetNavMeshAgent(_agent);
        _agent.speed = (1 + ((_config.MoveSpeed / 100.0f) * 2));
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
