using System.Collections.Generic;
using UnityEngine;

public class StoreManager : InteractableObj
{
    [SerializeField] private Queue<Npc> npcQue = new Queue<Npc>();
    [SerializeField] private List<Npc> npcList = new List<Npc>();
    [SerializeField] private Npc npc;

    [SerializeField] GameObject popUp;

    int curNpcNum = 0;

    public void EnqueueInNpcQue(Npc npc) => npcQue.Enqueue(npc);
    public Npc PeekInNpcQue() => npcQue.Peek();
    public Npc DequeueInNpcQue() => npcQue.Dequeue();

    public override void Interaction()
    {
        PeekInNpcQue().TalkToPlayer();
        PeekInNpcQue().StateMachine.ChangeState(new NpcWaitItemState(npc));
    }
    public override void UiOnOffMethod(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            popUp.GetComponent<PopUp>().objName = "д╚©Нем";
            popUp.SetActive(!popUp.gameObject.activeSelf);
        }
    }


    private void GoToDesk()
    {
        Npc curNpc = npcQue.Peek();
        npc = curNpc;
    }

    private void Waiting()
    {
        Vector3 randomPos = npc.GetComponentInChildren<TargetSensorInNpc>().GetRandomPosition();
        npc.StateMachine.ChangeState(new NpcMoveState(npc, randomPos));
    }

    public void GoToOutside()
    {
        npc.LeaveStore();
        npc = null;
    }
}
