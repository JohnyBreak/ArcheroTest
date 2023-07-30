using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Dash")]
public class UnitDashAction : UnitBaseAction
{
    public override void Act(UnitStateMachine machine)
    {
        Dash(machine);
    }
    private void Dash(UnitStateMachine machine)
    {
        machine.Agent.destination = machine.TargetPosition;
        machine.Agent.isStopped = false;
    }
}
