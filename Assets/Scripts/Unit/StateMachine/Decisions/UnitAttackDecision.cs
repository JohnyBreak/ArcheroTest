using UnityEditor;
using UnityEngine;
[CreateAssetMenu(menuName = "PluggableAI/Decisions/AttackDecision")]
public class UnitAttackDecision : UnitBaseDecision
{
    public override bool Decide(UnitStateMachine machine)
    {
        if (TargetInRange(machine, machine.TargetTransform) && !machine.CheckIfMachineCountDownElapsed(machine.Config.IdleTime))
        {
            return true;
        }
        else 
        {
            machine.ResetMachineCountDown();
            return false;
        }
    }

    private bool TargetInRange(UnitStateMachine machine, Transform target)
    {
        float distance = (((target.position.x - machine.transform.position.x) * (target.position.x - machine.transform.position.x)) +
            ((target.position.y - machine.transform.position.y) * (target.position.y - machine.transform.position.y)) +
            ((target.position.z - machine.transform.position.z) * (target.position.z - machine.transform.position.z)));

        return (distance < (machine.Config.AttackRange * machine.Config.AttackRange));
    }
}
