using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStateMachine : MonoBehaviour
{
    public INpcState currentState;
    

    public void ChangeState(INpcState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}
