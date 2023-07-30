using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelInfo", menuName = "Level/LevelInfo")]
public class LevelInfo : ScriptableObject
{
    [SerializeField] private List<EnemyAmountPair> _enenmyAmountPairs;
    public List<EnemyAmountPair> EnemyAmountPairs => _enenmyAmountPairs;
}

[System.Serializable]
public class EnemyAmountPair 
{
    public int Amount;
    public Enemy Enemy;
}