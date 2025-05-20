using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AttackHandlerFactory
{

    public static IAttackHandler ChangeAttackType(CharacterWeaponType weaponType)
    {
        return weaponType switch
        {
            CharacterWeaponType.BOW => new RangeAttackHandler(),
            CharacterWeaponType.WAND => new MeleeAttackHandler(),
            CharacterWeaponType.SHORT_SWORD => new MeleeAttackHandler(),
            CharacterWeaponType.LONG_SWORD => new MeleeAttackHandler(),
            CharacterWeaponType.SPEAR => new MeleeAttackHandler(),
            _ => null
        };
    }
    
}
