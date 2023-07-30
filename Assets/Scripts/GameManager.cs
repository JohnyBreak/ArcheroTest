using Cinemachine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Action StartLevelEvent;

    [Header("Camera")]
    [SerializeField] private Camera _mainCam;
    [SerializeField] private CinemachineVirtualCamera _virtualCam;
    [SerializeField] private CameraFitter _cameraFitter;
     
    [Header("Levels")]
    [SerializeField] private Transform _groundTransform;
    [SerializeField] private List<LevelInfo> _levelInfos;

    [Header("Player")]
    [SerializeField] private Player _playerPrefab;
    [SerializeField] private Transform _playerSpawnTransform;
    [SerializeField] private Joystick _playerJoystick;

    [Header("Enemy")]
    [SerializeField] private EnemySpawner _enemySpawner;

    [Header("Coins")]
    [SerializeField] private CoinController _coinController;

    [Header("EventBus")]
    [SerializeField] private EventBus _eventBus;

    private CinemachineTransposer _cinemachineTransposer;
    private int _currentLvlIndex = 0;
    private Player _player;
    
    private (Vector3, float) _cameraSettings;

    private void Awake()
    {
        _eventBus.Subscribe<RestartLevelClickedSignal>(OnRestartLevelClicked);
        _cinemachineTransposer = _virtualCam.GetCinemachineComponent<CinemachineTransposer>();
        _eventBus.Subscribe<FinishTriggerSignal>(PlayerFinished);
        _eventBus.Subscribe<StartCountDownFinishedSignal>(OnCountDownFinished);
        SetUpLevel(_currentLvlIndex);
    }

    private void SetUpLevel(int levelIndex)
    {
        
        ChangeGameState(GameStateManager.GameState.Paused); 
        _eventBus.Invoke(new SetupLevelSignal(levelIndex));
        SetCamera(); 
        SpawnPlayer();
        SpawnEnemies();
        _eventBus.Invoke(new StartLevelSignal(levelIndex));
    }

    private void OnCountDownFinished(StartCountDownFinishedSignal signal) 
    {
        ChangeGameState(GameStateManager.GameState.GamePlay);
    }

    private void SetCamera()
    {
        _cameraFitter.SetFitObjectPosition(_playerSpawnTransform.position);

        if (_cameraSettings.Item2 == 0) _cameraSettings = _cameraFitter.GetPosAndOrthoSize();

        _cinemachineTransposer.m_FollowOffset = _cameraSettings.Item1;
        _virtualCam.m_Lens.OrthographicSize = _cameraSettings.Item2;
    }

    private void SpawnPlayer()
    {
        _player = Instantiate(_playerPrefab, _playerSpawnTransform.position, Quaternion.identity);
        _player.SetCameraTransform(_mainCam.transform);
        _player.SetJoystick(_playerJoystick);
        _virtualCam.m_LookAt = _player.transform;
        _virtualCam.m_Follow = _player.transform;
        _coinController.Init(_player.transform);
    }

    private void SpawnEnemies() 
    {
        _enemySpawner.Init(_levelInfos, _player.transform);
        _enemySpawner.SpawnEnemies(_currentLvlIndex);
    }

    private void ChangeGameState(GameStateManager.GameState state)
    {
        GameStateManager.SetState(state);
    }

    private void OnRestartLevelClicked(RestartLevelClickedSignal signal) 
    {
        RestartLevel();
    }

    private void RestartLevel()
    {
        _eventBus.Invoke(new RestartLevelSignal());
        
        SetUpLevel(_currentLvlIndex);
    }

    private void PlayerFinished(FinishTriggerSignal signal)
    {
        _currentLvlIndex++;

        if (_currentLvlIndex > _levelInfos.Count - 1) _currentLvlIndex = 0;

        RestartLevel();
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<RestartLevelClickedSignal>(OnRestartLevelClicked);
        _eventBus.Unsubscribe<FinishTriggerSignal>(PlayerFinished);

        _eventBus.Unsubscribe<StartCountDownFinishedSignal>(OnCountDownFinished);
    }

    /*
    геймћенеджер
    поле уровн€ будет одно, но будут лвл»нфо
    лвл инфо содержит количество врагов и их типы дл€ спавна
    
    у менеджера есть список лвл инфо и он берет их по очереди

    спавнит врагов в рандомных точках 
    спавнит игрока
    выставл€ет камеру по ширине уровн€ и над игроком
    включает отсчет перед игрой 3..2..1..го
    переключает игру с состо€ние геймплей

    во врем€ геймпле€ геймћенеджер следит за смерт€ми
    если умирает враг, смотрит остались ли еще живые враги, если нет, 
    то открывает дорогу на другой уровень
    если умирает персонаж, то включает экран смерти

    камера следует за игроком по оси Z

    триггер в конце уровн€
    по€вл€етс€, когда все враги побеждены
    вызывает запуск следующего уровн€ 
     */
}
