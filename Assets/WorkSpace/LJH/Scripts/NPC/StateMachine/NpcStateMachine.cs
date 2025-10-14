using UnityEngine;

public class NpcStateMachine
{
    private INpcState currentState;

    public void ChangeState(INpcState newState)
    {
        Debug.Log($"{currentState} 종료됨");
        currentState?.Exit();
        currentState = newState;
        Debug.Log($"{currentState} 진입함");
        currentState?.Enter();

    }

    public void Tick()
    {
        currentState?.Update();
        
        
    }

    public INpcState CurrentState => currentState;
    
}