using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class UnitChaseAction : UnitBaseAction
{
    public override void Act(UnitStateMachine machine)
    {
        Chase(machine);
    }

    private void Chase(UnitStateMachine machine)
    {
        machine.Agent.destination = machine.TargetTransform.position;
        machine.Agent.isStopped = false;
    }
}
