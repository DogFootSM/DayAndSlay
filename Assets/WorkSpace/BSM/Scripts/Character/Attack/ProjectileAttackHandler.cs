using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackHandler : IAttackHandler
{
    public void NormalAttack(Vector2 direction, Vector2 position)
    {
        //몬스터의 위치를 감지하고
        //화살 관리자에게 위치 전달?
        //그리고 화살 오브젝트를 그 위치로 발사해서 화살 오브젝트가 몬스터에 닿았을 때 데미지를 주면되려나..
    }
}
