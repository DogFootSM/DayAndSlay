using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreManager : InteractableObj
{
    [SerializeField] private Queue<Npc> npcQue = new Queue<Npc>();
    [SerializeField] private List<Npc> npcList = new List<Npc>();
    [SerializeField] private Npc npc;

    [SerializeField] private GameObject popUp;
    [SerializeField] private TextMeshProUGUI reputationTextObj;

    private int reputation = 0;

    int curNpcNum = 0;

    public void PlusRepu(int plus) => reputationTextObj.text = $"평판 점수 : {reputation += plus}";
    public void MinusRepu(int minus) => reputationTextObj.text = $"평판 점수 : {reputation -= minus}";

    private void Start()
    {
        reputationTextObj.text = $"평판 점수 : {reputation}";
    }
    public void EnqueueInNpcQue(Npc npc)
    {
        if (!npcQue.Contains(npc))
        {
            npcQue.Enqueue(npc);
        }
    }
    public Npc PeekInNpcQue()
    {
        if (npcQue.Count > 0)
        { 
            return npcQue.Peek();
        }

        return null;
    }
    public Npc DequeueInNpcQue()
    {
        if (npcQue.Count > 0)
        {
            return npcQue.Dequeue();
        }

        return null;
    }


    public override void Interaction()
    {
        PeekInNpcQue().TalkToPlayer();
        PeekInNpcQue().StateMachine.ChangeState(new NpcWaitItemState(npc));
    }
    public override void UiOnOffMethod(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            popUp.GetComponent<PopUp>().objName = "카운터";
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
