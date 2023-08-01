using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PluggableAI/Decisions/DashDecision")]
public class UnitDashDecision : UnitBaseDecision
{
    public override bool Decide(UnitStateMachine machine)
    {
        if (machine.CheckIfMachineCountDownElapsed(machine.Config.IdleTime * 3))
        {
            Vector3 temp = machine.TargetTransform.position;
            temp.y = machine.transform.position.y;
            machine.TargetPosition = temp;
            machine.ResetMachineCountDown();
            machine.UnitAnimations.TriggerMove();
            return true;
        }
        else return false;
    }
}
