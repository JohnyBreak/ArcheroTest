using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletConfig", menuName = "Bullet/BulletConfig")]
public class BulletConfig : ScriptableObject
{
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _speed = 6;
    [SerializeField] private float _lifeTime = 3;

    public LayerMask TargetMask => _targetMask;
    public int Damage => _damage;
    public float Speed => _speed;
    public float LifeTime => _lifeTime;
}
