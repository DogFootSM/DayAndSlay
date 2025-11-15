using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class TapButton_Armor : MonoBehaviour
{
    [SerializeField] private List<TapButton_Armor> tapButtons;
    [SerializeField] private List<TypeButton_Armor> typeButtons;

    [SerializeField] [SerializedDictionary] private SerializedDictionary<Parts_kr, List<string>> typeDict;
    private void Start()
    {
        TypeDictInit();
        GetComponent<Button>().onClick.AddListener(TypeDictInit);
    }
    

    /// <summary>
    /// 해당 버튼이 무슨부위인지 알려주는 메서드
    /// </summary>
    /// <returns></returns>
    public int WhoAmI()
    {
        return tapButtons.IndexOf(this);
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
