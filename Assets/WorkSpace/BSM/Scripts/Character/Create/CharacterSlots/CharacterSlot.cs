using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CharacterSlot : MonoBehaviour
{
    public int slotIndex;

    [Inject] private SqlManager sqlManager;
    
    private void Start()
    {
        IDataReader reader = sqlManager.ReadDataColumn(new []{"slot_id"} ,new []{"is_create"}, null,null);

        while (reader.Read())
        {
            Debug.Log($"{slotIndex} : {reader.GetInt32(0)}");
        }
        
    }
}
