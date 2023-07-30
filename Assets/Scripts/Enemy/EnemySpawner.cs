using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EventBus _eventBus;
    [SerializeField] private GameObject _enemySpawnArea;
    private Vector3 _enemySpawnVolume;
    private Vector3 _enemySpawnCenter;
    private List<Enemy> _aliveEnemies = new List<Enemy>();
    private Transform _playerTransform;
    private List<LevelInfo> _levelInfos;

    private void Awake()
    {
        _eventBus.Subscribe<EnemyDeathSignal>(OnEnemyDeath);
        _eventBus.Subscribe<RestartLevelSignal>(OnRestartLevel);
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<EnemyDeathSignal>(OnEnemyDeath);
        _eventBus.Unsubscribe<RestartLevelSignal>(OnRestartLevel);
    }

    public void Init(List<LevelInfo> levelInfos, Transform playerT)
    {
        _levelInfos = levelInfos;
        _playerTransform = playerT;
    }

    public void SpawnEnemies(int levelIndex)
    {
        GetReadyToEnemySpawn();

        foreach (var pair in _levelInfos[levelIndex].EnemyAmountPairs)
        {
            for (int i = 0; i < pair.Amount; i++)
            {
                var enemy = Instantiate(pair.Enemy);
                enemy.transform.position = GetRandomPosForEnemySpawn();
                //enemy.SetGameManager(_gameManager);
                enemy.SetTarget(_playerTransform);
                _aliveEnemies.Add(enemy);
            }
        }
    }

    private void DestroyAllEnemies()
    {
        foreach (var enemy in _aliveEnemies)
        {
            Destroy(enemy.gameObject);
        }
        _aliveEnemies.Clear();
        _aliveEnemies.Capacity = 2;
    }

    private void DestroyEnemy(Enemy enemy)
    {
        _aliveEnemies.Remove(enemy);
        Destroy(enemy.gameObject);

        if (!IsHereAnyAliveEnemy()) _eventBus.Invoke(new AllEnemyDeadSignal());
    }

    private void OnEnemyDeath(EnemyDeathSignal signal) 
    {
        DestroyEnemy(signal.Value);
    }

    private void OnRestartLevel(RestartLevelSignal signal)
    {
        DestroyAllEnemies();
    }

    private bool IsHereAnyAliveEnemy()
    {
        return _aliveEnemies.Count > 0;
    }

    private Vector3 GetRandomPosForEnemySpawn()
    {
        Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(_enemySpawnCenter.x - _enemySpawnVolume.x, _enemySpawnCenter.x + _enemySpawnVolume.x),
                    0, UnityEngine.Random.Range(_enemySpawnCenter.z - _enemySpawnVolume.z, _enemySpawnCenter.z + _enemySpawnVolume.z));
        return spawnPos;
    }

    private void GetReadyToEnemySpawn()
    {
        _enemySpawnCenter = _enemySpawnArea.transform.position;
        _enemySpawnVolume = new Vector3(_enemySpawnArea.transform.localScale.x / 2, 1, _enemySpawnArea.transform.localScale.z / 2);
    }
}
