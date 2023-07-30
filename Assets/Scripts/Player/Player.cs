using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Player : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private UnitConfig _config;
    [SerializeField] private BulletConfig _bulletConfig;
    [SerializeField] private PlayerMobileInput _playerInput;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private HealthSystem _healthSystem;
    [SerializeField] private PlayerStateMachine _playerSM;
    [SerializeField]  private EventBus _eventBus;
    [SerializeField] private PlayerAnimation _anim;

    void Awake()
    {
        _eventBus.Subscribe<RestartLevelSignal>(OnRestartLevel);

        _agent.speed = _config.MoveSpeed;
        _healthSystem.SetMaxHealth(_config.MaxHealth);
        _playerSM.BulletConfig = _bulletConfig;
        _playerSM.SetShootDelay(_config.ShootDelay);
        _playerSM.SetAnimations(_anim);
        _playerSM.SetCameraTransform(_camera);
        _healthSystem.DeathEvent += OnPlayerDeath;
    }

    private void OnDestroy()
    {
        _healthSystem.DeathEvent -= OnPlayerDeath;
        _eventBus.Unsubscribe<RestartLevelSignal>(OnRestartLevel);
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.G)) _healthSystem.TakeDamage(9999);
    //}

    private void OnRestartLevel(RestartLevelSignal signal)
    {
        Destroy(gameObject);
    }

    private void OnPlayerDeath() 
    {
        _eventBus.Invoke(new PlayerDeathSignal());
    }

    public void SetCameraTransform(Transform tr) 
    {
        _playerSM.SetCameraTransform(tr);
    }

    public void SetJoystick(Joystick joystick) 
    {
        _playerInput.SetJoystick(joystick);
    }
}
