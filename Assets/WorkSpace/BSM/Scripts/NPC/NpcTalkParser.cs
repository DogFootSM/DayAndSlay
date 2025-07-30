using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using UnityEngine.Analytics;

[CustomEditor(typeof(NpcTalkParser))]
public class NpcSheetDownButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NpcTalkParser npcTalkParser = (NpcTalkParser)target;

        if (GUILayout.Button("Download Talk Sheet"))
        {
            npcTalkParser.StartDownload(true);
        } 
    }
    
}

public class NpcTalkParser : MonoBehaviour
{
    private string folderPath = "Assets/WorkSpace/BSM/Data/TalkData";
    private const string sheetPath = "https://docs.google.com/spreadsheets/d/1U9wHi3n6M9UnL3CGWfCs82apEzu5Q9xDj1Cmx9YlSH0/export?format=tsv&range=B2:E42&gid=857855805";
    
    private Dictionary<string, List<string>> talkData = new Dictionary<string, List<string>>();
    private Dictionary<string, int> headerMap = new Dictionary<string, int>();
    
    public void StartDownload(bool renameFiles)
    {
        StartCoroutine(TalkDataDownRoutine(renameFiles));
    }

    private IEnumerator TalkDataDownRoutine(bool renameFiles)
    {
        UnityWebRequest uwq = UnityWebRequest.Get(sheetPath);
        yield return uwq.SendWebRequest();

        if (uwq.result == UnityWebRequest.Result.Success)
        {
            CloseAllData();
            talkData.Clear();
            headerMap.Clear();
            
            string tsvText = uwq.downloadHandler.text;

            string[] lines = tsvText.Split('\n');

            string[] headers = lines[0].Split('\t');

            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split('\t');

                if (!talkData.ContainsKey(values[0]))
                {
                    talkData.Add(values[0], new List<string>());
                }
                
                for (int j = 0; j < headers.Length && j < values.Length; j++)
                {
                    talkData[values[0]].Add(values[j]);
                    headerMap[headers[j].Trim()] = j;
                }
                
                ApplyDataToSo(talkData[values[0]], renameFiles);
            }
            

        }
        else
        {
            Debug.Log("시트 다운 실패");
        }

    }

    private void ApplyDataToSo(List<string> talkList, bool renameFiles)
    {
        string talkId = talkList[headerMap["TalkID"]];
        
        GenderType genderType = (GenderType)Enum.Parse(typeof(GenderType), talkList[headerMap["GenderType"]]);
        
        AgeType ageType = (AgeType)Enum.Parse(typeof(AgeType), talkList[headerMap["AgeType"]]); 
        string talk = talkList[headerMap["TalkText"]];
        
        NpcTalkData talkData = new NpcTalkData();

        talkData = CreateTalkData(talkId);

        if (renameFiles)
        {
            RenameScriptableObjectFile(talkData, talkId);
        }
        
        talkData.SetParseTalkData(genderType, ageType, talk);
        EditorUtility.SetDirty(talkData);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void RenameScriptableObjectFile(NpcTalkData talkData, string newFileName)
    {
        string path = AssetDatabase.GetAssetPath(talkData);
        string newPath = Path.GetDirectoryName(path) + "/" + newFileName + ".asset";

        if (path != newPath)
        {
            AssetDatabase.RenameAsset(path, newPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        } 
    }
    
    private NpcTalkData CreateTalkData(string talkId)
    {

        NpcTalkData talkData = ScriptableObject.CreateInstance<NpcTalkData>();

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string path = $"{folderPath}/{talkId}.asset";
        AssetDatabase.CreateAsset(talkData, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return talkData;
    }
    
    private void CloseAllData()
    {
        if (!Directory.Exists(folderPath)) return;

        string[] files = Directory.GetFiles(folderPath, "*.asset");

        foreach (string file in files)
        {
            AssetDatabase.DeleteAsset(file);
        }
         
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    
}
