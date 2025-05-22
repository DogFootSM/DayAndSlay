using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;
using IInitializable = Unity.VisualScripting.IInitializable;

public class DataManager : MonoBehaviour
{
    [Inject] private SqlManager sqlManager;
    public int SlotId;

    private Sprite[][] changeSprites = new Sprite[(int)BodyPartsType.SIZE][];

    private int curWeapon;
    private int weaponIndex;

    private List<string> spriteColumns = new List<string>();
    private List<string> spriteNames = new List<string>();

    /// <summary>
    /// 캐릭터 생성 -> 선택한 프리셋 json 저장
    /// </summary>
    /// <param name="presets"></param>
    public void SavePresetData(List<Image> presets, int WeaponType)
    {
        for (int i = 0; i < presets.Count; i++)
        {
            //착용 에셋명 저장
            spriteColumns.Add(presets[i].sprite.name);
        }

        sqlManager.UpdateDataColumn(new[]
            {
                sqlManager.CharacterColumn(CharacterDataColumns.HAIR_SPRITE),
                sqlManager.CharacterColumn(CharacterDataColumns.BODY_SPRITE),
                sqlManager.CharacterColumn(CharacterDataColumns.SHIRT_SPRITE),
                sqlManager.CharacterColumn(CharacterDataColumns.WEAPON_SPRITE),
            }, spriteColumns.ToArray(), sqlManager.CharacterColumn(CharacterDataColumns.SLOT_ID), $"{SlotId}");

        sqlManager.UpdateDataColumn(new[] { sqlManager.CharacterColumn(CharacterDataColumns.WEAPON_TYPE)}, new[] { $"{WeaponType}" }, 
            sqlManager.CharacterColumn(CharacterDataColumns.SLOT_ID), 
            $"{SlotId}");
    }


    /// <summary>
    /// 캐릭터 데이터 로드
    /// </summary>
    /// <param name="characterAnimatorController">현재 캐릭터</param>
    public void LoadPresetData(CharacterAnimatorController characterAnimatorController)
    {
        IDataReader dataReader = sqlManager.ReadDataColumn(new[]
        {
            sqlManager.CharacterColumn(CharacterDataColumns.HAIR_SPRITE),
            sqlManager.CharacterColumn(CharacterDataColumns.BODY_SPRITE),
            sqlManager.CharacterColumn(CharacterDataColumns.SHIRT_SPRITE),
            sqlManager.CharacterColumn(CharacterDataColumns.WEAPON_SPRITE),
        }, new[] {sqlManager.CharacterColumn(CharacterDataColumns.SLOT_ID)}, 
            new [] {$"{SlotId}"}, 
            null);
        
        //Body Sprite 데이터 가져옴
        while (dataReader.Read())
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                spriteNames.Add(dataReader.GetString(i)); 
            } 
        }
         
        dataReader = sqlManager.ReadDataColumn(new []{sqlManager.CharacterColumn(CharacterDataColumns.WEAPON_TYPE),}, 
            new []{sqlManager.CharacterColumn(CharacterDataColumns.SLOT_ID)}, 
            new [] {$"{SlotId}"}, 
            null);
        
        //현재 무기 상태 데이터 가져옴
        while (dataReader.Read())
        {
            curWeapon = dataReader.GetInt32(0);
        }
        
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
                                                                 $"{spriteNames[i]}/" +
                                                                 $"{(CharacterAnimationType)j}/" +
                                                                 $"{(CharacterWeaponType)curWeapon}");
                }
                else
                {
                    // 경로 : 어느 부위 - 에셋 이름 - 캐릭터 상태 
                    changeSprites[i] = Resources.LoadAll<Sprite>($"Preset/Animations/Character/" +
                                                                 $"{((BodyPartsType)i)}/" +
                                                                 $"{spriteNames[i]}/{(CharacterAnimationType)j}");
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


        //현재 무기가 Wand일 경우 Short Sword 인덱스로, 그 외 자기 무기 인덱스 할당
        weaponIndex = (CharacterWeaponType)curWeapon switch
        {
            CharacterWeaponType.WAND => (int)CharacterWeaponType.SHORT_SWORD,
            _ => curWeapon
        }; 
        
        //무기 스프라이트 변경
        for (int i = 0; i < (int)CharacterAnimationType.SIZE; i++)
        {
            changeSprites[0] = Resources.LoadAll<Sprite>($"Preset/Animations/Character/WEAPON/" +
                                                         $"{(CharacterWeaponType)curWeapon}/" +
                                                         $"{spriteNames[spriteNames.Count - 1]}/" +
                                                         $"{(CharacterAnimationType)i}");
  
            for (int j = 0; j < changeSprites[0].Length; j++)
            {
                characterAnimatorController.EquipmentLibraryAsset[weaponIndex].AddCategoryLabel(changeSprites[0][j],
                    ((CharacterAnimationType)i).ToString(),
                    $"{(CharacterAnimationType)i + "_" + j}");
            }
        }

        characterAnimatorController.WeaponAnimatorChange(weaponIndex);
    }

    /// <summary>
    /// 캐릭터 생성 여부 업데이트
    /// </summary>
    public void CreateDataUpdate()
    {
        sqlManager.UpdateDataColumn(new [] {sqlManager.CharacterColumn(CharacterDataColumns.IS_CREATE)},
            new [] {"1"}, 
            sqlManager.CharacterColumn(CharacterDataColumns.SLOT_ID),
            $"{SlotId}");
    }
    
}