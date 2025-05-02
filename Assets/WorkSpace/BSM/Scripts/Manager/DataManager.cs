using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{

    private string path; 
    private Sprite[] changeSprites;
    
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
            //착용 에셋명 저장
            savePresetData.PresetNames.Add(presets[i].sprite.name);
        }

        string presetJson = JsonUtility.ToJson(savePresetData);
        File.WriteAllText(path, presetJson); 
    }

    public void LoadPresetData(List<SpriteRenderer> playerSprites, PlayerController playerController)
    {
        SetSavePath();
        playerController = playerController;
        
        //저장 데이터
        SavePresetData savePresetData = new SavePresetData();
        
        //데이터 파일 변환
        string loadPresetJson = File.ReadAllText(path);
        savePresetData = JsonUtility.FromJson<SavePresetData>(loadPresetJson);
        
        //착용한 스프라이트의 경로에 있는 리소스로 변경 
        for (int i = 0; i < savePresetData.PresetNames.Count; i++)
        {
            playerSprites[i].sprite = Resources.Load<Sprite>($"Preset/{((CharacterPresetType)i).ToString()}/{savePresetData.PresetNames[i]}");
        }
          
        //웨폰 애니메이션은 제외로 -1
        //노말(무기 장착x) 상태의 애니메이션 스프라이트 이미지 교체
        for (int i = 0; i < savePresetData.PresetNames.Count - 1; i++)
        {
            //TODO: 저장이 잘 안됨
            changeSprites = Resources.LoadAll<Sprite>($"Preset/Animations/Normal/{((NoneEquipStateType)i).ToString()}/{savePresetData.PresetNames[i]}/{i}");
            Debug.Log(changeSprites.Length);
            
            for (int j = 0; j < changeSprites.Length; j++)
            {
                 
                CharacterAnimType category = (CharacterAnimType)i;
                Debug.Log(category);
                
                playerController.SpriteLibraryAsset[i].AddCategoryLabel(changeSprites[j], $"{(category).ToString()}", $"{changeSprites[j].name}");
            } 
        }
        
        
        //TODO: Weapon, weapon 장착 애니메이션 따로 제작 및 추가 필요
        

    }
 
    
}
