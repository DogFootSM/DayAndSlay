using UnityEngine;
using UnityEditor;
using System.IO;
using System.Globalization;

public class ItemParser
{
    [MenuItem("Tools/Import CSV/Item Data")]
    public static void ImportItemData()
    {
        string csvPath = Application.dataPath + "/WorkSpace/LJH/Scripts/CSV/ItemData.txt";
        string assetFolder = "Assets/WorkSpace/LJH/Scriptable/Item/data";

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

            if (values.Length < 27)
            {
                Debug.LogWarning($"라인 {i} 필드 부족: {values.Length}개 (필요: 27개)");
                continue;
            }

            int parsingFieldIndex = -1;

            try
            {
                // ───────────────────────────────────────
                // ① 기존 아이템 검사 (업데이트 모드)
                // ───────────────────────────────────────
                ItemData item = null;

                string assetPath = $"{assetFolder}/{values[9]}.asset"; // Name 기준
                item = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);

                bool isNew = false;
                if (item == null)
                {
                    // 기존 아이템이 없어서 새로 생성해야 함
                    item = ScriptableObject.CreateInstance<ItemData>();
                    isNew = true;
                }

                // ───────────────────────────────────────
                // ② CSV 데이터 파싱 및 값 세팅
                // ───────────────────────────────────────

                parsingFieldIndex = 0; item.ItemId = int.Parse(values[0]);
                item.ItemImageId = item.ItemId;

                parsingFieldIndex = 1; item.IsEquipment = bool.Parse(values[1]);
                parsingFieldIndex = 2; item.IsOverlaped = bool.Parse(values[2]);

                parsingFieldIndex = 3; item.Parts = (Parts)System.Enum.Parse(typeof(Parts), values[3]);
                parsingFieldIndex = 4; item.ItemSet = (ItemSet)System.Enum.Parse(typeof(ItemSet), values[4]);
                parsingFieldIndex = 5; item.WeaponType = (WeaponType)System.Enum.Parse(typeof(WeaponType), values[5]);
                parsingFieldIndex = 6; item.SubWeaponType = (SubWeaponType)System.Enum.Parse(typeof(SubWeaponType), values[6]);
                parsingFieldIndex = 7; item.MaterialType = (MaterialType)System.Enum.Parse(typeof(MaterialType), values[7]);

                parsingFieldIndex = 8; item.Tier = int.Parse(values[8]);

                item.Name = values[9];

                parsingFieldIndex = 10; item.Strength = int.Parse(values[10]);
                parsingFieldIndex = 11; item.Agility = int.Parse(values[11]);
                parsingFieldIndex = 12; item.Intelligence = int.Parse(values[12]);

                parsingFieldIndex = 13; item.Critical = float.Parse(values[13], CultureInfo.InvariantCulture);

                parsingFieldIndex = 14; item.Hp = int.Parse(values[14]);
                parsingFieldIndex = 15; item.Attack = int.Parse(values[15]);
                parsingFieldIndex = 16; item.Defence = int.Parse(values[16]);

                parsingFieldIndex = 17; item.Range = float.Parse(values[17], CultureInfo.InvariantCulture);

                parsingFieldIndex = 18; item.SellPrice = int.Parse(values[18]);

                item.ItemDescA = values[19];

                // 재료 초기화
                item.Ingredients.Clear();

                if (item.IsEquipment)
                {
                    int temp;

                    parsingFieldIndex = 20; item.ingredients_1 = int.TryParse(values[20], out temp) ? temp : 0;
                    parsingFieldIndex = 21; item.ingredients_1_Count = int.TryParse(values[21], out temp) ? temp : 0;

                    parsingFieldIndex = 22; item.ingredients_2 = int.TryParse(values[22], out temp) ? temp : 0;
                    parsingFieldIndex = 23; item.ingredients_2_Count = int.TryParse(values[23], out temp) ? temp : 0;

                    parsingFieldIndex = 24; item.ingredients_3 = int.TryParse(values[24], out temp) ? temp : 0;
                    parsingFieldIndex = 25; item.ingredients_3_Count = int.TryParse(values[25], out temp) ? temp : 0;

                    parsingFieldIndex = 26; item.ingredients_4 = int.TryParse(values[26], out temp) ? temp : 0;
                    parsingFieldIndex = 27; item.ingredients_4_Count = int.TryParse(values[27], out temp) ? temp : 0;

                    item.Ingredients.Add(item.ingredients_1);
                    item.Ingredients.Add(item.ingredients_2);
                    item.Ingredients.Add(item.ingredients_3);
                    item.Ingredients.Add(item.ingredients_4);
                }
                else
                {
                    item.ingredients_1 = 0;
                    item.ingredients_2 = 0;
                    item.ingredients_3 = 0;
                    item.ingredients_4 = 0;
                }

                // ───────────────────────────────────────
                // ③ 새 아이템이면 CreateAsset 실행
                // ───────────────────────────────────────
                if (isNew)
                {
                    Debug.Log($"[새 자산 생성] {assetPath}");
                    AssetDatabase.CreateAsset(item, assetPath);
                }
                else
                {
                    Debug.Log($"[기존 아이템 업데이트] {assetPath}");
                    EditorUtility.SetDirty(item);
                }
            }
            catch (System.Exception ex)
            {
                string valueDetail =
                    (parsingFieldIndex >= 0 && parsingFieldIndex < values.Length)
                    ? $"인덱스 [{parsingFieldIndex}] (값: '{values[parsingFieldIndex]}')."
                    : "인덱스 추적 실패.";

                Debug.LogError($"라인 {i} 처리 중 오류 발생: {valueDetail} {ex.Message}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("아이템 데이터 생성/업데이트 완료");
    }
}