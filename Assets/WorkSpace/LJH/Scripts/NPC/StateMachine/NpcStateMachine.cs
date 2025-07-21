using UnityEngine;

public class NpcStateMachine
{
    private INpcState currentState;

    public void ChangeState(INpcState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();

    }

    public void Tick()
    {
        currentState?.Update();
    }

    public INpcState CurrentState => currentState;
}