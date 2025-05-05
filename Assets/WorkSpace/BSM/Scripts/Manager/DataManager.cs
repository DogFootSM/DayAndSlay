using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{

    private string path; 
    private Sprite[][] changeSprites = new Sprite[(int)NoneEquipStateType.SIZE][];
    
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
    public void SavePresetData(List<Image> presets, int WeaponType)
    {
        SetSavePath();
        
        SavePresetData savePresetData = new SavePresetData();

        for (int i = 0; i < presets.Count; i++)
        {
            //착용 에셋명 저장
            savePresetData.PresetNames.Add(presets[i].sprite.name);
        }
        
        //현재 무기 타입 저장
        savePresetData.CharacterWeaponType = WeaponType;
        
        string presetJson = JsonUtility.ToJson(savePresetData);
        File.WriteAllText(path, presetJson); 
    }

    public void LoadPresetData(List<SpriteRenderer> playerSprites, PlayerController playerController)
    {
        SetSavePath();
        
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

        
        //노말(무기 장착x) 상태의 애니메이션 스프라이트 이미지 교체
        for (int i = 0; i < playerController.SpriteLibraryAsset.Length; i++)
        {
            
            //타입의 사이즈만큼 반복
            for (int j = 0; j < (int)CharacterNormalAnimType.SIZE; j++)
            {  
                // 경로 : 어느 부위 - 에셋 이름 - 캐릭터 상태
                changeSprites[i] = Resources.LoadAll<Sprite>($"Preset/Animations/Normal/" +
                                                             $"{((NoneEquipStateType)i).ToString()}/" +
                                                             $"{savePresetData.PresetNames[i]}/{j}");
                
                //찾아온 애셋의 개수만큼 반복
                for (int k = 0; k < changeSprites[i].Length; k++)
                {
                    playerController.SpriteLibraryAsset[i].AddCategoryLabel(changeSprites[i][k], 
                        ((CharacterNormalAnimType)j).ToString(),
                        $"{((CharacterNormalAnimType)j) + "_" + k}");
                }
                 
            } 
        }
   

        //TODO: Weapon, weapon 장착 애니메이션 따로 제작 및 추가 필요


    }
 
    
}
