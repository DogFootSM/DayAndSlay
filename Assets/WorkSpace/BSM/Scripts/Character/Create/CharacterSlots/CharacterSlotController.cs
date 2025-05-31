using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

public class CharacterSlotController : MonoBehaviour
{
    [Inject] private SqlManager sqlManager;
    [SerializeField] private List<CharacterSlot> characterSlots;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private SceneReference inGameScene;

    [SerializeField] private GameObject DeleteAlert;
    [SerializeField] private int DeleteSlotId;

    protected void Awake()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            characterSlots[i].slotId = i + 1;
        }

        cancelButton.onClick.AddListener(() => DeleteAlert.SetActive(false));
        confirmButton.onClick.AddListener(DeleteSlotConfirm);
    }

    /// <summary>
    /// 삭제 얼럿 활성화
    /// </summary>
    /// <param name="slotId">삭제할 데이터 슬롯 id</param>
    public void DeleteSlot(int slotId)
    {
        DeleteSlotId = slotId;
        DeleteAlert.SetActive(true);
    }

    /// <summary>
    /// 슬롯 삭제 완료
    /// </summary>
    private void DeleteSlotConfirm()
    {
        DeleteAlert.SetActive(false);
        //해당 Slot_id 컬럼 삭제
        sqlManager.DeleteDataColumn(sqlManager.CharacterColumn(CharacterDataColumns.SLOT_ID),
            $"{DeleteSlotId}");

        characterSlots[DeleteSlotId - 1].IsCreatedCharacter();
    }

    /// <summary>
    /// 슬롯 선택 시 게임 씬 이동
    /// </summary>
    public void LoadInGameScene()
    {
        SceneManager.LoadScene(inGameScene.Name);
    }
}