
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Attack")]
public class UnitAttackAction : UnitBaseAction
{
    public override void Act(UnitStateMachine machine)
    {
        Attack(machine);
    }

    private void Attack(UnitStateMachine machine) 
    {
        machine.RotateTo(machine.TargetTransform);
        if (TargetInRange(machine, machine.TargetTransform) && machine.CheckIfStateCountDownElapsed(machine.Config.ShootDelay)) 
        {
            MakeShot(machine);
            machine.ResetStateCountDown();
        }
    }

    private void MakeShot(UnitStateMachine machine) 
    {
        machine.UnitAnimations.TriggerAttack();
        Bullet b = BulletPool.Instance.GetPooledObject();//ObjectPool.Instance.GetPooledObject<Bullet>();
        b.StartFly(machine.BulletConfig.Damage, machine.BulletConfig.TargetMask, machine.BulletConfig.LifeTime, machine.BulletConfig.Speed);
        //b.transform.localRotation = _ctx.transform.localRotation;
        b.transform.position = machine.transform.localPosition;
        Quaternion lookRotation = Quaternion.LookRotation(machine.TargetTransform.position - b.transform.position);
        b.transform.localRotation = lookRotation;
    }

    private bool TargetInRange(UnitStateMachine machine, Transform target)
    {
        float distance = (((target.position.x - machine.transform.position.x) * (target.position.x - machine.transform.position.x)) +
            ((target.position.y - machine.transform.position.y) * (target.position.y - machine.transform.position.y)) +
            ((target.position.z - machine.transform.position.z) * (target.position.z - machine.transform.position.z)));

        return (distance < (machine.Config.AttackRange * machine.Config.AttackRange));
    }
}
