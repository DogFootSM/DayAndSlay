using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    [SerializeField] private Queue<Npc> npcQue = new Queue<Npc>();
    [SerializeField] private List<Npc> npcList = new List<Npc>();
    [SerializeField] private Npc npc;

    int curNpcNum = 0;

    public void EnqueueInNpcQue(Npc npc) => npcQue.Enqueue(npc);
    public Npc PeekInNpcQue() => npcQue.Peek();
    public Npc DequeueInNpcQue() => npcQue.Dequeue();

    private void Update()
    {
       
    }


    private void GoToDesk()
    {
        Npc curNpc = npcQue.Peek();
        npc = curNpc;
    }

    private void Waiting()
    {
        Vector3 randomPos = npc.GetComponentInChildren<TargetSensorInNpc>().GetRandomPosition();
        npc.StateMachine.ChangeState(new MoveState(npc, randomPos));
    }

    public void GoToOutside()
    {
        npc.LeaveStore();
        npc = null;
    }
}
