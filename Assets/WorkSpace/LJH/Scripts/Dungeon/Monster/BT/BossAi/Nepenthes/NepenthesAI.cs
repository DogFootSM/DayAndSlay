using System.Collections.Generic;
using UnityEngine;

public class NepenthesAI : BossMonsterAI
{
    // AI가 움직여야겠다 라고 판단했을 때 무브를 실행해
    // AI가 공격해야겠다 라고 판단했을 때 어택을 실행해
    // AI가 할게 없을 때 아이들을 실행해

    //Idle이 아닐때에만 Idle을 실행해야지
    //움직이는 중이 아닐 때에만 Move를 실행하고
    // 공격중이 아닐때에만 Attack을 실행하고
    
    //네펜데스는 두 송이로 이루어짐
    //좌측 개체는 벨루스Bellus(아름다움)
    //우측 개체는 말루스Malus(사악함)
    
    //좌측 개체는 힐 / 독장판 / 물기
    //우측 개체는 소환 / 줄기 촉수 공격 / 물기
    
    //기본적인 특수공격 시퀀스, 원거리 공격 시퀀스, 근접 공격 시퀀스는 여기에 구현해두고
    //이걸 상속한 Bellus, Malus 클래스 만들어서 각자 상세 행동을 구현
    
    // 각 개체별 공격 패턴은 BuildAttackSelector()에서 구현.

    // Bellus, Malus에서 Override 할 Attack Selector를 호출
    protected override List<BTNode> BuildAttackSelector()
    {
        return SequenceMethod() ?? new List<BTNode>();
    }

    // 기본적으로 빈 리스트 반환
    // Bellus, Malus가 이 메서드를 Override해서 공격 시퀀스를 채움
    protected virtual List<BTNode> SequenceMethod()
    {
        return new List<BTNode>();
    }
}
