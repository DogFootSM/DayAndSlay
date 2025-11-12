using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class TapButton : MonoBehaviour
{

    [SerializeField] private List<TapButton> tapButtons;
    [SerializeField] private List<TypeButton> typeButtons;
	[SerializeField] private ItemButton itemButton;

    [SerializeField] [SerializedDictionary] private SerializedDictionary<Parts_kr, List<string>> typeDict;
    
    private void Start()
    {
        TypeDictInit();

        Button btn = GetComponent<Button>();
        
		btn.onClick.AddListener(SetDefaultItem);
        btn.onClick.Invoke();
        
    }

    private void SetDefaultItem()
    {
        itemButton.GetComponent<Button>().onClick.Invoke();
    }

	
    
	// 탭 버튼이 눌릴때마다 미리보기 창에 현재 탭 > type0 > items0 이 눌려야함

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
