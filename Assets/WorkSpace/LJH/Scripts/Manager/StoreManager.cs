using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoreManager : InteractableObj
{
    [SerializeField] private Queue<Npc> npcQue = new Queue<Npc>();
    [SerializeField] private Npc npc;

    [SerializeField] private GameObject popUp;
    [SerializeField] private TextMeshProUGUI reputationTextObj;

    private int reputation = 0;

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
        Debug.Log("얘 쓰이고 있나?");
        npc = PeekInNpcQue();
        npc.TalkToPlayer();
        npc.StateMachine.ChangeState(new NpcWaitItemState(npc));
    }
    public override void UiOnOffMethod(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            popUp.GetComponent<PopUp>().objName = "카운터";
            popUp.SetActive(!popUp.gameObject.activeSelf);
        }
    }

}
