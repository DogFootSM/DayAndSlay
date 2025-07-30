using UnityEngine;

/// <summary>
/// 플레이어가 거래를 수락한 후 아이템을 전달받길 기다리는 상태
/// </summary>
public class NpcWaitItemState : INpcState
{
    private Npc npc;
    //Todo : waitTime = 60f
    private float waitTime = 5f;
    private float elapsed = 0f;

    public NpcWaitItemState(Npc npc)
    {
        this.npc = npc;
    }

    public void Enter()
    {
        elapsed = 0f;
    }

    public void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= waitTime)
        {
            //아이템 판매 실패한 경우
            npc.FailBuyItem();
            npc.WantItemMarkOnOff(Emoji.EXCLAMATION);
            npc.WantItemMarkOnOff(Emoji.ANGRY);
            npc.GetStoreManager().MinusRepu(10);
            npc.StateMachine.ChangeState(new NpcLeaveState(npc));
        }
        
    }

    public void Exit() { }
}