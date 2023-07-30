using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "PluggableAI/Decisions/StandIdleDecision")]
public class UnitStandIdleDecision : UnitBaseDecision
{
    public override bool Decide(UnitStateMachine machine)
    {
        if (machine.CheckIfMachineCountDownElapsed(machine.Config.IdleTime))
        {
            machine.ResetMachineCountDown();
            machine.UnitAnimations.TriggerIdle();
            return true;
        }
        else
        {
            return false;
        }
    }
}