using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory)
    {
        _isRootState = true;
    }
    

    public override void EnterState()
    {
        //_ctx.PlayerAnimation.SetGrounded(true);
        _ctx.AppliedMovementZ = 0;
        _ctx.AppliedMovementX = 0;
        InitializeSubState();
        
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        
    }

    public override void InitializeSubState()
    {
        if (!_ctx.IsMovementPressed)
        {
            SetSubState(_factory.Idle());
        }
        else
        {
            SetSubState(_factory.Run());
        }
    }

    public override void CheckSwitchStates()
    {
    }
}
