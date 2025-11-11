using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeButton_Armor : MonoBehaviour
{

    [SerializeField] private List<GameObject> taps;
    [SerializeField][SerializedDictionary] private SerializedDictionary<MaterialType_kr, List<ItemData>> mateDict = new SerializedDictionary<MaterialType_kr, List<ItemData>>();

    [SerializeField] private List<TypeButton_Armor> typeButtons;    
    [SerializeField] private List<ItemButton> itemButtons;


    private void Start()
    {
        SetMyName();
        
        Button btn = GetComponent<Button>();
        
        btn.onClick.AddListener(() => SetItemButtonData(typeButtons.IndexOf(this)));
        
        MateDictInit();
        
        SetItemButtonData(0);

    }

    public void MateDictInit()
    {
        GameObject tap_armor = null;

        foreach (GameObject tap in taps)
        {
            if (tap.gameObject.activeSelf)
            {
                tap_armor = tap;
            }
        }
        
        List<ItemData> list = new List<ItemData>();
        list = ItemDatabaseManager.instance.GetWantTypeItem(MaterialType.CLOTH);
        
        List<ItemData> tempClothList = new List<ItemData>();
        List<ItemData> tempLeatherList = new List<ItemData>();
        List<ItemData> tempPlateList = new List<ItemData>();
        
        foreach (ItemData item in list)
        {
            if (item.Parts == (Parts)taps.IndexOf(tap_armor) + 2)
            {
                tempClothList.Add(item);
            }
        }
        mateDict[MaterialType_kr.Ãµ] = tempClothList;
        list = ItemDatabaseManager.instance.GetWantTypeItem(MaterialType.LEATHER);
        foreach (ItemData item in list)
        {
            if (item.Parts == (Parts)taps.IndexOf(tap_armor) + 2)
            {
                tempLeatherList.Add(item);
            }
        }
        mateDict[MaterialType_kr.°¡Á×] = tempLeatherList;
        
        list = ItemDatabaseManager.instance.GetWantTypeItem(MaterialType.PLATE);
        foreach (ItemData item in list)
        {
            if (item.Parts == (Parts)taps.IndexOf(tap_armor) + 2)
            {
                tempPlateList.Add(item);
            }
        }
        
        mateDict[MaterialType_kr.Áß°©] = tempPlateList;
    }

    private void SetMyName()
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        
        text.text = ((MaterialType_kr)typeButtons.IndexOf(this)+1).ToString();;
    }
    

    public void SetItemButtonData(int index)
    {
        if (mateDict == null)
        {
            StartCoroutine(DelayCoroutine());
        }

        else
        {
            for (int i = 0; i < itemButtons.Count; i++)
            {
                itemButtons[i].SetButtonItem(mateDict[(MaterialType_kr)index + 1][i]);
            }
        }
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        SetItemButtonData(0);
    }


}