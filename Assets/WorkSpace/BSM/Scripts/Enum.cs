using JetBrains.Annotations;
using System;

[Serializable]
public enum CanvasType
{
    GAMESTART, CHARACTER_CREATE, ENVIRONMENT, EXIT, LOADING, SIZE
}

//캐릭터 상태 전환에 사용할 타입
[Serializable]
public enum CharacterStateType
{
    IDLE, WALK, ATTACK, SKILL, HIT, SIZE
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

//캐릭터 능력치 투자에 대한 스탯 타입
[Serializable]
public enum CharacterStatsType
{
    STR, AGI, INT, CRI, SIZE
}

/// <summary>
/// 캐릭터 데이터 컬럼
/// </summary>
[Serializable]
public enum CharacterDataColumns
{
    SLOT_ID, IS_CREATE, HAIR_SPRITE, BODY_SPRITE, 
    SHIRT_SPRITE, WEAPON_SPRITE, LAST_PLAYED_TIME, 
    WEAPON_TYPE, REMAINING_DAYS, EXP, STATS_POINT, SKILL_POINT, CHAR_LEVEL, STRENGTH, 
    AGILITY, INTELLIGENCE, OBJECTIVE_ITEM, SIZE
}

[Serializable]
public enum CharacterSkillDataColumns
{
    SLOT_ID, SKILL_ID, SKILL_LEVEL, SKILL_UNLOCKED
}

[Serializable]
public enum CharacterItemDataColumns
{
    ITEM_ID, SLOT_ID, ITEM_AMOUNT,INVENTORY_SLOT_ID, IS_EQUIPMENT
}

/// <summary>
/// 아이템 파츠
/// </summary>
[Serializable]
public enum Parts
{
    WEAPON,
    SUBWEAPON,
    HELMET,
    ARMOR,
    PANTS,
    ARM,
    SHOES,
    ACCESSORY,
    CLOAK,
    INGREDIANT,
    RARE_INGREDIANT
}

/// <summary>
/// 아이템 파츠 한글
/// </summary>
[Serializable]
public enum Parts_kr
{
    무기,
    보조무기,
    투구,
    갑옷,
    바지,
    장갑,
    신발,
    장신구,
    망토,
    재료,
    레어_재료,
}

/// <summary>
/// 공격 장비 타입
/// </summary>
[Serializable]
public enum EquipType
{
    WEAPON,
    SUBWEAPON,
}

/// <summary>
/// 메인 무기 타입
/// </summary>
[Serializable]
public enum WeaponType
{
    BOW,
    SHORT_SWORD,
    SPEAR,
    WAND,
    NOT_WEAPON,
    LONG_SWORD,
}

/// <summary>
/// 메인 무기 타입 한글
/// </summary>
[Serializable]
public enum WeaponType_kr
{
    활,
    한손검,
    창,
    완드,
    무기아님,
    두손검,
}

/// <summary>
/// 서브 무기 타입
/// </summary>
[Serializable]
public enum SubWeaponType
{

    ARROW,
    SHEATH,
    EMBLEM,
    BOOK,
    NOT_SUBWEAPON,
    GAUNTLET,
}

/// <summary>
/// 서브 무기 타입 한글
/// </summary>
[Serializable]
public enum SubWeaponType_kr
{
    화살통,
    칼집,
    엠블렘,
    마도서,
    보조무기아님,
    건틀렛,
}

/// <summary>
/// 방어구 재질 타입
/// </summary>
[Serializable]
public enum MaterialType
{
    PLATE,
    LEATHER,
    CLOTH,
}
[Serializable]
public enum MaterialType_kr
{
    중갑,
    가죽,
    천,
}

/// <summary>
/// 방어구 부위 타입
/// </summary>
[Serializable]
public enum ArmorType
{
    HELMET,
    ARMOR,
    PANTS,
    ARM,
    SHOES,
    Size,
}
[Serializable]
public enum ArmorType_kr
{
    투구,
    갑옷,
    바지,
    장갑,
    신발,
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
    IDLE,
    ATTACK,
    MOVE,
    DIE
}

public enum ItemSet
{
    NOT_SET,
    APOLON,
    ARTEMIS,
    ZEUS,
    COSMOS,
    NIGHTFANG,
    DRAGONKNIGHT,

}

/// <summary>
/// 스킬 사용 타입
/// </summary>
[Serializable]
public enum SkillType
{
    ACTIVE, PASSIVE
}

/// <summary>
/// 스킬 해금 상태
/// </summary>
public enum SkillState
{
    LOCKED, UNLOCKED, COMPLETE
}

/// <summary>
/// 스킬이 장착되어 있는 퀵슬롯 키 타입
/// </summary>
[Serializable]
public enum QuickSlotType
{
    Q, W, E, R, A, S, D, F, NONE
}

/// <summary>
/// 재생할 효과음 타입
/// </summary>
[Serializable]
public enum SFXSound
{
   INVENTORY_DROP, GET_ITEM, UI_WINDOW_OPENED, UI_WINDOW_CLOSED, BUTTON_CLICKED, SHORT_SWORD_NORMAL_ATTACK
}

/// <summary>
/// 재생할 배경음 타입
/// </summary>
[Serializable]
public enum BGMSound
{
    START_SCENE_BGM, TOWN_SCENE_BGM, DENGEON_1_BGM
}

/// <summary>
/// 설정창 탭 타입
/// </summary>
[Serializable]
public enum Setting
{
    AUDIO, VIDEO
}

/// <summary>
/// 윈도우 모드 설정 타입
/// </summary>
[Serializable]
public enum WindowModeState
{
    FULLSCREEN, WINDOW, BORDERLESS
}

[Serializable]
public enum DoorType
{
    DOOR,
    LADDER,
}

[Serializable]
public enum DayAndNight
{
    DAY,
    NIGHT,
}

[Serializable]
public enum Emoji
{
    EXCLAMATION = 2,
    ANGRY,
}
public enum GenderType
{
    MALE, FEMALE
}

public enum AgeType
{
    CHILD, ADULT, SENIOR
}

public enum StageNum
{
    STAGE1,
    STAGE2,
    STAGE3,
    STAGE4,
    STAGE5,
    STAGE6,
    STAGE7,
    STAGE8,
    STAGE9,
    STAGE10,
}