using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Coin : MonoBehaviour, IPoolable
{
    [SerializeField] private EventBus _eventBus;
   
    [SerializeField]private LayerMask _playerMask;
    private CoinController _controller;

    private Collider _collider;

    public void StartFly() 
    {
        _collider.enabled = true;
    }

    private void Awake()
    {
        _eventBus.Subscribe<RestartLevelSignal>(OnRestartLevel);
        GetComponent<Rigidbody>().useGravity = false;
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        _collider.enabled = false;
    }

    private void OnDestroy()
    {
        _eventBus.Unsubscribe<RestartLevelSignal>(OnRestartLevel);
    }

    public void SetController(CoinController controller) 
    {
        _controller = controller;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (((1 << collision.gameObject.layer) & _playerMask) == 0) return;

        _controller.DespawnCoin(this);
    }

    private void OnRestartLevel(RestartLevelSignal signal) 
    {
        BackToPool();
    }

    public void BackToPool()
    {
        _collider.enabled = false;
        CoinPool.Instance.DisableObject(this);
    }
}
