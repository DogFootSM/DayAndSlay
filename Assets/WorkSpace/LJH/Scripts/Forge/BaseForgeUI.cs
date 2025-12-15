using UnityEngine;

public abstract class BaseForgeUI : BaseUI
{
    private TapButton weaponTab;
    private TapButton subweaponTab;

    private TypeButton typeButton1;
    private TypeButton typeButton2;
    private TypeButton typeButton3;
    private TypeButton typeButton4;

    private ItemButton itemButton1;
    private ItemButton itemButton2;
    private ItemButton itemButton3;
    private ItemButton itemButton4;
    private ItemButton itemButton5;

    protected ItemData curItem;


    private void Start()
    {
    }

    

    protected void Tap_TabButton(Parts parts)
    {
        
    }

    protected void Tap_TypeButton(int typeIndex)
    {
        
    }

    protected void Tap_ItemButton(int index)
    {
        
    }

    private int greatPercent = 10;
    private int successPercent = 50;
    private int downPercent = 30;
    private int failPercent = 10;

    public void CreateItem()
    {
        if (curItem == null)
        {
            Debug.Log($"선택된 아이템이 없습니다. 제작할 아이템을 선택해 주세요.");
            return;
        }

        int percent = Random.Range(0, 100);

        ItemData finalItem = null;

        int cumulative = 0;

        //제작 대성공
        cumulative += greatPercent;
        if (percent < cumulative) // 0~9
        {
            Debug.Log("고급 아이템이 제작");
            finalItem = GetUpgradedItem(curItem);
        }

        //제작 성공
        else
        {
            cumulative += successPercent;
            if (percent < cumulative) // 10~59
            {
                Debug.Log("일반 아이템이 제작");
                finalItem = curItem;
            }

            //제작 실패
            else
            {
                cumulative += downPercent;
                if (percent < cumulative) // 60~89
                {
                    Debug.Log("열화 아이템이 제작");
                    finalItem = GetDowngradedItem(curItem);
                }

                //제작 대실패
                else // 나머지
                {
                    Debug.Log("아이템 미생성");
                    finalItem = null; // 아이템 파괴
                }
            }
        }

        if (finalItem != null)
        {
            //player.inventories.Add(finalItem);
            Debug.Log($"{finalItem}이(가) 인벤토리에 추가되었습니다.");
        }
        else
        {
            // 아이템이 null인 경우 (파괴) 인벤토리에 추가하지 않음
            Debug.Log("아이템 파괴로 인해 인벤토리에 추가되지 않았습니다.");
        }
    }

    private ItemData GetUpgradedItem(ItemData currentItemData)
    {
        return ItemDatabaseManager.instance.GetItemByID(currentItemData.ItemId - 1);
    }

    private ItemData GetDowngradedItem(ItemData currentItemData)
    {
        return ItemDatabaseManager.instance.GetItemByID(currentItemData.ItemId + 1);
    }

}