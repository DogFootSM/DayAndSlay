using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AttackHandlerFactory
{

    private static Dictionary<CharacterWeaponType, IAttackHandler> attackHandlerDict =
        new Dictionary<CharacterWeaponType, IAttackHandler>();
    
    /// <summary>
    /// 공격 핸들러 객체 반환
    /// </summary>
    /// <param name="weaponType">공격 타입 키</param>
    /// <returns>핸들러 객체</returns>
    public static IAttackHandler ChangeAttackType(CharacterWeaponType weaponType)
    {
        if (!attackHandlerDict.ContainsKey(weaponType))
        { 
            attackHandlerDict[weaponType] = weaponType switch
            {
                CharacterWeaponType.BOW => new ProjectileAttackHandler(),
                CharacterWeaponType.WAND => new MeleeAttackHandler(),
                CharacterWeaponType.SHORT_SWORD => new MeleeAttackHandler(),
                CharacterWeaponType.LONG_SWORD => new MeleeAttackHandler(),
                CharacterWeaponType.SPEAR => new MeleeAttackHandler(),
                _ => null
            };
        }

        return attackHandlerDict[weaponType]; 
    }
    
}
