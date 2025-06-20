using UnityEngine;
using UnityEditor;
using System.IO;

public class MonsterCsvImporter
{
    [MenuItem("Tools/Import CSV/Monster Data")]
    public static void ImportMonsterData()
    {
        string csvPath = Application.dataPath + "/WorkSpace/LJH/Scripts/CSV/MonsterData.txt";
        string assetFolder = "Assets/WorkSpace/LJH/Scriptable/Monster/Data";

        if (!File.Exists(csvPath))
        {
            Debug.LogError("CSV 파일이 존재하지 않습니다: " + csvPath);
            return;
        }

        if (!Directory.Exists(assetFolder))
            Directory.CreateDirectory(assetFolder);

        string[] lines = File.ReadAllLines(csvPath);
        Debug.Log($"몬스터 CSV 줄 수: {lines.Length}");

        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더
        {
            Debug.Log($"[Line {i}] 원본: {lines[i]}");

            string[] values = lines[i].Split(',');
            Debug.Log($"[Line {i}] 필드 수: {values.Length}");

            if (values.Length < 8)
            {
                Debug.LogWarning($"[Line {i}] 필드 부족으로 스킵됨");
                continue;
            }

            try
            {
                MonsterData monster = ScriptableObject.CreateInstance<MonsterData>();

                monster.Id = int.Parse(values[0]);
                monster.Name = values[1];
                monster.Hp = int.Parse(values[2]);
                monster.Attack = int.Parse(values[3]);
                monster.AttackRange = float.Parse(values[4]);
                monster.ChaseRange = float.Parse(values[5]);
                monster.MoveSpeed = float.Parse(values[6]);
                monster.AttackCooldown = float.Parse(values[7]);

                Debug.Log($"[Line {i}] 기본 정보 파싱 완료: ID={monster.Id}, Name={monster.Name}");

                // 드랍 테이블이 존재하면 파싱
                if (values.Length > 8 && !string.IsNullOrWhiteSpace(values[8]))
                {
                    string[] drops = values[8].Split('|');

                    foreach (string dropStr in drops)
                    {
                        if (int.TryParse(dropStr, out int itemId))
                        {
                            ItemData itemData = LoadItemById(itemId);
                            if (itemData == null)
                            {
                                Debug.LogWarning($"드랍 아이템 ID {itemId} 찾을 수 없음 (라인 {i + 1})");
                                continue;
                            }

                            float rate = itemData.Parts switch
                            {
                                Parts.RARE_INGREDIANT => 10f,
                                Parts.INGREDIANT => 30f,
                                _ => 0f
                            };

                            Debug.Log($"[Line {i}] 드랍 아이템 ID={itemId}, Rate={rate}");

                            monster.DropTable.Add(new DropItemEntry
                            {
                                ItemId = itemId,
                                DropRate = rate
                            });
                        }
                        else
                        {
                            Debug.LogWarning($"[Line {i}] 드랍 ID 파싱 실패: {dropStr}");
                        }
                    }
                }

                string assetPath = $"{assetFolder}/{monster.Id}_{monster.Name}.asset";

                if (File.Exists(assetPath))
                {
                    Debug.LogWarning($"이미 존재하는 에셋: {assetPath}, 스킵됨");
                    continue;
                }

                AssetDatabase.CreateAsset(monster, assetPath);
                Debug.Log($"에셋 생성됨: {assetPath}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"라인 {i + 1} 처리 중 에러: {ex.Message}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("몬스터 ScriptableObject 생성 완료");
    }

    // 아이템 ID로 ScriptableObject 찾기
    private static ItemData LoadItemById(int id)
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemData");
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);
            if (item != null && item.ItemId == id)
                return item;
        }
        return null;
    }
}