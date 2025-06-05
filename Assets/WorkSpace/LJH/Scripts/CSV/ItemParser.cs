using UnityEngine;
using UnityEditor;
using System.IO;

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

            if (values.Length < 18)
            {
                Debug.LogWarning($"라인 {i} 필드 부족: {values.Length}개 (필요: 18개)");
                continue;
            }

            try
            {
                ItemData item = ScriptableObject.CreateInstance<ItemData>();

                item.ItemId = int.Parse(values[0]);
                item.IsEquipment = bool.Parse(values[1]);
                item.IsOverlaped = bool.Parse(values[2]);
                item.Parts = (Parts)System.Enum.Parse(typeof(Parts), values[3]);
                item.ItemSet = (ItemSet)System.Enum.Parse(typeof(ItemSet), values[4]);
                item.WeaponType = (WeaponType)System.Enum.Parse(typeof(WeaponType), values[5]);
                item.SubWeaponType = (SubWeaponType)System.Enum.Parse(typeof(SubWeaponType), values[6]);
                item.Tier = int.Parse(values[7]);
                item.Name = values[8];
                item.Strength = int.Parse(values[9]);
                item.Agility = int.Parse(values[10]);
                item.Intelligence = int.Parse(values[11]);
                item.Critical = float.Parse(values[12]);
                item.Hp = int.Parse(values[13]);
                item.Attack = int.Parse(values[14]);
                item.Defence = int.Parse(values[15]);
                item.SellPrice = int.Parse(values[16]);
                item.ItemDescA = values[17];

                string assetPath = $"{assetFolder}/{item.Tier}T_{item.Name}.asset";
                Debug.Log($"ScriptableObject 생성 → {assetPath}");

                AssetDatabase.CreateAsset(item, assetPath);
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