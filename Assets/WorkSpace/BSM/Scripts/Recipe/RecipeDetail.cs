using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RecipeDetail : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ItemNameText;
    [SerializeField] private Image ItemImage;
    [SerializeField] private GameObject materialPrefab;
    [SerializeField] private GameObject materialParent;
    [SerializeField] private GameObject detailPanel;
    
    private Queue<GameObject> materialPool = new Queue<GameObject>();
    private List<(int,int)> validMaterialIds = new List<(int,int)>();
    private ItemDatabaseManager itemDatabase => ItemDatabaseManager.instance;
    
    private void Awake()
    {
        Init();
    }

    private void OnDisable()
    {
        ItemNameText.gameObject.SetActive(false);
        ItemImage.gameObject.SetActive(false);
        ReturnRecipeMaterialPool();
    }

    /// <summary>
    /// 재료 레시피 풀 초기화
    /// </summary>
    private void Init()
    {
        if (materialPool.Count == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                GameObject instance = Instantiate(materialPrefab, transform.position, Quaternion.identity, materialParent.transform);
                instance.SetActive(false);
                
                materialPool.Enqueue(instance);
            } 
        }
    }

    public void SetActiveDetailPanel(bool isActive)
    {
        if (isActive)
        {
            ReturnRecipeMaterialPool();
        }
        
        detailPanel.SetActive(isActive);
    }
    
    /// <summary>
    /// 선택한 레시피의 아이템 정보 업데이트
    /// </summary>
    /// <param name="itemData"></param>
    public void UpdateRecipeDetail(ItemData itemData)
    {
        validMaterialIds.Clear();
        ItemNameText.gameObject.SetActive(true);
        ItemImage.gameObject.SetActive(true);
        ItemNameText.text = itemData.Name;
        ItemImage.sprite = itemData.ItemImage;

        if (itemData.ingredients_1 != 0)
        {
            validMaterialIds.Add((itemData.ingredients_1, itemData.ingredients_1_Count));
        }
        
        if (itemData.ingredients_2 != 0)
        {
            validMaterialIds.Add((itemData.ingredients_2, itemData.ingredients_2_Count));
        }
        
        if (itemData.ingredients_3 != 0)
        {
            validMaterialIds.Add((itemData.ingredients_3, itemData.ingredients_3_Count));
        }
        
        if (itemData.ingredients_4 != 0)
        {
            validMaterialIds.Add((itemData.ingredients_4, itemData.ingredients_4_Count));
        } 
        
        SetRecipeMaterial(validMaterialIds.Count);
    }
     
    /// <summary>
    /// 필요 재료 아이템 정보 설정
    /// </summary>
    /// <param name="validCount"></param>
    private void SetRecipeMaterial(int validCount)
    {
        for (int i = 0; i < validCount; i++)
        {
            int requestMaterialId = validMaterialIds[i].Item1;

            ItemData recipeMaterialData = itemDatabase.GetItemByID(requestMaterialId);

            RecipeMaterial recipeMaterial = GetRecipeMaterialPool().GetComponent<RecipeMaterial>();
            recipeMaterial.SetMaterialData(recipeMaterialData, validMaterialIds[i].Item2);
            recipeMaterial.gameObject.SetActive(true);
        } 
    }

    /// <summary>
    /// 재료 아이템 풀에서 아이템 오브젝트를 반환
    /// </summary>
    /// <returns></returns>
    private GameObject GetRecipeMaterialPool()
    {
        GameObject instance;

        if (materialPool.Count > 0)
        {
            instance = materialPool.Dequeue();    
        }
        else
        {
            instance = Instantiate(materialPrefab, transform.position, Quaternion.identity, materialParent.transform);
            instance.SetActive(false);
        }

        return instance;
    }

    /// <summary>
    /// 선택한 레시피 변경 시 재료 아이템 풀에 반환
    /// </summary>
    private void ReturnRecipeMaterialPool()
    {
        for (int i = 0; i < materialParent.transform.childCount; i++)
        {
            GameObject child = materialParent.transform.GetChild(i).gameObject;
            
            if (child.activeSelf)
            {
                materialPool.Enqueue(child);
                child.SetActive(false);
            } 
        } 
    }
    
}
