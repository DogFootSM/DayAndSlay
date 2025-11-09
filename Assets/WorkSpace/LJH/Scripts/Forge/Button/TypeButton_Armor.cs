using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeButton_Armor : MonoBehaviour
{
  
    [SerializeField][SerializedDictionary] private SerializedDictionary<MaterialType_kr, List<ItemData>> mateDict;

    [SerializeField] private List<TypeButton_Armor> typeButtons;    
    [SerializeField] private List<ItemButton> itemButtons;


    private void Start()
    {
        
        mateDict[MaterialType_kr.천] = ItemDatabaseManager.instance.GetWantTypeItem(MaterialType.CLOTH);
        mateDict[MaterialType_kr.가죽] = ItemDatabaseManager.instance.GetWantTypeItem(MaterialType.LEATHER);
        mateDict[MaterialType_kr.중갑] = ItemDatabaseManager.instance.GetWantTypeItem(MaterialType.PLATE);
        SetMyName();
        
        Button btn = GetComponent<Button>();
        SetItemButtonData(typeButtons.IndexOf(this));
        btn.onClick.AddListener(() => SetItemButtonData(typeButtons.IndexOf(this)));

    }

    private void SetMyName()
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        
        text.text = ((MaterialType_kr)typeButtons.IndexOf(this)+1).ToString();;
    }
    
    /// <summary>
    /// 버튼 설정(다른 클래스에서 사용)
    /// </summary>
    public void SetThisButton(string typeName)
    {
        
    }

    private void SetItemButtonData(int index)
    {
        for (int i = 0; i < itemButtons.Count; i++)
        {
            itemButtons[i].SetButtonItem(mateDict[(MaterialType_kr)index+1][i]);
        }
    }


}