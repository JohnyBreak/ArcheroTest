using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StopDecision")]
public class UnitStopDecision : UnitBaseDecision
{
    public override bool Decide(UnitStateMachine machine)
    {
        if (machine.CheckIfStateCountDownElapsed(machine.Config.MoveTime))
        {
            machine.UnitAnimations.TriggerIdle();
            machine.Agent.destination = machine.transform.position;
            return true;
        }
        else return false;
    }
}
