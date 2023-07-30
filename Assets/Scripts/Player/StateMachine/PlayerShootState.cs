using System.Threading;
using UnityEngine;

public class PlayerShootState : PlayerBaseState
{
    public PlayerShootState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }
    
    public override void CheckSwitchStates()
    {
        if (_ctx.IsMovementPressed)
        {
            SwitchState(_factory.Run());
        }
        if (_ctx.FieldOfView.CurrentTarget == null)
        {
            SwitchState(_factory.Idle());
        }
    }

    public override void EnterState()
    {
        _ctx.ShootCancelToken = new CancellationTokenSource();
        CancellationToken token = _ctx.ShootCancelToken.Token;

        RotateRoutine(token);

    }

    private async void RotateRoutine(CancellationToken token) 
    {
        Quaternion lookRotation = Quaternion.LookRotation(_ctx.FieldOfView.CurrentTarget.transform.position - _ctx.transform.position);
        lookRotation.x = _ctx.transform.localRotation.x;
        lookRotation.z = _ctx.transform.localRotation.z;

        float rotationSpeed = 7f;
        float time = 0;
        while (time < 1 && !token.IsCancellationRequested) 
        {
            _ctx.transform.rotation = Quaternion.Lerp(_ctx.transform.rotation, lookRotation, time);
            time += Time.deltaTime * rotationSpeed;
            await System.Threading.Tasks.Task.Yield();//Delay((int)(Time.deltaTime * 1000));
        }

        ShootRoutine(token);

        while (!token.IsCancellationRequested)
        {
            if (_ctx.FieldOfView.CurrentTarget == null) return;

            Vector3 enemyPos = _ctx.FieldOfView.CurrentTarget.transform.position;
            enemyPos.y = _ctx.transform.position.y;

            _ctx.transform.LookAt(enemyPos, Vector3.up);

            await System.Threading.Tasks.Task.Delay((int)(Time.deltaTime * 1000));

        }
    }

    private async void ShootRoutine(CancellationToken token) 
    {

        while (!token.IsCancellationRequested) 
        {
            await System.Threading.Tasks.Task.Delay((int)(_ctx.ShootDelay * 1000));
            if (!token.IsCancellationRequested) MakeShot();
        }
    }

    private void MakeShot() 
    {
        if (GameStateManager.CurrentGameState != GameStateManager.GameState.GamePlay) return;
        if (_ctx.FieldOfView.CurrentTarget == null) return;
        _ctx.PlayerAnimation.ToggleAttack(true);

        Bullet b = BulletPool.Instance.GetPooledObject();
        b.StartFly(_ctx.BulletConfig.Damage, _ctx.BulletConfig.TargetMask, _ctx.BulletConfig.LifeTime, _ctx.BulletConfig.Speed);
        //b.transform.localRotation = _ctx.transform.localRotation;
        b.transform.position = _ctx.transform.localPosition;
        Quaternion lookRotation = Quaternion.LookRotation(_ctx.FieldOfView.CurrentTarget.transform.position - b.transform.position);
        b.transform.localRotation = lookRotation;
    }

    public override void ExitState()
    {
        _ctx.PlayerAnimation.ToggleAttack(false);
        _ctx.ShootCancelToken.Cancel();
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }
}
