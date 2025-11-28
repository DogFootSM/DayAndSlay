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

            if (values.Length < 26)
            {
                Debug.LogWarning($"라인 {i} 필드 부족: {values.Length}개 (필요: 26개)");
                continue;
            }

            // 오류 추적을 위한 변수 선언. catch 블록에서만 사용됨.
            int parsingFieldIndex = -1;

            try
            {
                ItemData item = ScriptableObject.CreateInstance<ItemData>();
                
                parsingFieldIndex = 0; item.ItemId = int.Parse(values[0]);
                item.ItemImageId = item.ItemId;
                
                parsingFieldIndex = 1; item.IsEquipment = bool.Parse(values[1]);
                parsingFieldIndex = 2; item.IsOverlaped = bool.Parse(values[2]);
                
                parsingFieldIndex = 3; item.Parts = (Parts)System.Enum.Parse(typeof(Parts), values[3]);
                parsingFieldIndex = 4; item.ItemSet = (ItemSet)System.Enum.Parse(typeof(ItemSet), values[4]);
                parsingFieldIndex = 5; item.WeaponType = (WeaponType)System.Enum.Parse(typeof(WeaponType), values[5]);
                parsingFieldIndex = 6; item.SubWeaponType = (SubWeaponType)System.Enum.Parse(typeof(SubWeaponType), values[6]);
                
                parsingFieldIndex = 7; item.Tier = int.Parse(values[7]);
                
                item.Name = values[8]; 
                
                parsingFieldIndex = 9; item.Strength = int.Parse(values[9]);
                parsingFieldIndex = 10; item.Agility = int.Parse(values[10]);
                parsingFieldIndex = 11; item.Intelligence = int.Parse(values[11]);

                parsingFieldIndex = 12; item.Critical = float.Parse(values[12], CultureInfo.InvariantCulture);
                
                parsingFieldIndex = 13; item.Hp = int.Parse(values[13]);
                parsingFieldIndex = 14; item.Attack = int.Parse(values[14]);
                parsingFieldIndex = 15; item.Defence = int.Parse(values[15]);
                
                parsingFieldIndex = 16; item.Range = float.Parse(values[16], CultureInfo.InvariantCulture);
                
                parsingFieldIndex = 17; item.SellPrice = int.Parse(values[17]);
                
                item.ItemDescA = values[18];

                if (item.IsEquipment)
                {
                    int temp;
                    
                    item.ingredients_1 = int.TryParse(values[19], out temp) ? temp : 0;
                    item.ingredients_2 = int.TryParse(values[21], out temp) ? temp : 0;
                    item.ingredients_3 = int.TryParse(values[23], out temp) ? temp : 0;
                    item.ingredients_4 = int.TryParse(values[25], out temp) ? temp : 0;
                    
                    item.Ingredients.Add(item.ingredients_1);
                    item.Ingredients.Add(item.ingredients_2);
                    item.Ingredients.Add(item.ingredients_3);
                    item.Ingredients.Add(item.ingredients_4);
                }

                else
                {
                    item.ingredients_1 = 000000;
                    item.ingredients_2 = 000000;
                    item.ingredients_3 = 000000;
                    item.ingredients_4 = 000000;
                }

                string assetPath = $"{assetFolder}/{item.Name}.asset";
                Debug.Log($"ScriptableObject 생성 → {assetPath}");

                AssetDatabase.CreateAsset(item, assetPath);
            }
            catch (System.Exception ex)
            {
                // 인덱스가 유효한지 확인 후 메시지를 구성합니다.
                string valueDetail = (parsingFieldIndex >= 0 && parsingFieldIndex < values.Length) 
                    ? $"인덱스 [{parsingFieldIndex}] (값: '{values[parsingFieldIndex]}')."
                    : "인덱스 추적 실패.";
                    
                Debug.LogError($"라인 {i} 처리 중 오류 발생: {valueDetail} {ex.Message}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("아이템 데이터 생성 완료");
    }
}