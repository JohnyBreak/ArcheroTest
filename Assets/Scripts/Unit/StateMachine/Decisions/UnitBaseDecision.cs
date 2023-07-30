using UnityEngine;

public abstract class UnitBaseDecision : ScriptableObject
{
    public abstract bool Decide(UnitStateMachine machine);
}
