using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;
using Zenject;

public class DataManager : MonoBehaviour
{
    [Inject] private SqlManager sqlManager;
    public int SlotId;
    
    private Sprite[][] changeSprites = new Sprite[(int)BodyPartsType.SIZE][];

    private int curWeapon;
    private int weaponTier;
    public int WeaponTier => weaponTier;
    
    private List<string> spriteColumns = new List<string>();
    private List<string> spriteNames = new List<string>();

    private string path;
    private AudioSettings audioSettings;
    private DisplaySettings displaySettings;
    
    private SoundManager soundManager => SoundManager.Instance;
    private GameManager gameManager => GameManager.Instance;

    private const string audioDataPath = "AudioSetting.json";
    private const string displayDataPath = "DisplaySettings.json";
    
    private void Awake()
    {
        Init();
    }

    /// <summary>
    /// Setting Data 객체 생성
    /// </summary>
    private void Init()
    {
        displaySettings = new DisplaySettings();
        audioSettings = new AudioSettings();
    }
    
    /// <summary>
    /// Data Path 설정
    /// </summary>
    /// <param name="path"></param>
    private void SetPath(string path)
    {
        this.path = Path.Combine(Application.streamingAssetsPath, path);
    }

    /// <summary>
    /// Display 관련 설정 데이터 Load
    /// </summary>
    public void LoadDisplayData()
    {
        SetPath(displayDataPath);
         
        if (!File.Exists(path))
        { 
            SaveDisplayData(0, (int)FullScreenMode.ExclusiveFullScreen, (int)CursorLockMode.None);
        }

        string loadDisplayData = File.ReadAllText(path);

        displaySettings = JsonUtility.FromJson<DisplaySettings>(loadDisplayData);

        gameManager.ResolutionIndex = displaySettings.Resolution;
        gameManager.SetWindowMode(displaySettings.WindowMode);
        gameManager.SetResolution(displaySettings.Resolution);
        gameManager.SetMouseCursorLockMode(displaySettings.MouseLock);
    }

    /// <summary>
    /// Display 설정 저장
    /// </summary>
    /// <param name="resolution">화면비</param>
    /// <param name="windowMode">화면 모드</param>
    /// <param name="mouseCursorLockMode">마우스 잠금 설정</param>
    public void SaveDisplayData(int resolution, int windowMode, int mouseCursorLockMode)
    {
        displaySettings.Resolution = resolution;
        displaySettings.WindowMode = windowMode;
        displaySettings.MouseLock = mouseCursorLockMode;

        string toJson = JsonUtility.ToJson(displaySettings);
        File.WriteAllText(path, toJson);
    }

    /// <summary>
    /// 퀵슬롯 데이터 불러오기
    /// </summary>
    /// <returns>퀵슬롯 매니저에서 스킬 노드 초기화에 사용할 데이터 객체</returns>
    public QuickSlotSetting LoadQuickSlotSetting()
    {
        //TODO: 테스트 끝나면 수정
        //SetPath($"QuickSlotSaveData{SlotId}.json");
        SetPath($"QuickSlotSaveData0.json");

        QuickSlotSetting quickslotSetting = new QuickSlotSetting();
        
        if (!File.Exists(path))
        {
            SaveQuickSlotSetting();
        }

        string loadQuickSlotData = File.ReadAllText(path);
        quickslotSetting = JsonUtility.FromJson<QuickSlotSetting>(loadQuickSlotData);

        return quickslotSetting;
    }

    /// <summary>
    /// 퀵슬롯 데이터 저장
    /// </summary>
    public void SaveQuickSlotSetting()
    {
        //TODO:테스트 끝나면 변경하기
        //SetPath($"QuickSlotSaveData{SlotId}.json");
        SetPath($"QuickSlotSaveData0.json");
        
        QuickSlotSetting quickslotSetting = new QuickSlotSetting();
        
        //각각의 무기 별 퀵슬롯 순회
        foreach (CharacterWeaponType weaponType in Enum.GetValues(typeof(CharacterWeaponType)))
        {
            if(weaponType == CharacterWeaponType.SIZE) continue;
            
            //무기 그룹 생성 후 무기 타입 할당
            var weaponGroup = new WeaponGroup()
            {
                WeaponType = weaponType
            };
            
            //퀵 슬롯 그룹을 순회하면서 객체를 생성하고 키, 값 할당
            foreach (var quickSlot in QuickSlotData.WeaponQuickSlotDict[weaponType])
            {
                var quickSlotGroup = new QuickSlotGroup()
                {
                    QuickSlotType = quickSlot.Key,
                    SkillDataID = quickSlot.Value.skillData.SkillId
                };
                
                //각각의 퀵슬롯 그룹 추가
                weaponGroup.QuickSlotGroups.Add(quickSlotGroup);
            }
            
            //각 무기 별 그룹 추가
            quickslotSetting.WeaponGroups.Add(weaponGroup); 
        }
          
        string toJson = JsonUtility.ToJson(quickslotSetting, true);
        File.WriteAllText(path, toJson);
    }
    
