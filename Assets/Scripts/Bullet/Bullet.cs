using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour, IPoolable
{
    [SerializeField] private EventBus _eventBus;
    [SerializeField] private LayerMask _obstacleMask;
    private float _speed;
    private int _damage;
    private float _lifeTime;
    private LayerMask _damageableMask;

    private bool _canFly = false;
    private Coroutine _lifeRoutine;

    private void Awake()
    {
        _eventBus.Subscribe<RestartLevelSignal>(OnRestartLevel);
        //GameManager.RestartLevelEvent += StopFly;
    }

    void Update()
    {
        if (GameStateManager.CurrentGameState != GameStateManager.GameState.GamePlay) return;
        if (!_canFly) return;
        transform.position += transform.forward * _speed * Time.deltaTime;
    }

    private void OnDisable()
    {
        StopFly();
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<RestartLevelSignal>(OnRestartLevel);
        //GameManager.RestartLevelEvent -= StopFly;
    }

    public void StartFly(int damage, LayerMask mask, float time, float speed)
    {
        _damageableMask = mask;
        _damage = damage;
        _lifeTime = time;
        _speed = speed;
        _canFly = true;
        this.gameObject.SetActive(true);

        if (_lifeRoutine != null) 
        {
            StopCoroutine(_lifeRoutine);
            _lifeRoutine = null;
        }
        _lifeRoutine = StartCoroutine(LifeRoutine());
    }

    private IEnumerator LifeRoutine() 
    { 
        float time = 0;
        while (time < _lifeTime) 
        {
            if (GameStateManager.CurrentGameState == GameStateManager.GameState.GamePlay) 
            {
                time += Time.deltaTime;
            }
            yield return null;
        }
        StopFly();
    }

    private void OnRestartLevel(RestartLevelSignal signal) 
    {
        StopFly();
    }

    public void StopFly()
    {
        if (_lifeRoutine != null)
        {
            StopCoroutine(_lifeRoutine);
            _lifeRoutine = null;
        }

        _canFly = false;
        //DestroyBullet();
        if(BulletPool.Instance != null) BackToPool();
    }

    private void DestroyBullet() 
    {
        Destroy(transform.root.gameObject);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (((1 << collision.gameObject.layer) & _obstacleMask) != 0)
        {
            BackToPool();
            return;
        }

       if (((1 << collision.gameObject.layer) & _damageableMask) == 0) return;



        if (collision.gameObject.TryGetComponent<HealthSystem>(out var health))
        {
            health.TakeDamage(_damage);
            BackToPool();
        }
    }

    public void BackToPool()
    {
        BulletPool.Instance.DisableObject(this);
    }
}
