
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
        if (TargetInRange(machine, machine.TargetTransform) && machine.CheckIfStateCountDownElapsed(machine.AttackSpeed)) 
        {
            
            MakeShot(machine);
            machine.ResetStateCountDown();
        }
    }

    private async void MakeShot(UnitStateMachine machine) 
    {
        await System.Threading.Tasks.Task.Delay((int)(machine.Config.AnimationSettings.BeforeAttackTime * 1000));
        Bullet b = BulletPool.Instance.GetPooledObject();//ObjectPool.Instance.GetPooledObject<Bullet>();
        b.StartFly(machine.BulletConfig.Damage, machine.BulletConfig.TargetMask, machine.BulletConfig.LifeTime, machine.BulletConfig.Speed);
        //b.transform.localRotation = _ctx.transform.localRotation;

        if (machine == null) return;
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
