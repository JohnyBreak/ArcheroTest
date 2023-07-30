using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{
    public event Action DeathEvent;
    public event Action <int> HealthChangedEvent;
    [SerializeField] private int _maxHealth = 15;
    private int _currentHealth;

    public int MaxHealth => _maxHealth;

    //private void Awake()
    //{
    //    SetMaxHealth(_maxHealth);
    //}

    public void SetMaxHealth(int amount) 
    {
        _maxHealth = amount;
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        HealthChangedEvent?.Invoke(_currentHealth);
        if (_currentHealth < 1) 
        {
            // die
            DeathEvent?.Invoke();
        }
    }
}
