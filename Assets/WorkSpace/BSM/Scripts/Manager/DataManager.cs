using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DataManager : MonoBehaviour
{
    [Inject] private SqlManager sqlManager;
    public int SlotId;

    private Sprite[][] changeSprites = new Sprite[(int)BodyPartsType.SIZE][];

    private int curWeapon;
    private int weaponIndex;

    private List<string> spriteColumns = new List<string>();
    private List<string> spriteNames = new List<string>();

    private string path;
    private AudioSettings audioSettings;
    private DisplaySettings displaySettings;
    private SoundManager soundManager => SoundManager.Instance;
    private GameManager gameManager => GameManager.Instance;

    private const string audioDataPath = "AudioSetting.json";
    private const string displayDataPath = "DisplaySettings.json";
    private const string quickslotSetPath = "QuickSlotSaveData.json";
    
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

    public void LoadQuickSlotSetting()
    {
        SetPath(quickslotSetPath);

        if (!File.Exists(path))
        {
            SaveQuickSlotSetting();
        }
        
        
        
    }

    public void SaveQuickSlotSetting()
    {
        SetPath(quickslotSetPath);
        
        QuickSlotSetting quickSlotSetting = new QuickSlotSetting();
        
        
        
        
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
            SaveAudioData(0.5f, 0.5f, 0.5f); 
        }
        
        string loadAudioData = File.ReadAllText(path);
        audioSettings = JsonUtility.FromJson<AudioSettings>(loadAudioData);
        
        soundManager.SetMasterVolume(audioSettings.MasterVolume);
        soundManager.SetSFxVolume(audioSettings.SfxVolume);
        soundManager.SetBgmVolume(audioSettings.BgmVolume);
    }

    /// <summary>
    /// 현재 설정되어 있는 Audio Volume 값 저장
    /// </summary>
    /// <param name="MasterVolume">전체 음량</param>
    /// <param name="BgmVolume">배경음</param>
    /// <param name="SfxVolume">효과음</param>
    public void SaveAudioData(float MasterVolume, float BgmVolume, float SfxVolume)
    {
        SetPath(audioDataPath);
         
        audioSettings.MasterVolume = MasterVolume;
        audioSettings.BgmVolume = BgmVolume;
        audioSettings.SfxVolume = SfxVolume;
        
        string json = JsonUtility.ToJson(audioSettings);
        File.WriteAllText(path, json);
    }
    
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

        sqlManager.UpdateCharacterDataColumn(new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_TYPE) },
            new[] { $"{WeaponType}" },
            sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID),
            $"{SlotId}");

        StartItemDataInsert();
        StartSkillDataInsert();
    }

    /// <summary>
    /// 초기 캐릭터 생성 시 아이템 지급
    /// </summary>
    private void StartItemDataInsert()
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
                $"{curWeapon + 100}", //지급할 Item_id
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
            new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.WEAPON_TYPE), },
            new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID) },
            new[] { $"{SlotId}" },
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
        sqlManager.UpdateCharacterDataColumn(new[] { sqlManager.GetCharacterColumn(CharacterDataColumns.IS_CREATE) },
            new[] { "1" },
            sqlManager.GetCharacterColumn(CharacterDataColumns.SLOT_ID),
            $"{SlotId}");
    }
}