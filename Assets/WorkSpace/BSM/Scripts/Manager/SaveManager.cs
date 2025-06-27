using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SaveManager : MonoBehaviour
{
    private SkillTree skillTree;
    private PlayerModel playerModel;
    private InventoryController inventoryController;

    public void InitSkillNode(SkillTree skillTree)
    {
        this.skillTree = skillTree;
    }

    public void InitPlayerModel(PlayerModel playerModel)
    {
        this.playerModel = playerModel;
    }

    public void InitInventorySlots(InventoryController inventoryController)
    {
        this.inventoryController = inventoryController;
    }

    public void GameDataSave()
    {
        SkillDataSave();
        StatDataSave();
        ItemDataSave();
    }
 
    /// <summary>
    /// 
    /// </summary>
    private void SkillDataSave()
    {
        Debug.Log($"스킬트리 저장 :{skillTree == null}"); 
    }
    
    private void StatDataSave()
    {
        Debug.Log($"스탯 저장 :{playerModel == null}");
    }

    private void ItemDataSave()
    {
        Debug.Log($"아이템 저장 :{inventoryController == null}");
    }
}