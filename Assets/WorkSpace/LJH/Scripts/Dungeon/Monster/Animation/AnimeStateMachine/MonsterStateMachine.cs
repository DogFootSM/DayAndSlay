using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine
{
    public IMonsterState currentState;
    GeneralAnimator animator;

    public MonsterStateMachine(GeneralAnimator animator)
    {
        this.animator = animator;
    }

    public void ChangeState(IMonsterState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter(animator);
    }

    public void Update()
    {
        currentState?.Update();
    }
}
