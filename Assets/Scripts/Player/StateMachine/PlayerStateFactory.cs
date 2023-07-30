
using System.Collections.Generic;

public class PlayerStateFactory
{
    public enum PlayerStates
    {
        idle,
        run,
        grounded,
        shoot,
    }

    private PlayerStateMachine _context;
    private Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();

    public PlayerStates RootState;
    public PlayerStates SubState;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
        _states[PlayerStates.idle] = new PlayerIdleState(_context, this);
        _states[PlayerStates.run] = new PlayerRunState(_context, this);
        _states[PlayerStates.grounded] = new PlayerGroundedState(_context, this);
        _states[PlayerStates.shoot] = new PlayerShootState(_context, this);
    }


    public PlayerBaseState Idle() { SubState = PlayerStates.idle; return _states[PlayerStates.idle]; }
    public PlayerBaseState Run() { SubState = PlayerStates.run; return _states[PlayerStates.run]; }
    public PlayerBaseState Grounded() { RootState = PlayerStates.grounded; return _states[PlayerStates.grounded]; }
    public PlayerBaseState Shoot() { SubState = PlayerStates.shoot; return _states[PlayerStates.shoot]; }
}