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
            machine.UnitAnimations.TriggerIdle();
            machine.ResetMachineCountDown();
            
            return true;
        }
        else
        {
            return false;
        }
    }
}