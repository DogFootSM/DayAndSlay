using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTNode
{
    public NodeState State;
    public abstract NodeState Tick();
}
