using UnityEngine;

public class NpcStateMachine
{
    private INpcState currentState;

    public void ChangeState(INpcState newState)
    {
        Debug.Log($"상태 전이: {currentState?.GetType().Name} → {newState.GetType().Name}");
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