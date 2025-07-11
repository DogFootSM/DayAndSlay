using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INpcState
{
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}
