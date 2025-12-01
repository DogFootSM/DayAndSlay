using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField] private EquipCreateButton createButton;
    [SerializeField] private PreviewUI previewUi;
    public ItemData itemData;

    private Button createButton_Button;


    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(Tap_ItemButton);
        createButton_Button = createButton.GetComponent<Button>();
        createButton_Button.onClick.AddListener(CreateButtonIsInteractable);
    }

    public void SetButtonItem(ItemData itemData)
    {
        this.itemData = itemData;
        GetComponentInChildren<TextMeshProUGUI>().text = itemData.name;
    }

    public ItemData GetButtonItem() => itemData;

    public void Tap_ItemButton()
    {
        previewUi.SetPreview(itemData);
        createButton.SetCurSelectedItem(itemData);
        
        CreateButtonIsInteractable();
        

    }

    public void CreateButtonIsInteractable()
    {
        createButton_Button.interactable = HasIngredientCheck();
    }

    /// <summary>
    /// 인벤토리에 재료 있는지 확인해주는 메서드
    /// </summary>
    /// <returns></returns>
    private bool HasIngredientCheck()
    {
        List<InventorySlot> slotList = new List<InventorySlot>();

        if (itemData.ingredients_1 != 0)
            slotList.Add(
                createButton.inventory.HasRequiredMaterials(itemData.ingredients_1, itemData.ingredients_1_Count));
        if (itemData.ingredients_2 != 0)
            slotList.Add(
                createButton.inventory.HasRequiredMaterials(itemData.ingredients_2, itemData.ingredients_2_Count));
        if (itemData.ingredients_3 != 0)
            slotList.Add(
                createButton.inventory.HasRequiredMaterials(itemData.ingredients_3, itemData.ingredients_3_Count));
        if (itemData.ingredients_4 != 0)
            slotList.Add(
                createButton.inventory.HasRequiredMaterials(itemData.ingredients_4, itemData.ingredients_4_Count));

        //슬롯중에 널이 있으면 false 반환해야함
        //근데 재료칸이 원래 비어있는 경우 == 카운트가 0인 경우 에는 원래 널임
        //인그레 카운트가 0인 경우는 원래 그 슬롯이 널이 나옴

        createButton.SetSlotList(slotList);

        foreach (InventorySlot slot in slotList)
        {

            if (slot == null)
            {
                return false;
            }
        }

        return  true;
    }
}


