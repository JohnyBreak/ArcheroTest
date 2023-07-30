
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamager : MonoBehaviour
{
    [SerializeField] private LayerMask _damageableLayer;
    [SerializeField] private float _damageInterval = 1f;
    private int _damage = 5;

    private Coroutine _damageRoutine;
    private WaitForSeconds _wait;
    private List<HealthSystem> _damageableHealths = new List<HealthSystem>();

    private void Awake()
    {
        _wait = new WaitForSeconds(_damageInterval);
    }

    public void SetFamage(int amount) 
    {
        _damage = amount;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & _damageableLayer) == 0) return;

        if (collision.transform.TryGetComponent<HealthSystem>(out var health))
        {
            if (!_damageableHealths.Contains(health)) _damageableHealths.Add(health);
            StartDamage();
        }
    }

    private void StartDamage()
    {
        StopDamage();
        StartCoroutine(DamageRoutine());
    }

    private void DealDamage(HealthSystem health)
    {
        health.TakeDamage(_damage);
    }

    private void StopDamage() 
    {
        if (_damageRoutine != null)
        {
            StopCoroutine(_damageRoutine);
            _damageRoutine = null;
        }
    }

    private IEnumerator DamageRoutine() 
    {
        while (true) 
        { 
            yield return null;

            foreach (var health in _damageableHealths)
            {
                DealDamage(health);
            }

            yield return _wait;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & _damageableLayer) == 0) return;

        if (collision.transform.TryGetComponent<HealthSystem>(out var health))
        {
            if (_damageableHealths.Contains(health)) 
            {
                _damageableHealths.Remove(health);
            }

            if(_damageableHealths.Count < 1) 
            {
                _damageableHealths.Clear();
                _damageableHealths.Capacity = 2;
                StopDamage();
            }
        }
    }

    private void OnDestroy()
    {
        StopDamage();
    }
}
