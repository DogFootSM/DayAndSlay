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
    public void DequeueInNpcQueue() => npcQue.Dequeue();

    private void Update()
    {
        if (npc == null && npcQue.Count > 0)
        {
            Debug.Log("데스크로 가서 줄섬");
            GoToDesk();
        }
        if(npc != null)
        {
            Debug.Log("상점 돌아다님");
            //npc에 안들어간 애들한테만
            Waiting();
        }

        Debug.Log($"현재 엔피씨 큐의 인원 수{npcQue.Count}");

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
