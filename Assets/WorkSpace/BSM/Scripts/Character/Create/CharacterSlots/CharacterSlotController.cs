using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSlotController : MonoBehaviour
{
    [SerializeField] private List<CharacterSlot> characterSlots;

    protected void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            characterSlots[i].slotIndex = i + 1;
        }
    }
}