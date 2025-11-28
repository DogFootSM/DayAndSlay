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

        
        if (HasIngredientCheck())
        {
            createButton_Button.interactable = true;
        }
        else
        {
            createButton_Button.interactable = false;    
        }

    }

    //인수에서 인벤토리 집어넣어줘야할듯
    private bool HasIngredientCheck()
    {
        return createButton.inventory.HasRequiredMaterials(itemData.ingredients_1, itemData.ingredients_1_Count) &&
               createButton.inventory.HasRequiredMaterials(itemData.ingredients_2, itemData.ingredients_2_Count) &&
               createButton.inventory.HasRequiredMaterials(itemData.ingredients_3, itemData.ingredients_3_Count) &&
               createButton.inventory.HasRequiredMaterials(itemData.ingredients_4, itemData.ingredients_4_Count);
    }


}
