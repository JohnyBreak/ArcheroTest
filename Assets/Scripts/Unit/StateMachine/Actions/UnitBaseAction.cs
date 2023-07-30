using UnityEngine;

public abstract class UnitBaseAction : ScriptableObject
{
    public abstract void Act(UnitStateMachine machine);
}
