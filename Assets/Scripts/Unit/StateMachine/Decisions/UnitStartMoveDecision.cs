using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StartMoveDecision")]
public class UnitStartMoveDecision : UnitBaseDecision
{
    public override bool Decide(UnitStateMachine machine)
    {
        if (machine.CheckIfStateCountDownElapsed(machine.Config.IdleTime))
        {
            machine.UnitAnimations.TriggerMove();
            return true;
        }
        else return false; 
    }
}
