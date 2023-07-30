
public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
    { }
    

    public override void EnterState()
    {
        _ctx.AppliedMovementX = 0;
        _ctx.AppliedMovementZ = 0;

        _ctx.PlayerAnimation.SetStance(PlayerAnimation.Stance.Idle);
    }

    public override void ExitState()
    {
    }

    public override void InitializeSubState()
    {
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.IsMovementPressed)
        {
            SwitchState(_factory.Run());
        }
        if (_ctx.FieldOfView.CurrentTarget != null) 
        {
            SwitchState(_factory.Shoot());
        }
    }
}
