using UnityEngine;

[CreateAssetMenu(menuName ="PluggableAI/State")]
public class UnitState : ScriptableObject
{
    public UnitBaseAction[] Actions;
    public UnitStateTransition[] Transitions;

    public void UpdateState(UnitStateMachine machine) 
    {
        DoActions(machine);
        CheckTransitions(machine);
    }

    private void DoActions(UnitStateMachine machine) 
    {
        for (int i = 0; i < Actions.Length; i++)
        {
            Actions[i].Act(machine);
        }
    }

    private void CheckTransitions(UnitStateMachine machine)
    {
        for (int i = 0; i < Transitions.Length; i++)
        {

            if (Transitions[i].Decision.Decide(machine))
            {
                machine.TransitionToState(Transitions[i].PositiveState);
            }
            else
            {
                machine.TransitionToState(Transitions[i].NegativeState);
            }
        }
    }
}

