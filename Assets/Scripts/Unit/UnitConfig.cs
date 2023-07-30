using UnityEngine;

[CreateAssetMenu(fileName = "UnitConfig", menuName = "Unit/UnitConfig")]
public class UnitConfig : ScriptableObject
{
    [SerializeField] protected float _moveSpeed = 5;
    [SerializeField] protected int _maxHealth = 5;
    [SerializeField] [Min(0.1f)] protected float _shootDelay = 0.2f;
    public float ShootDelay => _shootDelay;
    public float MoveSpeed => _moveSpeed;
    public int MaxHealth => _maxHealth;
}
