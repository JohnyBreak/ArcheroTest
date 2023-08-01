using UnityEngine;

[CreateAssetMenu(fileName = "UnitConfig", menuName = "Unit/UnitConfig")]
public class UnitConfig : ScriptableObject
{
    [SerializeField] protected float _moveSpeed = 5;
    [SerializeField] protected int _maxHealth = 5;
    [SerializeField] [Min(0.1f)] protected float _attackSpeed = 0.2f;
    [SerializeField] protected AnimationSettings _animationSettings;
    
    public float AttackSpeed => _attackSpeed;
    public float MoveSpeed => _moveSpeed;
    public int MaxHealth => _maxHealth;
    public AnimationSettings AnimationSettings => _animationSettings;
}
