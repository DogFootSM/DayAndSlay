using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum CanvasType
{
    GAMESTART, CHARACTER_CREATE, ENVIRONMENT, EXIT, LOADING, SIZE
}

//캐릭터 상태 전환에 사용할 타입
[Serializable]
public enum CharacterStateType
{
    IDLE, WALK, ATTACK, SIZE
}

//이어하기 캐릭터 스프라이트 데이터 저장할 타입
[Serializable]
public enum CharacterPresetType
{
    HAIR, BODY, SHIRT, WEAPON, HAT, GLASSES, NECT, OUTER, HAND, SIZE
}

//캐릭터 상태 스프라이트 라이브러리 교체 타입
[Serializable]
public enum CharacterAnimationType
{
    DOWNIDLE, SIDEIDLE, UPIDLE, SIDEWALK, UPWALK, DOWNWALK, SIDEATTACK, UPATTACK, DOWNATTACK, SIZE
}

//캐릭터 생성 시 무기 설정 타입
[Serializable]
public enum CharacterWeaponType
{
    BOW, SHORT_SWORD, SPEAR, WAND, LONG_SWORD, SIZE
}

//캐릭터 생성 시 스프라이트 데이터 경로에 사용 타입, 
[Serializable]
public enum BodyPartsType
{
    HAIR, BODY, SHIRT, SIZE
}


/// <summary>
/// 캐릭터 데이터 컬럼
/// </summary>
[Serializable]
public enum CharacterDataColumns
{
    SLOT_ID, IS_CREATE, HAIR_SPRITE, BODY_SPRITE, 
    SHIRT_SPRITE, WEAPON_SPRITE, LAST_PLAYED_TIME, 
    WEAPON_TYPE, REMAINING_DAYS, STRENGTH, 
    AGILITY, INTELLIGENCE, OBJECTIVE_ITEM, SIZE
}

[Serializable]
public enum CharacterItemDataColumns
{
    ITEM_ID, SLOT_ID, ITEM_AMOUNT,INVENTORY_SLOT_ID
}

/// <summary>
/// 무기 타입
/// </summary>
[Serializable]
public enum Type_Weapon
{
    NotWeapon,
    Sword,
    Bow,
    Spear,
    Wand
}

/// <summary>
/// 방어구 타입
/// </summary>
[Serializable]
public enum Type_Armor
{
    NotArmor,
    Helmet,
    Armor,
    Arm,
    Pants,
    Shoes,
    Accessery,
    Cloak
}

/// <summary>
/// 방향
/// </summary>
[Serializable]
public enum Direction
{
    Up,
    Right,
    Down,
    Left
}

/// <summary>
/// 현재 맵 타입
/// 각 타입으로 맵 사이즈 가져옴 
/// </summary>
[Serializable]
public enum MapType
{
    //TODO: 추후 내용 수정 필요
    TOWN, DEONGEON
}

/// <summary>
/// 캐릭터 상호작용 시스템 창 타입
/// </summary>
[Serializable]
public enum SystemType
{
    STATUS, INVENTORY, SKILL, RECIPE, SETTING, SIZE
}

/// <summary>
/// 노드가 반환하는 값을 정해줌
/// </summary>
[Serializable]
public enum NodeState
{
    Success,
    Failure,
    Running
}


/// <summary>
/// 몬스터 상태
/// </summary>
[Serializable]
public enum M_State
{
    Attck,
    Move,
    Die
}

