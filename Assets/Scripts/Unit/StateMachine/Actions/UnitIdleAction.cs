using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Idle")]
public class UnitIdleAction : UnitBaseAction
{
    public override void Act(UnitStateMachine machine)
    {
        Idle(machine);
    }
    private void Idle(UnitStateMachine machine) 
    {
        machine.RotateTo(machine.TargetTransform);
    }
}
