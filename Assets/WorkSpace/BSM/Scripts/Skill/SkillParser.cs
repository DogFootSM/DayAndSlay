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
    private const string skillDataUrlPath = "https://docs.google.com/spreadsheets/d/1qy-UZH2OCVpJoAEroGsVQxFnvbZVeYt8-P1trYRojOk/export?format=tsv&range=B4:S10&gid=117661071";

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

    /// <summary>
    /// 파싱한 구글 시트 정보 Json으로 변경
    /// </summary>
    /// <param name="tsvText"></param>
    /// <returns></returns>
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

    /// <summary>
    /// JArray에 추가된 파싱 데이터 값 추출 후 SO 데이터에 설정
    /// </summary>
    /// <param name="jsonData"></param>
    /// <param name="renameFiles"></param>
    private void ApplyDataToSO(JArray jsonData, bool renameFiles)
    {
        ClearAllData();
        skillDatas.Clear();

        for (int i = 0; i < jsonData.Count; i++)
        {
            //스킬 데이터 값 추출
            JObject row = (JObject)jsonData[i];
                
            string skillId = row.Value<string>("ID") ?? string.Empty;
              
            string skillName = row.Value<string?>("Name") ?? string.Empty;
            
            string skillDescription= row.Value<string?>("Description") ?? string.Empty;

            string skillEffect = row.Value<string?>("Effect") ?? string.Empty;
            
            float skillCoolDown = row.Value<float?>("CoolDown") ?? 0f;

            int skillMaxLevel = row.Value<int?>("MaxLevel") ?? 0;
            
            float skillDamage = row.Value<float?>("Damage") ?? 0f;
            
            float skillDamageIncreaseRate = row.Value<float?>("DamageIncreaseRate") ?? 0f;
            
            string skillIcon = row.Value<string>("Icon") ?? string.Empty;
            
            int weaponType = row.Value<int?>("WeaponType") ?? 4;
            
            float skillDelay = row.Value<float?>("Delay") ?? 0f;
            
            float skillDelayDecreaseRate = row.Value<float?>("DecreaseDelayRate") ?? 0f;
            
            float skillRange = row.Value<float?>("Range") ?? 0f;
            
            float castingTime = row.Value<float?>("CastingTime") ?? 0f;
            
            int skillHitCount = row.Value<int?>("HitCount") ?? 0;

            int active = row.Value<int?>("isActive") ?? 0;
            
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
            
            //파일 이름을 변경 여부
            if (renameFiles)
            {
                RenameScriptableObjectFile(skillData, skillId);
            }
            
            //추출한 데이터 값으로 SO 데이터 설정
            skillData.SetData(skillId, skillName, skillDescription, skillEffect, skillCoolDown, skillMaxLevel, skillDamage, skillDamageIncreaseRate,
                skillIcon, (WeaponType)weaponType, skillDelay, skillDelayDecreaseRate, skillRange, castingTime,skillHitCount, active);
            EditorUtility.SetDirty(skillData);
        }
        
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
