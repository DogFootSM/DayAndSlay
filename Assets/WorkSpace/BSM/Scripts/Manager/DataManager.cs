using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{

    private string path;

    private void Start()
    {
        path = Path.Combine(Application.dataPath, "presetData.json");
    }


    public void SavePresetData(List<Image> presets)
    {
        SavePresetData savePresetData = new SavePresetData();

        for (int i = 0; i < presets.Count; i++)
        {
            savePresetData.PresetNames.Add(presets[i].sprite.name);
        }

        string presetJson = JsonUtility.ToJson(savePresetData);
        
        File.WriteAllText(path, presetJson); 
    }
    
    
}
