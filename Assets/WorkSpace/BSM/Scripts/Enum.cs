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
    IDLE, WALK, SIZE
}

[Serializable]
public enum CharacterPresetType
{
    HAIR, BODY, SHIRT, WEAPON, HAT, GLASSES, NECT, OUTER, HAND, SIZE
}

[Serializable]
public enum CharacterAnimType
{
    IDLE, SIDEWALK, UPWALK, DOWNWALK, SIZE
}

[Serializable]
public enum CharacterWeaponStateType
{
    NONE, EQUIPPED
}

[Serializable]
public enum NoneEquipStateType
{
    HAIR, BODY, SHIRT, SIZE
}