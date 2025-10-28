using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TapButton : MonoBehaviour
{
    [SerializeField] private Parts_kr parts;

    [SerializeField] private List<TypeButton> typeButtons;

    [SerializeField] private List<WeaponType_kr> weaponList;
    [SerializeField] private List<SubWeaponType_kr> subweaponList;

public void SetTypeButtons()
    {
        if (parts == Parts_kr.무기)
        {
            Debug.Log("무기버튼임");
            for (int i = 0; i < typeButtons.Count; i++)
            {
                typeButtons[i].SetThisButton(weaponList[i].ToString());
            }
        }
        
        if (parts == Parts_kr.보조무기)
        {
            Debug.Log("보조무기버튼임");
            for (int i = 0; i < typeButtons.Count; i++)
            {
                typeButtons[i].SetThisButton(subweaponList[i].ToString());
            }
        }
    }
}
