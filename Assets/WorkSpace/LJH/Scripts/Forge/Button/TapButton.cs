using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class TapButton : MonoBehaviour
{
    //추후 인젝트로 변경
    [SerializeField] private ForgeCanvas forge;
    [SerializeField] private Parts_kr parts;

    [SerializeField] private List<TypeButton> typeButtons;

    [SerializeField] [SerializedDictionary] private SerializedDictionary<Parts_kr, List<string>> typeDict;
    private void Start()
    {
        TypeDictInit();
        
        GetComponent<Button>().onClick.AddListener(SetTypeButtons);
        SetTypeButtons();
    }
    

public void SetTypeButtons()
    {
        //현재 선택된 Parts 등록
        forge.SetCurParts(parts);
        
        //무기
        if (forge.GetCurParts() == Parts_kr.무기 && parts == Parts_kr.무기)
        {
            for (int i = 0; i < typeButtons.Count; i++)
            {
                typeButtons[i].SetThisButton(typeDict[Parts_kr.무기][i]);
            }
        }
        
        //보조무기
        else if (forge.GetCurParts() == Parts_kr.보조무기 && parts == Parts_kr.보조무기)
        {
            for (int i = 0; i < typeButtons.Count; i++)
            {
                typeButtons[i].SetThisButton(typeDict[Parts_kr.보조무기][i]);
            }
        }
        
        //방어구
        else
        {
            for (int i = 0; i < typeButtons.Count; i++)
            {
                typeButtons[i].SetThisButton(typeDict[Parts_kr.갑옷][i]);
            }
        }
        
        
        if (typeButtons.Count > 0)
        {
            // 첫 번째 버튼의 클릭 이벤트 호출
            typeButtons[0].GetComponent<Button>().onClick.Invoke(); 
        }
        
    }



    /// <summary>
    /// Type 버튼 초기화
    /// </summary>
    private void TypeDictInit()
    {
        List<string> weapon = new List<string>();
        List<string> subweapon = new List<string>();
        List<string> armor = new List<string>();
        
        for (int i = 0; i < (int)WeaponType_kr.무기아님; i++)
        {
            WeaponType_kr wp = (WeaponType_kr)i;
            weapon.Add(wp.ToString());
        }
        
        for (int i = 0; i < (int)SubWeaponType_kr.보조무기아님; i++)
        {
            SubWeaponType_kr swp = (SubWeaponType_kr)i;
            subweapon.Add(swp.ToString());
        }
        for (int i = 0; i < (int)MaterialType_kr.천 + 1; i++)
        {
            MaterialType_kr ar = (MaterialType_kr)i;
            armor.Add(ar.ToString());
        }



        typeDict[Parts_kr.무기] = weapon;
        typeDict[Parts_kr.보조무기] = subweapon;
        typeDict[Parts_kr.갑옷] = armor;
    }
}
