using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterState
{
    public abstract void Enter(GeneralAnimator animator);
    public abstract void Update();
    public abstract void Exit();
}
