using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/StopDashDecision")]
public class UnitStopDashDecision : UnitBaseDecision
{
    public override bool Decide(UnitStateMachine machine)
    {
        if (CheckDistance(machine, machine.TargetPosition))
        {
            machine.UnitAnimations.TriggerIdle();
            return true;
        }
        else return false;
    }

    private bool CheckDistance(UnitStateMachine machine, Vector3 pos)
    {
        float distance = (((pos.x - machine.transform.position.x) * (pos.x - machine.transform.position.x)) +
            ((pos.y - machine.transform.position.y) * (pos.y - machine.transform.position.y)) +
            ((pos.z - machine.transform.position.z) * (pos.z - machine.transform.position.z)));
        return (distance <= (machine.Agent.stoppingDistance * machine.Agent.stoppingDistance));
    }
}