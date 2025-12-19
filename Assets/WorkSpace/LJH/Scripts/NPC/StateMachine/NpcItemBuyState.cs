using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcItemBuyState : INpcState
{
    private Npc npc;
    private Table table;
    public NpcItemBuyState(Npc npc, Table table)
    {
        this.npc = npc;
        this.table = table;
    }
    
    public void Enter()
    {
        npc.BuyItemFromTable();
        table.BuyItem();
    }

    public void Update() { }
    public void Exit() { }
}
