using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class TapButton_Armor : MonoBehaviour
{
    //추후 인젝트로 변경
    [SerializeField] private ForgeCanvas forge;
    [SerializeField] private Parts_kr parts;

    [SerializeField] private List<TypeButton_Armor> typeButtons;

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
        List<string> material = new List<string>();
        
        material.Add(nameof(MaterialType_kr.천));
        material.Add(nameof(MaterialType_kr.가죽));
        material.Add(nameof(MaterialType_kr.중갑));
        
        for (int i = 2; i < (int)Parts_kr.신발+1; i++)
        {
            typeDict[(Parts_kr)i] = material;
        }
    }
}
