using UnityEngine;

public class NpcWaitItemState : INpcState
{
    private Npc npc;
    //Todo : waitTiem = 60f
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
            npc.BuyItemFromDesk();
            npc.WantItemMarkOnOff(Emoji.EXCLAMATION);
            npc.WantItemMarkOnOff(Emoji.ANGRY);
            npc.GetStoreManager().MinusRepu(10);
            npc.StateMachine.ChangeState(new NpcLeaveState(npc));
        }
        //아이템 판매 성공한 경우
        //if(아이템 판매 성공)
        npc.BuyItemFromDesk();
        npc.WantItemMarkOnOff(Emoji.ANGRY);
        npc.GetStoreManager().PlusRepu(10);
        npc.StateMachine.ChangeState(new NpcLeaveState(npc));
    }

    public void Exit() { }
}