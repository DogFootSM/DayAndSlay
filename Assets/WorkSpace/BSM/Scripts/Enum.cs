using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum ButtonType
{
    NEWSTART, CONTINUEWSTART, ENVIRONMENT, EXIT
}

[Serializable]
public enum CharacterStateType
{
    IDLE, WALK, RUN, ATTACK, HURT, DEATH, SIZE
}