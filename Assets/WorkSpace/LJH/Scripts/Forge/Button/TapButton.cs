using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TapButton : MonoBehaviour
{
    //추후 인젝트로 변경
    [SerializeField] private ForgeCanvas forge;
    [SerializeField] private Parts_kr parts;

    [SerializeField] private List<TypeButton> typeButtons;

    [SerializeField] private List<WeaponType_kr> weaponList;
    [SerializeField] private List<SubWeaponType_kr> subweaponList;
    
    [SerializeField] private List<MaterialType_kr> armorList;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetTypeButtons);
    }
    

public void SetTypeButtons()
    {
        //현재 선택된 Parts 등록
        forge.SetCurParts(parts);
        
        //무기
        if (parts == Parts_kr.무기)
        {
            for (int i = 0; i < typeButtons.Count; i++)
            {
                Debug.Log(weaponList[i].ToString());
                typeButtons[i].SetThisButton(weaponList[i].ToString());
            }
        }
        
        //보조무기
        else if (parts == Parts_kr.보조무기)
        {
            for (int i = 0; i < typeButtons.Count; i++)
            {
                typeButtons[i].SetThisButton(subweaponList[i].ToString());
            }
        }
        
        //방어구
        else
        {
            for (int i = 0; i < typeButtons.Count; i++)
            {
                typeButtons[i].SetThisButton(armorList[i].ToString());
            }
        }
        
    }
}
