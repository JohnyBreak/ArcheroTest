using System;

[Serializable]
public class UnitStateTransition
{
    public UnitBaseDecision Decision;
    public UnitState PositiveState;
    public UnitState NegativeState;
}
