using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitStateMachine : MonoBehaviour
{

    [SerializeField] private UnitState _currentState;
    [SerializeField] private UnitState _remainState;

    private Transform _targetTransform;
    private NavMeshAgent _agent;
    private UnitAnimations _unitAnimations;
    public EnemyConfig Config { get; set; }
    public Vector3 TargetPosition { get; set; }
    public UnitAnimations UnitAnimations => _unitAnimations;

    public UnitState CurrentState => _currentState;
    public UnitState RemainState => _remainState;
    public Transform TargetTransform => _targetTransform;
    public NavMeshAgent Agent => _agent;

    public BulletConfig BulletConfig { get; internal set; }

    private Vector3 _lastAgentVelocity;
    private NavMeshPath _lastAgentPath;
    private float _stateTimeElapsed;
    private float _machineTimeElapsed;

    private void Awake()
    {
        GameStateManager.GameStateChangedEvent += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateManager.GameStateChangedEvent -= OnGameStateChanged;
    }

    public void SetAnimations(UnitAnimations animations) 
    {
        _unitAnimations = animations;
    }

    public void RotateTo(Transform targetTransform)
    {
        //Vector3 lookPos = targetTransform.position;
        //lookPos.y = transform.localPosition.y;
        Quaternion lookRotation = Quaternion.LookRotation(targetTransform.transform.position - transform.position);
        lookRotation.x = transform.localRotation.x;
        lookRotation.z = transform.localRotation.z;
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, .1f);
        //transform.LookAt(lookPos);
    }

    public void SetNavMeshAgent(NavMeshAgent agent)
    {
        _agent = agent;
    }

    public void SetTarget(Transform targetTransform)
    {
        _targetTransform = targetTransform;
    }

    void Update()
    {
        if (GameStateManager.CurrentGameState != GameStateManager.GameState.GamePlay) return;

        _currentState.UpdateState(this);
    }

    public void TransitionToState(UnitState nextState)
    {
        if (nextState != RemainState)
        {
            _currentState = nextState;
            OnExitState();
        }
    }

    private void OnGameStateChanged(GameStateManager.GameState state)
    {
        if (GameStateManager.CurrentGameState != GameStateManager.GameState.GamePlay)
        {
            PauseAgent();
        }
        else
        {
            ResumeAgent();
        }
    }

    private void PauseAgent()
    {
        
        _lastAgentVelocity = _agent.velocity;
        _lastAgentPath = _agent.path;
        _agent.velocity = Vector3.zero;
        if (!_agent.isActiveAndEnabled) return;
        _agent.ResetPath();
        _agent.isStopped = true;
    }

    private void ResumeAgent()
    {
        _agent.velocity = _lastAgentVelocity;
        if (_lastAgentPath != null) _agent.SetPath(_lastAgentPath);
        _agent.isStopped = false;
    }

    public bool CheckIfStateCountDownElapsed(float duration)
    {
        _stateTimeElapsed += Time.deltaTime;

        return (_stateTimeElapsed >= duration);
    }

    public bool CheckIfMachineCountDownElapsed(float duration)
    {
        _machineTimeElapsed += Time.deltaTime;

        return (_machineTimeElapsed >= duration);
    }

    public void ResetMachineCountDown()
    {
        _machineTimeElapsed = 0;
    }

    public void ResetStateCountDown()
    {
        _stateTimeElapsed = 0;
    }

    private void OnExitState()
    {
        _stateTimeElapsed = 0;
    }
}
