using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForPlayerState : INpcState
{
    private Npc npc;
    public WaitForPlayerState(Npc npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        npc.WantItemMarkOnOff(Emoji.EXCLAMATION);

        /*느낌표 떠있고 걍 서있게
         * 그상태에서 플레이어가 가까이가서 e누르면
         * 대화창 나오고 아이템 선택 그 UI 노출
         * 
         * 아이템을 바로 주면 거래 완료 후 애가 떠남
         * 아이템 없으면 만들어올 시간 주고 시간 지나면 빡쳐서 떠남
         * 
         * 카운터 클래스를 만들고
         * 스토어매니저 참조 > 카운터에서 스토어매니저의 큐를 픽함
         * 카운터에서 픽을 가져옴 > 캐릭터가 카운터에 가면 > e키누름 > 카운터가 가지고 있게 > 
         */
    }

    public void Update()
    {
    }
    public void Exit() { }
}
