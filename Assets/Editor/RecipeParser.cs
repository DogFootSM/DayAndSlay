using UnityEngine;
using UnityEditor;
using System.IO;

public class RecipeParser
{
    [MenuItem("Tools/Import CSV/Item Recipe")]
    public static void ImportItemData()
    {
        string csvPath = Application.dataPath + "/WorkSpace/LJH/Scripts/CSV/ItemRecipe.txt";
        string assetFolder = "Assets/WorkSpace/LJH/Scriptable/Item/RecipeData";

        Debug.Log($"[CSV 파서 시작] 경로: {csvPath}");

        if (!File.Exists(csvPath))
        {
            Debug.LogError("CSV 파일이 존재하지 않습니다: " + csvPath);
            return;
        }

        if (!Directory.Exists(assetFolder))
        {
            Debug.Log($"폴더가 없어 생성 중: {assetFolder}");
            Directory.CreateDirectory(assetFolder);
        }

        string[] lines = File.ReadAllLines(csvPath);
        Debug.Log($"총 줄 수: {lines.Length}");

        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더
        {
            string line = lines[i].Trim();

            if (string.IsNullOrWhiteSpace(line))
            {
                Debug.LogWarning($"라인 {i} 비어있어서 스킵");
                continue;
            }

            string[] values = line.Split(',');

            Debug.Log($"[라인 {i}] 필드 수: {values.Length} / 내용: {line}");

            if (values.Length < 6)
            {
                Debug.LogWarning($"라인 {i} 필드 부족: {values.Length}개 (필요: 6개)");
                continue;
            }

            try
            {
                ItemRecipe recipe = ScriptableObject.CreateInstance<ItemRecipe>();


                recipe.item = int.Parse(values[0]);
                recipe.itemName = values[1];
                recipe.ingredients_1 = int.Parse(values[2]);
                recipe.ingredients_2 = int.Parse(values[3]);
                recipe.ingredients_3 = int.Parse(values[4]);
                recipe.ingredients_4 = int.Parse(values[5]);

                string assetPath = $"{assetFolder}/{recipe.itemName}Recipe.asset";
                Debug.Log($"ScriptableObject 생성 → {assetPath}");

                AssetDatabase.CreateAsset(recipe, assetPath);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"라인 {i} 처리 중 오류 발생: {ex.Message}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("아이템 데이터 생성 완료");
    }
}