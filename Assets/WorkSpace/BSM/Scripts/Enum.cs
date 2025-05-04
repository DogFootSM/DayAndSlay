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

//무기를 장착하지 않은 상태의 캐릭터 상태 타입
[Serializable]
public enum CharacterNormalAnimType
{
    IDLE, SIDEWALK, UPWALK, DOWNWALK, SIZE
}

[Serializable]
public enum CharacterWeaponStateType
{
    NONE, EQUIPPED
}

//무기를 장착하지 않은 상태의 애니메이션 타입
[Serializable]
public enum NoneEquipStateType
{
    HAIR, BODY, SHIRT, SIZE
}