    /// <summary>
    /// 소리 설정 Data Load
    /// </summary>
    public void LoadAudioData()
    {
        SetPath(audioDataPath);
             
        //해당 경로에 파일이 없을 경우 데이터 생성
        if (!File.Exists(path))
        {
            SaveAudioData(0.5f, 0.5f, 0.5f, false, false, false); 
        }
        
        string loadAudioData = File.ReadAllText(path);
        audioSettings = JsonUtility.FromJson<AudioSettings>(loadAudioData);
        
        soundManager.SetMasterVolume(audioSettings.MasterVolume);
        soundManager.SetSFxVolume(audioSettings.SfxVolume);
        soundManager.SetBgmVolume(audioSettings.BgmVolume);
        soundManager.SetMuteState(audioSettings.MasterMute, audioSettings.BgmMute, audioSettings.SfxMute);
    }

    /// <summary>
    /// 현재 설정되어 있는 Audio Volume 값 저장
    /// </summary>
    /// <param name="MasterVolume">전체 음량</param>
    /// <param name="BgmVolume">배경음</param>
    /// <param name="SfxVolume">효과음</param>
    public void SaveAudioData(float MasterVolume, float BgmVolume, float SfxVolume, bool masterMute, bool bgmMute, bool sfxMute)
    {
        SetPath(audioDataPath);
         
        audioSettings.MasterVolume = MasterVolume;
        audioSettings.BgmVolume = BgmVolume;
        audioSettings.SfxVolume = SfxVolume;
        audioSettings.MasterMute = masterMute;
        audioSettings.BgmMute = bgmMute;
        audioSettings.SfxMute = sfxMute;
        
        string json = JsonUtility.ToJson(audioSettings);
        File.WriteAllText(path, json);
    }
    
