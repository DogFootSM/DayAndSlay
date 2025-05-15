using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    private string path;
    private Sprite[][] changeSprites = new Sprite[(int)BodyPartsType.SIZE][];
    private int weaponIndex;

    /// <summary>
    /// 저장 경로 지정
    /// </summary>
    private void SetSavePath()
    { 
        path = Path.Join(Application.dataPath, "/StreamingAssets/presetData.json"); 
    }

    /// <summary>
    /// 캐릭터 생성 -> 선택한 프리셋 json 저장
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

    
    /// <summary>
    /// 캐릭터 데이터 로드
    /// </summary>
    /// <param name="characterAnimatorController">현재 캐릭터</param>
    public void LoadPresetData(CharacterAnimatorController characterAnimatorController)
    {
        SetSavePath();

        //저장 데이터
        SavePresetData savePresetData = new SavePresetData();

        //데이터 파일 변환
        string loadPresetJson = File.ReadAllText(path);
        savePresetData = JsonUtility.FromJson<SavePresetData>(loadPresetJson);

        //캐릭터 애니메이션 스프라이트 이미지 교체
        for (int i = 0; i < characterAnimatorController.BodyLibraryAsset.Length; i++)
        {
            //타입의 사이즈만큼 반복
            for (int j = 0; j < (int)CharacterAnimationType.SIZE; j++)
            { 
                //0 ~ 5 : IDLE, WALK 동작, 6 ~ 무기별 공격 모션
                if (j > 5)
                {
                    changeSprites[i] = Resources.LoadAll<Sprite>($"Preset/Animations/Character/" +
                                                                 $"{((BodyPartsType)i)}/" +
                                                                 $"{savePresetData.PresetNames[i]}/" +
                                                                 $"{(CharacterAnimationType)j}/" +
                                                                 $"{(CharacterWeaponType)savePresetData.CharacterWeaponType}");
                }
                else
                {
                    // 경로 : 어느 부위 - 에셋 이름 - 캐릭터 상태 
                    changeSprites[i] = Resources.LoadAll<Sprite>($"Preset/Animations/Character/" +
                                                                 $"{((BodyPartsType)i)}/" +
                                                                 $"{savePresetData.PresetNames[i]}/{(CharacterAnimationType)j}");
                }

                //찾아온 애셋의 개수만큼 반복
                for (int k = 0; k < changeSprites[i].Length; k++)
                {
                    characterAnimatorController.BodyLibraryAsset[i].AddCategoryLabel(changeSprites[i][k],
                        ((CharacterAnimationType)j).ToString(),
                        $"{((CharacterAnimationType)j) + "_" + k}");
                }
            }
        }

        //무기 스프라이트 변경 로직
        for (int i = 0; i < (int)CharacterAnimationType.SIZE; i++)
        {
            changeSprites[0] = Resources.LoadAll<Sprite>($"Preset/Animations/Character/WEAPON/" +
                                                         $"{(CharacterWeaponType)savePresetData.CharacterWeaponType}/" +
                                                         $"{savePresetData.PresetNames[savePresetData.PresetNames.Count - 1]}/" +
                                                         $"{(CharacterAnimationType)i}");
            
            //현재 무기가 Wand일 경우 Short Sword 인덱스로, 그 외 자기 무기 인덱스 할당
            weaponIndex = (CharacterWeaponType)savePresetData.CharacterWeaponType switch
            {
                CharacterWeaponType.WAND => (int)CharacterWeaponType.SHORT_SWORD,
                _ => savePresetData.CharacterWeaponType
            };
            
            for (int j = 0; j < changeSprites[0].Length; j++)
            {
                characterAnimatorController.EquipmentLibraryAsset[weaponIndex].AddCategoryLabel(changeSprites[0][j],
                    ((CharacterAnimationType)i).ToString(),
                    $"{(CharacterAnimationType)i + "_" + j}");
                
            } 
        } 
        
        characterAnimatorController.WeaponAnimatorChange(weaponIndex);
    }
}