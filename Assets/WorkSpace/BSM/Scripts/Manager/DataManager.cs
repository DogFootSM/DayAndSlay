using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{

    private string path;
    private PlayerController playerController;

    public Sprite[] test;
    
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

    public void LoadPresetData(List<SpriteRenderer> playerSprites, PlayerController playerController)
    {
        SetSavePath();
        this.playerController = playerController;
        
        SavePresetData savePresetData = new SavePresetData();

        string loadPresetJson = File.ReadAllText(path);
        savePresetData = JsonUtility.FromJson<SavePresetData>(loadPresetJson);

        for (int i = 0; i < savePresetData.PresetNames.Count; i++)
        {
            Debug.Log(savePresetData.PresetNames[i]);
            
            playerSprites[i].sprite = Resources.Load<Sprite>($"Preset/{((CharacterPresetType)i).ToString()}/{savePresetData.PresetNames[i]}");
            
        }
        
        this.playerController.SpriteLibraryAsset.AddCategoryLabel(playerSprites[1].sprite, "IDLE", "Idle_0");

        test = Resources.LoadAll<Sprite>($"Preset/Animations/Body/{savePresetData.PresetNames[1]}/1");
 
        
        
        


    }
    
    
}
