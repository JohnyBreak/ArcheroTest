using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationSettings")]
public class AnimationSettings : ScriptableObject
{
    public float BeforeAttackTime = 0.5f;
    public float AfterAttackTime = 0.5f;
}
