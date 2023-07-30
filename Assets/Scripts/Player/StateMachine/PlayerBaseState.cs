
public abstract class PlayerBaseState 
{
    protected bool _isRootState = false;
    protected PlayerStateMachine _ctx;
    protected PlayerStateFactory _factory;
    protected PlayerBaseState _currentSuperState;
    protected PlayerBaseState _currentSubState;

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory unitStateFactory) 
    {
        _ctx = currentContext;
        _factory = unitStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates() 
    {
        UpdateState();
        if (_currentSubState != null) _currentSubState.UpdateStates();
    }
    protected void SwitchState(PlayerBaseState newState) 
    {
        ExitState();

        newState.EnterState();

        if (_isRootState)
        {
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState) 
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState) 
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
    public void ExitStates() 
    {
        ExitState();
        if (_currentSubState != null) _currentSubState.ExitStates();
    }
}
