using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class TapButton : MonoBehaviour
{
    //추후 인젝트로 변경
    [SerializeField] private ForgeCanvas forge;
    [SerializeField] private Parts_kr parts;

    [SerializeField] private List<TapButton> tapButtons;
    [SerializeField] private List<TypeButton> typeButtons;

    [SerializeField] [SerializedDictionary] private SerializedDictionary<Parts_kr, List<string>> typeDict;
    private void Start()
    {
        TypeDictInit();
        
    }
    

    /// <summary>
    /// 해당 버튼이 웨펀인지 서브웨펀인지 알려주는 메서드
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
        HashSet<string> tempSet = new HashSet<string>();
        
        ItemDatabaseManager IDM = ItemDatabaseManager.instance;

        if (WhoAmI() == 0)
        {
            foreach (ItemData item in IDM.GetNormalWeaponItem())
            {
                tempSet.Add(item.WeaponType.ToString());
            }
            
            typeDict[Parts_kr.무기] = tempSet.ToList();
        }

        else
        {
            foreach (ItemData item in IDM.GetSubWeaponItem())
            {
                tempSet.Add(item.SubWeaponType.ToString());
            }
            
            typeDict[Parts_kr.보조무기] = tempSet.ToList();
        }
    }
}
