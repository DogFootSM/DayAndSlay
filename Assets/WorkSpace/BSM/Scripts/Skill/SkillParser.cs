using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

[CustomEditor(typeof(SkillParser))]
public class SheetDownButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        SkillParser skillParser = (SkillParser)target;

        if (GUILayout.Button("Download Skill Sheet"))
        {
            skillParser.StartDownload(true);
        }
        
    }
}
 
public class SkillParser : MonoBehaviour
{
    [SerializeField] private List<SkillData> skillDatas;

    private string folderPath = "Assets/WorkSpace/BSM/Data/SkillData";
    private Dictionary<string, List<string>> rowData = new Dictionary<string, List<string>>();
    private Dictionary<string, int> headerMap = new Dictionary<string, int>();
    
    //구글 시트 주소
    private const string skillDataUrlPath = "https://docs.google.com/spreadsheets/d/1qy-UZH2OCVpJoAEroGsVQxFnvbZVeYt8-P1trYRojOk/export?format=tsv&range=B4:V60&gid=117661071";

    public void StartDownload(bool renameFiles)
    {
        StartCoroutine(DownloadSkillDataRoutine(renameFiles));
    }

    private IEnumerator DownloadSkillDataRoutine(bool renameFiles)
    {
        UnityWebRequest req = UnityWebRequest.Get(skillDataUrlPath);
        yield return req.SendWebRequest();
        
        //Request 결과를 정상적으로 받아왔을 경우
        if (req.result == UnityWebRequest.Result.Success)
        {
            ClearAllData();
            skillDatas.Clear();
            rowData.Clear();
            
            string tsvText = req.downloadHandler.text;
            
            string[] lines = tsvText.Split('\n');

            string[] headers = lines[0].Split('\t');

            for (int i = 1; i < lines.Length; i++)
            {
                string[] value = lines[i].Split('\t');
            
                if (!rowData.ContainsKey(value[0]))
                {
                    rowData.Add(value[0], new List<string>());
                }
                
                for (int j = 0; j < headers.Length && j < value.Length; j++)
                {
                    rowData[value[0]].Add(value[j]);
                    headerMap[headers[j].Trim()] = j; 
                }
                
                ApplySoData(rowData[value[0]], renameFiles);
            }
        }
        else
        {
            Debug.LogError("데이터 실패 :" + req.error);
        }
    }

    private void ApplySoData(List<string> rowData, bool renameFiles)
    {
        string skillId = rowData[headerMap["ID"]] ?? string.Empty;
              
        string skillName = rowData[headerMap["Name"]] ?? string.Empty;
            
        string skillDescription= rowData[headerMap["Description"]] ?? string.Empty;

        string skillEffect = rowData[headerMap["Effect"]] ?? string.Empty;

        float skillCoolDown = 0f;
        float.TryParse(rowData[headerMap["CoolDown"]], out skillCoolDown);

        int skillMaxLevel = 0;
        int.TryParse(rowData[headerMap["MaxLevel"]], out skillMaxLevel);    
         
        float skillDamage = 0f;
        float.TryParse(rowData[headerMap["Damage"]], out skillDamage);    
        
        float skillDamageIncreaseRate = 0f;
        float.TryParse(rowData[headerMap["DamageIncreaseRate"]], out skillDamageIncreaseRate);
        
        string skillIcon = rowData[headerMap["Icon"]];

        int weaponType = 0;
        int.TryParse(rowData[headerMap["WeaponType"]], out weaponType);    
        
        int active = 0;
        int.TryParse(rowData[headerMap["isActive"]], out active);
        
        float skillDelay = 0f;
        float.TryParse(rowData[headerMap["Delay"]], out skillDelay);    
        
        float skillDelayDecreaseRate = 0f;
        float.TryParse(rowData[headerMap["DecreaseDelayRate"]], out skillDelayDecreaseRate);    
         
        float skillRange = 0f;
        float.TryParse(rowData[headerMap["Range"]], out skillRange);    
        
        float castingTime = 0f;
        float.TryParse(rowData[headerMap["CastingTime"]], out castingTime);
        
        int skillHitCount = 0;
        int.TryParse(rowData[headerMap["HitCount"]], out skillHitCount);

        float buffDuration = 0;
        float.TryParse(rowData[headerMap["BuffDuration"]], out buffDuration);

        float deBuffDuration = 0f;
        float.TryParse(rowData[headerMap["DebuffDuration"]], out deBuffDuration);

        float skillRadiusRange = 0f;
        float.TryParse(rowData[headerMap["Radius"]], out skillRadiusRange);

        string prerequisiteSkillsId = rowData[headerMap["PrecedingSkill"]] ?? string.Empty;

        float skillAbilityValue = 0f;
        float.TryParse(rowData[headerMap["SkillAbilityValue"]], out skillAbilityValue);
        
        float skillAbilityFactor = 0f;
        float.TryParse(rowData[headerMap["SkillAbilityFactor"]], out skillAbilityFactor);

        int detectedCount = 0;
        int.TryParse(rowData[headerMap["DetectedCount"]], out detectedCount);
        
        SkillData skillData = new SkillData();
        skillData = CreateSkillData(skillId);
        
        //파일 이름을 변경 여부
        if (renameFiles)
        {
            RenameScriptableObjectFile(skillData, skillId);
        }
        
        //추출한 데이터 값으로 SO 데이터 설정
        skillData.SetData(
            skillId, skillName, skillDescription, skillEffect, 
            skillCoolDown, skillMaxLevel, skillDamage, skillDamageIncreaseRate,
            skillIcon, (WeaponType)weaponType, skillDelay, 
            skillDelayDecreaseRate, skillRange, castingTime, skillHitCount, 
            active, buffDuration, deBuffDuration, prerequisiteSkillsId, 
            skillRadiusRange, skillAbilityValue, skillAbilityFactor, detectedCount);
        
        EditorUtility.SetDirty(skillData);
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
    /// <summary>
    /// SO 데이터 파일 이름 수정
    /// </summary>
    /// <param name="skillData"></param>
    /// <param name="newFileName"></param>
    private void RenameScriptableObjectFile(SkillData skillData, string newFileName)
    {
        string path = AssetDatabase.GetAssetPath(skillData);
        string newPath = Path.GetDirectoryName(path) + "/" + newFileName + ".asset";

        if (path != newPath)
        {
            AssetDatabase.RenameAsset(path, newPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        } 
    }

    /// <summary>
    /// GoogleSheets 파싱 시 기존 데이터들 삭제
    /// </summary>
    private void ClearAllData()
    {
        if (!Directory.Exists(folderPath))
        {
            Debug.Log("폴더 없음");
            return;
        }        
        
        string[] files = Directory.GetFiles(folderPath, "*.asset");

        foreach (string file in files)
        {
            AssetDatabase.DeleteAsset(file); 
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 파싱한 데이터 정보들을 설정하기 위한 SO 데이터 생성
    /// </summary>
    /// <param name="skillId"></param>
    /// <returns></returns>
    private SkillData CreateSkillData(string skillId)
    {
        SkillData skillData = ScriptableObject.CreateInstance<SkillData>();

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string assetPath = $"{folderPath}/{skillId}.asset";
        AssetDatabase.CreateAsset(skillData, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return skillData;
    }
    
}