    /// <summary>
    /// 캐릭터 생성 -> 선택한 프리셋 json 저장
    /// </summary>
    /// <param name="presets"></param>
    public void SavePresetData(List<Image> presets, int WeaponType, int weaponTier)
    {
        for (int i = 0; i < presets.Count; i++)
        {
            //착용 에셋명 저장
            spriteColumns.Add(presets[i].sprite.name);
        }

        sqlManager.CharacterInsertTable(
            new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID) },
            new[] { $"{SlotId}" });

        sqlManager.UpdateCharacterDataColumn(new[]
            {
                sqlManager.GetCharacterColumn(CharacterDataColumns.HAIR_SPRITE),
                sqlManager.GetCharacterColumn(CharacterDataColumns.BODY_SPRITE),
                sqlManager.GetCharacterColumn(CharacterDataColumns.SHIRT_SPRITE),
                sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_SPRITE),
                
            }, spriteColumns.ToArray(), sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID), $"{SlotId}");

        sqlManager.UpdateCharacterDataColumn(new[]
            {
                sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_TYPE),
                sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_TIER)
            },
            new[] { $"{WeaponType}", $"{weaponTier}"},
            sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID),
            $"{SlotId}");

        StartItemDataInsert(WeaponType);
        StartSkillDataInsert();
    }

    /// <summary>
    /// 초기 캐릭터 생성 시 아이템 지급
    /// </summary>
    private void StartItemDataInsert(int weaponType)
    {
        //TODO: 아이템 ID 양식 정립되면 수정하기.
        sqlManager.UpsertItemDataColumn(
            new[]
            {
                sqlManager.GetCharacterItemColumn(CharacterItemDataColumns.ITEM_ID),
                sqlManager.GetCharacterItemColumn(CharacterItemDataColumns.SLOT_ID),
                sqlManager.GetCharacterItemColumn(CharacterItemDataColumns.ITEM_AMOUNT),
                sqlManager.GetCharacterItemColumn(CharacterItemDataColumns.INVENTORY_SLOT_ID),
                sqlManager.GetCharacterItemColumn(CharacterItemDataColumns.IS_EQUIPMENT),
            },
            new[]
            {
                //TODO: 지급 무기 수정
                $"{100001 + (weaponType * 100)}02", //지급할 Item_id
                $"{SlotId}", //해당 캐릭터 slotId
                "1", //지급할 개수
                "0", //인벤토리 슬롯의 위치
                "1" //장비 착용 여부
            }
        );
    }

    /// <summary>
    /// 
    /// </summary>
    private void StartSkillDataInsert()
    {
         sqlManager.InsertSkillDataColumn($"{SlotId}");
    }


    /// <summary>
    /// 캐릭터 데이터 로드
    /// </summary>
    /// <param name="characterAnimatorController">현재 캐릭터</param>
    public void LoadPresetData(CharacterAnimatorController characterAnimatorController)
    {
        //TODO: 테스트용 슬롯 id 고정, 추후 제거하기
        SlotId = 1;
        
        IDataReader dataReader = sqlManager.ReadDataColumn(new[]
            {
                //TODO:망토, 모자 등 애니메이션 추가 필요 데이터 테이블 컬럼에도 추가해야함
                sqlManager.GetCharacterColumn(CharacterDataColumns.HAIR_SPRITE),
                sqlManager.GetCharacterColumn(CharacterDataColumns.BODY_SPRITE),
                sqlManager.GetCharacterColumn(CharacterDataColumns.SHIRT_SPRITE),
                sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_SPRITE),
            }, new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID) },
            new[] { $"{SlotId}" },
            null);

        //Body Sprite 데이터 가져옴
        while (dataReader.Read())
        {
            for (int i = 0; i < dataReader.FieldCount; i++)
            {
                spriteNames.Add(dataReader.GetString(i));
            }
        }

        dataReader = sqlManager.ReadDataColumn(
            new[]
            {
                sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_TYPE),
                sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_TIER),
            },
            new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID) },
            new[] { $"{SlotId}" },
            null);

        //현재 무기 상태 데이터 가져옴
        while (dataReader.Read())
        {
            curWeapon = dataReader.GetInt32(0);
            weaponTier = dataReader.GetInt32(1);
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

        //무기 스프라이트 변경
        for (int i = 0; i < (int)CharacterAnimationType.SIZE; i++)
        {
            changeSprites[0] = Resources.LoadAll<Sprite>($"Preset/Animations/Character/WEAPON/" +
                                                         $"{(CharacterWeaponType)curWeapon}/" +
                                                         $"{(WeaponTierType)weaponTier}/" +
                                                         $"{(CharacterAnimationType)i}");

            for (int j = 0; j < changeSprites[0].Length; j++)
            {
                characterAnimatorController.EquipmentLibraryAsset[curWeapon].AddCategoryLabel(changeSprites[0][j],
                    ((CharacterAnimationType)i).ToString(),
                    $"{(CharacterAnimationType)i + "_" + j}");
            }
        }
        
        characterAnimatorController.AnimatorChange(curWeapon, weaponTier);
    }

    public void ChangeWeaponSpriteLibraryAsset(SpriteLibraryAsset libraryAssets, int curWeapon, int weaponTier)
    {
        Debug.Log("무기 티어 변경");
        for (int i = 0; i < (int)CharacterAnimationType.SIZE; i++)
        {
            changeSprites[0] = Resources.LoadAll<Sprite>($"Preset/Animations/Character/WEAPON/" +
                                                         $"{(CharacterWeaponType)curWeapon}/" +
                                                         $"{(WeaponTierType)weaponTier}/" +
                                                         $"{(CharacterAnimationType)i}");
            Debug.Log($"경로 :" + $"Preset/Animations/Character/WEAPON/" +
                      $"{(CharacterWeaponType)curWeapon}/" +
                      $"{(WeaponTierType)weaponTier}/" +
                      $"{(CharacterAnimationType)i}");
            
            for (int j = 0; j < changeSprites[0].Length; j++)
            {
                libraryAssets.AddCategoryLabel(changeSprites[0][j],
                    ((CharacterAnimationType)i).ToString(),
                    $"{(CharacterAnimationType)i + "_" + j}");
            }
        }
    }
    
    /// <summary>
    /// 무기 변경 시 공격 애니메이션에 대한 라이브러리 에셋 변경
    /// Hair, Body, Shirt 등
    /// </summary>
    /// <param name="libraryAssets">공격 애니메이션 변경할 스프라이트 라이브러리 에셋</param>
    /// <param name="weaponType">변경할 무기 타입</param>
    public void ChangeAttackSpriteLibraryAsset(SpriteLibraryAsset[] libraryAssets, int weaponType)
    { 
        //라이브러리 에셋 크기만큼 반복
        for (int i = 0; i < libraryAssets.Length; i++)
        {
            //공격 애니메이션부터 변경
            for (int j = (int)CharacterAnimationType.SIDEATTACK; j < (int)CharacterAnimationType.SIZE; j++)
            {
                changeSprites[i] = Resources.LoadAll<Sprite>($"Preset/Animations/Character/" +
                                                             $"{((BodyPartsType)i)}/" +
                                                             $"{spriteNames[i]}/" +
                                                             $"{(CharacterAnimationType)j}/" +
                                                             $"{(CharacterWeaponType)weaponType}");
                 
                for (int k = 0; k < changeSprites[i].Length; k++)
                {
                    libraryAssets[i].AddCategoryLabel(changeSprites[i][k],
                        ((CharacterAnimationType)j).ToString(),
                        $"{((CharacterAnimationType)j) + "_" + k}");
                }
            } 
        }
    }
    
    /// <summary>
    /// 캐릭터 생성 여부 업데이트
    /// </summary>
    public void CreateDataUpdate()
    {
        sqlManager.UpdateCharacterDataColumn(new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.IS_CREATE) },
            new[] { "1" },
            sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID),
            $"{SlotId}");
    }
}