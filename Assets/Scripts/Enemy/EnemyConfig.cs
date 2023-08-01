using UnityEngine;
[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Unit/EnemyConfig")]
public class EnemyConfig : UnitConfig
{
    [SerializeField] protected float _attackRange = 14f;
    [SerializeField] protected float _moveTime = 1;
    [SerializeField] protected float _idleTime = 1.3f;
    
    public float AttackRange => _attackRange;
    public float MoveTime => _moveTime;
    public float IdleTime => _idleTime;
}
