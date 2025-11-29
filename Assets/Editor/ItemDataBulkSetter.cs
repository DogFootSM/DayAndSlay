using UnityEditor;
using UnityEngine;

public class ItemDataBulkSetter : Editor
{
    // 에디터 상단 메뉴에 [Custom Tools/Set ItemData Values to One] 버튼을 추가합니다.
    [MenuItem("Custom Tools/ScriptableObject/Set All ItemData Values to One")]
    public static void SetItemDataValuesToOne()
    {
        // 1. 목표 경로 설정 (경로: Assets/WorkSpace/LJH/Scriptable/Item/data)
        // AssetDatabase.FindAssets는 프로젝트 전체를 검색하므로, 경로를 제한하여 속도를 높일 수 있습니다.
        string targetFolder = "Assets/WorkSpace/LJH/Scriptable/Item/data";
        
        // 해당 폴더 내의 모든 ItemData 타입 에셋을 찾습니다.
        string[] guids = AssetDatabase.FindAssets("t:ItemData", new[] { targetFolder });
        
        int count = 0;
        
            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                
                // ItemData 에셋을 불러옵니다.
                ItemData itemData = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);

                if (itemData != null)
                {
                    // 3. 변수 값 변경 (예시: someValue와 someOtherValue를 1로 설정)
                    itemData.ingredients_1_Count = 1;
                    itemData.ingredients_2_Count = 1;
                    itemData.ingredients_3_Count = 1;
                    itemData.ingredients_4_Count = 1;

                    // 변경된 에셋을 Unity에게 알립니다. (저장될 준비 완료)
                    EditorUtility.SetDirty(itemData);
                    
                    count++;
                }
            }
        
        // 4. 모든 변경 사항을 디스크에 영구적으로 저장합니다.
        AssetDatabase.SaveAssets(); 
        AssetDatabase.Refresh();

        Debug.Log($"[Bulk Set Success] 총 {count}개의 ItemData 에셋의 'someValue' 값이 1로 설정되었습니다.");
    }
}