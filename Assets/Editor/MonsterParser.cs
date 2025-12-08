using UnityEngine;
using UnityEditor;
using System.IO;

public class MonsterParser
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

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] values = line.Split(',');
            if (values.Length < 8)
            {
                Debug.LogWarning($"[Line {i}] 필드 부족으로 스킵됨");
                continue;
            }

            try
            {
                int monsterId = int.Parse(values[0]);
                string monsterName = values[1];

                string assetPath = $"{assetFolder}/{monsterId}_{monsterName}.asset";

                MonsterData monster = AssetDatabase.LoadAssetAtPath<MonsterData>(assetPath);
                bool isNew = false;

                if (monster == null)
                {
                    monster = ScriptableObject.CreateInstance<MonsterData>();
                    isNew = true;
                }

                // 기본 데이터 파싱
                monster.Id = monsterId;
                monster.Name = monsterName;
                monster.Hp = int.Parse(values[2]);
                monster.Attack = int.Parse(values[3]);
                monster.AttackRange = float.Parse(values[4]);
                monster.ChaseRange = float.Parse(values[5]);
                monster.MoveSpeed = float.Parse(values[6]);
                monster.AttackCooldown = float.Parse(values[7]);

                // ------------------------------------------------------------
                // 드랍 테이블
                // ------------------------------------------------------------
                monster.DropTable.Clear();

                if (values.Length > 12 && !string.IsNullOrWhiteSpace(values[12]))
                {
                    string[] drops = values[12].Split('|');

                    foreach (string dropStr in drops)
                    {
                        if (int.TryParse(dropStr, out int itemId))
                        {
                            // ? 드랍률 결정: ID의 4번째 자리(인덱스 3)를 보고 판정
                            string idStr = itemId.ToString();

                            float dropRate = 0f;

                            if (idStr.Length >= 4)
                            {
                                char grade = idStr[3];

                                if (grade == '0')      // 일반 재료
                                    dropRate = 30f;
                                else if (grade == '1') // 레어 재료
                                    dropRate = 10f;
                                else
                                    dropRate = 0f;     // 기타
                            }

                            monster.DropTable.Add(new DropItemEntry
                            {
                                ItemId = itemId,
                                DropRate = dropRate
                            });

                            Debug.Log($"[Line {i}] 드랍 등록 → ID={itemId}, Rate={dropRate}");
                        }
                        else
                        {
                            Debug.LogWarning($"[Line {i}] 드랍 ID 파싱 실패: {dropStr} → 0으로 대체");

                            monster.DropTable.Add(new DropItemEntry
                            {
                                ItemId = 0,
                                DropRate = 0f
                            });
                        }
                    }
                }

                if (isNew)
                {
                    AssetDatabase.CreateAsset(monster, assetPath);
                    Debug.Log($"[생성] 몬스터 SO 생성됨 → {assetPath}");
                }
                else
                {
                    EditorUtility.SetDirty(monster);
                    Debug.Log($"[업데이트] 기존 몬스터 SO 갱신됨 → {assetPath}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[Line {i}] 처리 중 에러: {ex.Message}");
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("몬스터 ScriptableObject 생성/업데이트 완료");
    }
}