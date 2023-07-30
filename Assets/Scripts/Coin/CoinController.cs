using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    [SerializeField] private EventBus _eventBus;
    [SerializeField] private float _coinSpeed = 3;
    private List<Coin> _coins = new List<Coin>();
    private CancellationTokenSource _flyCancelToken;
    private Transform _player;

    private void Awake()
    {
        _eventBus.Subscribe<RestartLevelSignal>(OnRestartLevel, 1);
        _eventBus.Subscribe<EnemyDeathSignal>(OnEnemyDeath);
        _eventBus.Subscribe<AllEnemyDeadSignal>(OnAllEnemyDeath);
    }

    private void OnAllEnemyDeath(AllEnemyDeadSignal signal)
    {
        FlyCoinsToPlayer();
    }

    private void OnEnemyDeath(EnemyDeathSignal signal) 
    {
        SpawnCoins(signal.Value.transform.position);
    }

    public void Init(Transform player) 
    {
        _player = player;
    }

    private void SpawnCoins(Vector3 pos, int amount = 1) // прикрутить разброс по разным позициям на дотвине
    {
        for (int i = 0; i < amount; i++)
        {
            var coin = CoinPool.Instance.GetPooledObject();
            coin.gameObject.SetActive(true);
            coin.transform.position = pos;
            coin.SetController(this);
            _coins.Add(coin);
        }
    }

    private void FlyCoinsToPlayer()
    {
        foreach (var coin in _coins)
        {
            coin.StartFly();
        }

        _flyCancelToken = new CancellationTokenSource(); 
        FlyCoinsAsync(_player, _flyCancelToken.Token);
    }

    private async void FlyCoinsAsync(Transform player, CancellationToken token)
    {
        while (_coins.Count > 0 && !token.IsCancellationRequested)
        {
            if(GameStateManager.CurrentGameState == GameStateManager.GameState.GamePlay) 
            { 
                foreach (var coin in _coins)
                {
                    coin.transform.position = Vector3.MoveTowards(
                        coin.transform.position, 
                        player.transform.position, _coinSpeed * Time.deltaTime);
                }
            }
            await System.Threading.Tasks.Task.Yield();//Delay((int)(Time.deltaTime * 1000));
        }
    }

    public void DespawnCoin(Coin coin)
    {
        coin.BackToPool();
        _coins.Remove(coin);
        if (_coins.Count < 1) _coins.Capacity = 2;
    }

    private void DespawnAllCoins()
    {
        for (int i = 0; i < _coins.Count; i++)
        {
            _coins[i].BackToPool();
            _coins.Remove(_coins[i]);
        }
        _coins.Capacity = 2;
    }

    private void OnRestartLevel(RestartLevelSignal signal) 
    {
        DespawnAllCoins();
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<RestartLevelSignal>(OnRestartLevel);
        _eventBus.Unsubscribe<EnemyDeathSignal>(OnEnemyDeath);
        _eventBus.Unsubscribe<AllEnemyDeadSignal>(OnAllEnemyDeath);
        if (_flyCancelToken != null) _flyCancelToken.Cancel();
    }
}
