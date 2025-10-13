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
        
        Debug.Log($"현재 상태 {currentState}");
    }

    public INpcState CurrentState => currentState;
    
}