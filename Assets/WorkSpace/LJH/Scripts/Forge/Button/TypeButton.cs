using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypeButton : MonoBehaviour
{
    [SerializeField] private TapButton parentsTapButton;
  
    [SerializeField][SerializedDictionary] private SerializedDictionary<WeaponType_kr, List<ItemData>> dict;
    [SerializeField][SerializedDictionary] private SerializedDictionary<SubWeaponType_kr, List<ItemData>> _dict;

    [SerializeField][SerializedDictionary] private SerializedDictionary<MaterialType_kr, List<ItemData>> mateDict = new SerializedDictionary<MaterialType_kr, List<ItemData>>();
    
    [SerializeField] private List<TypeButton> typeButtons;    
    [SerializeField] private List<ItemButton> itemButtons;
    
    
    private int MyIndex => typeButtons.IndexOf(this);
    private int ParentIndex => parentsTapButton.WhoAmI();
    private bool IsArmor => ParentIndex >= (int)Parts_kr.투구;

/*
    private void Start()
    {
        DictInit();

        SetMyName();
        
        Button btn = GetComponent<Button>();
        SetItemButtonData(0);
        btn.onClick.AddListener(() => SetItemButtonData(typeButtons.IndexOf(this)));

    }

    private void DictInit()
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

    }

    private void SetMyName()
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();


        if (parentsTapButton.WhoAmI() == 0)
        {
            text.text = ((WeaponType_kr)typeButtons.IndexOf(this)).ToString();
        }

        else
        {
            text.text = ((SubWeaponType_kr)typeButtons.IndexOf(this)).ToString();
        }

        ;
    }

    private void SetItemButtonData(int index)
    {
        if (parentsTapButton.WhoAmI() == 0)
        {
            for (int i = 0; i < itemButtons.Count; i++)
            {

                itemButtons[i].SetButtonItem(dict[(WeaponType_kr)index][i]);
            }
        }
        else
        {
            for (int i = 0; i < itemButtons.Count; i++)
            {
            
                itemButtons[i].SetButtonItem(_dict[(SubWeaponType_kr)index][i]);
            }
        }
    }*/

private void Start()
    {
        // 1. 데이터 초기화 (DictInit 또는 MateDictInit)
        if (IsArmor)
        {
            MateDictInit(); 
        }
        else
        {
            DictInit();
        }

        // 2. 이름 설정
        SetMyName();
        
        // 3. 리스너 등록 및 초기 아이템 표시
        Button btn = GetComponent<Button>();
        SetItemButtonData(0); // 초기 아이템 버튼 설정은 항상 0번 인덱스(자신)의 데이터로
        btn.onClick.AddListener(() => SetItemButtonData(MyIndex));
    }

    // --- 일반 아이템 초기화 로직 ---
    private void DictInit()
    {
        if (IsArmor) return; 
        
        ItemDatabaseManager IDM = ItemDatabaseManager.instance;

        if (ParentIndex == 0) // 무기 탭
        {
            dict[WeaponType_kr.활] = IDM.GetWantTypeItem(WeaponType.BOW);
            dict[WeaponType_kr.한손검] = IDM.GetWantTypeItem(WeaponType.SHORT_SWORD);
            dict[WeaponType_kr.창] = IDM.GetWantTypeItem(WeaponType.SPEAR);
            dict[WeaponType_kr.완드] = IDM.GetWantTypeItem(WeaponType.WAND);
        }
        else // 보조무기 탭 (ParentIndex == 1)
        {
            _dict[SubWeaponType_kr.화살통] = IDM.GetWantTypeItem(SubWeaponType.ARROW);
            _dict[SubWeaponType_kr.칼집] = IDM.GetWantTypeItem(SubWeaponType.SHEATH);
            _dict[SubWeaponType_kr.엠블렘] = IDM.GetWantTypeItem(SubWeaponType.EMBLEM);
            _dict[SubWeaponType_kr.마도서] = IDM.GetWantTypeItem(SubWeaponType.BOOK);
        }
    }

    // --- 아머 아이템 초기화 로직 ---
    public void MateDictInit()
    {
        if (!IsArmor) return;

        // TypeButton이 3개(천, 가죽, 중갑)이므로, 각 버튼이 모두 이 함수를 실행하지만,
        // 결과적으로 mateDict은 3가지 재질 데이터를 모두 가지게 됩니다.

        // 현재 탭(부모)이 나타내는 부위(Parts)를 얻음
        Parts currentPart = (Parts)ParentIndex;
        ItemDatabaseManager IDM = ItemDatabaseManager.instance;

        // 1. 천(CLOTH) 아이템 로드 및 필터링
        List<ItemData> allClothItems = IDM.GetWantTypeItem(MaterialType.CLOTH);
        mateDict[MaterialType_kr.천] = allClothItems.Where(item => item.Parts == currentPart).ToList();
        
        // 2. 가죽(LEATHER) 아이템 로드 및 필터링
        List<ItemData> allLeatherItems = IDM.GetWantTypeItem(MaterialType.LEATHER);
        mateDict[MaterialType_kr.가죽] = allLeatherItems.Where(item => item.Parts == currentPart).ToList();
        
        // 3. 중갑(PLATE) 아이템 로드 및 필터링
        List<ItemData> allPlateItems = IDM.GetWantTypeItem(MaterialType.PLATE);
        mateDict[MaterialType_kr.중갑] = allPlateItems.Where(item => item.Parts == currentPart).ToList();
    }

    private void SetMyName()
    {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();

        if (IsArmor)
        {
            // TypeButton의 인덱스(0, 1, 2)를 MaterialType_kr Enum(1, 2, 3)으로 변환
            text.text = ((MaterialType_kr)(MyIndex + 1)).ToString(); 
        }
        else
        {
            if (ParentIndex == 0) // 무기
            {
                text.text = ((WeaponType_kr)MyIndex).ToString();
            }
            else // 보조무기
            {
                text.text = ((SubWeaponType_kr)MyIndex).ToString();
            }
        }
    }

    public void SetItemButtonData(int index)
    {
        if (IsArmor)
        {
            MaterialType_kr selectedMaterial = (MaterialType_kr)(index + 1);
            if (mateDict.ContainsKey(selectedMaterial))
            {
                List<ItemData> items = mateDict[selectedMaterial];
                for (int i = 0; i < itemButtons.Count; i++)
                {
                    itemButtons[i].SetButtonItem(i < items.Count ? items[i] : null); 
                }
            }
        }
        else
        {
            List<ItemData> items = null;
            if (ParentIndex == 0) // 무기
            {
                items = dict[(WeaponType_kr)index];
            }
            else // 보조무기
            {
                items = _dict[(SubWeaponType_kr)index];
            }

            for (int i = 0; i < itemButtons.Count; i++)
            {
                itemButtons[i].SetButtonItem(i < items.Count ? items[i] : null);
            }
        }
    }


}
