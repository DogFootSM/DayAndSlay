using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeButton : MonoBehaviour
{
    [SerializeField] private TapButton parentsTapButton;
  
    [SerializeField][SerializedDictionary] private SerializedDictionary<WeaponType_kr, List<ItemData>> dict;
    [SerializeField][SerializedDictionary] private SerializedDictionary<SubWeaponType_kr, List<ItemData>> _dict;

    
    [SerializeField] private List<TypeButton> typeButtons;    
    [SerializeField] private List<ItemButton> itemButtons;


    private void Start()
    {
        ItemDatabaseManager IDM = ItemDatabaseManager.instance;

        if (parentsTapButton.WhoAmI() == 0)
        {
            dict[WeaponType_kr.활] = IDM.GetWantTypeItem(WeaponType.BOW);
            dict[WeaponType_kr.한손검] = IDM.GetWantTypeItem(WeaponType.SHORT_SWORD);
            dict[WeaponType_kr.창] = IDM.GetWantTypeItem(WeaponType.SPEAR);
            dict[WeaponType_kr.완드] = IDM.GetWantTypeItem(WeaponType.WAND);
        }

        else
        {
            _dict[SubWeaponType_kr.화살통] = IDM.GetWantTypeItem(SubWeaponType.ARROW);
            _dict[SubWeaponType_kr.칼집] = IDM.GetWantTypeItem(SubWeaponType.SHEATH);
            _dict[SubWeaponType_kr.엠블렘] = IDM.GetWantTypeItem(SubWeaponType.EMBLEM);
            _dict[SubWeaponType_kr.마도서] = IDM.GetWantTypeItem(SubWeaponType.BOOK);
        }

        SetMyName();
        
        Button btn = GetComponent<Button>();
        SetItemButtonData(typeButtons.IndexOf(this));
        btn.onClick.AddListener(() => SetItemButtonData(typeButtons.IndexOf(this)));

    }

    private void SetMyName()
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        
        text.text = ((WeaponType_kr)typeButtons.IndexOf(this)).ToString();;
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
            
            itemButtons[i].SetButtonItem(dict[(WeaponType_kr)index][i]);
        }
    }


}
