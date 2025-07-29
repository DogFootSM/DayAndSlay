using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.Serialization;

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
    [FormerlySerializedAs("skillData")] [SerializeField] private List<SkillData> skillDatas;

    private string folderPath = "Assets/WorkSpace/BSM/Data/SkillData";
    
    //구글 시트 주소
    private const string skillDataUrlPath = "https://docs.google.com/spreadsheets/d/1qy-UZH2OCVpJoAEroGsVQxFnvbZVeYt8-P1trYRojOk/export?format=tsv&range=B4:S44&gid=117661071";
    
    private void Awake()
    {
        StartDownload(false);
    }

    private void Start()
    {
        Invoke(nameof(SetActiveDisable), 10f);
    }

    private void SetActiveDisable()
    {
        gameObject.SetActive(false);
    }
    
    public void StartDownload(bool renameFiles)
    {
        StartCoroutine(DownloadSkillDataRoutine(renameFiles));
    }

    private IEnumerator DownloadSkillDataRoutine(bool renameFiles)
    {
        UnityWebRequest req = UnityWebRequest.Get(skillDataUrlPath);
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.Success)
        {
            string tsvText = req.downloadHandler.text;
            string json = ConvertTsvToJson(tsvText);

            JArray jsonData = JArray.Parse(json);
            ApplyDataToSO(jsonData, renameFiles);

        }
        else
        {
            Debug.LogError("데이터 실패 :" + req.error);
        }
    }

    private string ConvertTsvToJson(string tsvText)
    {
        string[] lines = tsvText.Split('\n');

        if (lines.Length < 2) return "[]";
        
        string[] headers = lines[0].Split('\t');
        JArray jsonArray = new JArray();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split('\t');
            JObject jObject = new JObject();

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                string value = values[j].Trim();
                jObject[headers[j].Trim()] = value;
            }
            
            jsonArray.Add(jObject);
        }

        return jsonArray.ToString();
    }

    private void ApplyDataToSO(JArray jsonData, bool renameFiles)
    {
        ClearAllData();
        skillDatas.Clear();

        for (int i = 0; i < jsonData.Count; i++)
        {
            JObject row = (JObject)jsonData[i];
                
            string skillId = "";
            skillId = row["ID"].ToString();
              
            string skillName = "";
            skillName = row["Name"].ToString();
            
            string skillDescription= "";
            skillDescription = row["Description"].ToString();
            
            string skillEffect = "";
            skillEffect = row["Effect"].ToString();
            
            float skillCoolDown = 0f;
            float.TryParse(row["CoolDown"].ToString(), out skillCoolDown);
            
            int skillMaxLevel = 0;
            int.TryParse(row["MaxLevel"].ToString(), out skillMaxLevel);
            
            float skillDamage = 0f;
            float.TryParse(row["Damage"].ToString(), out skillDamage);
            
            float skillDamageIncreaseRate = 0f;
            float.TryParse(row["DamageIncreaseRate"].ToString(), out skillDamageIncreaseRate);
            
            string skillIcon = "";
            skillIcon = row["Icon"].ToString();

            int weaponType = 4;
            int.TryParse(row["WeaponType"].ToString(), out weaponType);
            
            
            float skillDelay = 0f;
            float.TryParse(row["Delay"].ToString(), out skillDelay);
            
            float skillDelayDecreaseRate = 0f;
            float.TryParse(row["DecreaseDelayRate"].ToString(), out skillDelayDecreaseRate);
            
            float skillRange = 0f;
            float.TryParse(row["Range"].ToString(), out skillRange);
            
            float castingTime = 0f;
            float.TryParse(row["CastingTime"].ToString(), out castingTime);
            
            int skillHitCount = 0;
            int.TryParse(row["HitCount"].ToString(), out skillHitCount);
            
            SkillData skillData = new SkillData();

            if (i < this.skillDatas.Count)
            {
                skillData = this.skillDatas[i];
            }
            else
            {
                skillData = CreateSkillData(skillId);
                skillDatas.Add(skillData);
            }

            if (renameFiles)
            {
                RenameScriptableObjectFile(skillData, skillId);
            }
                
            skillData.SetData(skillId, skillName, skillDescription, skillEffect, skillCoolDown, skillMaxLevel, skillDamage, skillDamageIncreaseRate,
                skillIcon, (WeaponType)weaponType, skillDelay, skillDelayDecreaseRate, skillRange, castingTime,skillHitCount);
            EditorUtility.SetDirty(skillData);
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    
    }

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
            Debug.Log($"파일 삭제 :{file}");
        }
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

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
