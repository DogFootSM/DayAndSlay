using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBuyItemState : INpcState
{
    private NPC npc;
    private ItemData item;

    public NpcBuyItemState(NPC npc, ItemData item)
    {
        this.npc = npc;
        this.item = item;
    }

    public void Enter()
    {
        Debug.Log("아이템 구매");
        npc.BuyItem(item);
        
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }
}
