using UnityEngine;
using UnityEditor;
using System.IO;

public class IngrediantParser
{
    [MenuItem("Tools/Import CSV/IngreDiant (Update/Create)")]
    public static void ImportItemData()
    {
        // 파일 및 폴더 경로 설정
        string csvPath = Application.dataPath + "/WorkSpace/LJH/Scripts/CSV/Ingrediant.txt";
        string assetFolder = "Assets/WorkSpace/LJH/Scriptable/Item/Ingrediant";

        Debug.Log($"[CSV 파서 시작] 경로: {csvPath}");

        // 1. CSV 파일 존재 여부 확인
        if (!File.Exists(csvPath))
        {
            Debug.LogError("CSV 파일이 존재하지 않습니다: " + csvPath);
            return;
        }

        // 2. 에셋 폴더 존재 여부 확인 및 생성
        if (!Directory.Exists(assetFolder))
        {
            Debug.Log($"폴더가 없어 생성 중: {assetFolder}");
            Directory.CreateDirectory(assetFolder);
        }

        string[] lines = File.ReadAllLines(csvPath);
        Debug.Log($"총 줄 수: {lines.Length}");

        // 3. 데이터 파싱 및 SO 업데이트/생성
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

            if (values.Length < 18)
            {
                Debug.LogWarning($"라인 {i} 필드 부족: {values.Length}개 (필요: 18개)");
                continue;
            }

            try
            {
                // **수정된 로직 시작**
                string itemName = values[8]; // 아이템 이름은 인덱스 8
                string assetPath = $"{assetFolder}/{itemName}.asset";
                
                ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);
                bool isNew = (item == null);

                if (isNew)
                {
                    // 에셋이 존재하지 않으면 새로 생성
                    item = ScriptableObject.CreateInstance<ItemData>();
                    Debug.Log($"[라인 {i}] ScriptableObject 새로 생성 → {assetPath}");
                }
                else
                {
                    // 에셋이 존재하면 업데이트
                    Debug.Log($"[라인 {i}] 기존 ScriptableObject 업데이트 → {assetPath}");
                }
                // **수정된 로직 끝**


                // 데이터 할당 (생성/업데이트 모두 동일)
                item.ItemId = int.Parse(values[0]);
                item.IsEquipment = bool.Parse(values[1]);
                item.IsOverlaped = bool.Parse(values[2]);
                item.Parts = (Parts)System.Enum.Parse(typeof(Parts), values[3]);
                item.ItemSet = (ItemSet)System.Enum.Parse(typeof(ItemSet), values[4]);
                item.WeaponType = (WeaponType)System.Enum.Parse(typeof(WeaponType), values[5]);
                item.SubWeaponType = (SubWeaponType)System.Enum.Parse(typeof(SubWeaponType), values[6]);
                item.Tier = int.Parse(values[7]);
                item.Name = itemName; // 이미 위에서 가져온 이름 사용
                item.Strength = int.Parse(values[9]);
                item.Agility = int.Parse(values[10]);
                item.Intelligence = int.Parse(values[11]);
                item.Critical = float.Parse(values[12]);
                item.Hp = int.Parse(values[13]);
                item.Attack = int.Parse(values[14]);
                item.Defence = int.Parse(values[15]);
                item.Range = int.Parse(values[16]);
                item.SellPrice = int.Parse(values[17]);
                item.ItemDescA = values[18];
                
                // 에셋 저장
                if (isNew)
                {
                    // 새로 생성된 경우에만 에셋 생성 함수 호출
                    AssetDatabase.CreateAsset(item, assetPath);
                }
                else
                {
                    // 기존 에셋이 업데이트된 경우 변경 사항을 저장하도록 알려줌
                    EditorUtility.SetDirty(item);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"라인 {i} 처리 중 오류 발생: {ex.Message}");
            }
        }

        // 4. 에셋 데이터베이스 저장 및 새로고침
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("아이템 데이터 업데이트/생성 완료");
    }
}