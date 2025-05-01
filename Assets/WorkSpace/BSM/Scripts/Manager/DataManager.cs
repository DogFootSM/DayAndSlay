using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{

    private string path;
  
    
    /// <summary>
    /// 저장 경로 지정
    /// </summary>
    private void SetSavePath()
    {
        path = Path.Combine(Application.dataPath, "presetData.json");
    }
    
    /// <summary>
    /// 선택한 프리셋 json 저장
    /// </summary>
    /// <param name="presets"></param>
    public void SavePresetData(List<Image> presets)
    {
        SetSavePath();
        
        SavePresetData savePresetData = new SavePresetData();

        for (int i = 0; i < presets.Count; i++)
        {
            savePresetData.PresetNames.Add(presets[i].sprite.name);
        }

        string presetJson = JsonUtility.ToJson(savePresetData);
        File.WriteAllText(path, presetJson); 
    }

    public void LoadPresetData()
    {
        SetSavePath();
        
        SavePresetData savePresetData = new SavePresetData();
        
        
        
    }
    
    
}
