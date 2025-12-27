using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private List<Image> buttonsSelectedMark;
    [SerializeField] private Button okayButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private DungeonEnterDoor door;

    private void OnEnable()
    {
        //킬때 1스테이지로 초기화
        for (int i = 0; i < buttonsSelectedMark.Count; i++)
        {
            buttonsSelectedMark[i].gameObject.SetActive(false);
        }
        
        buttonsSelectedMark[0].gameObject.SetActive(true);
    }
    private void Start()
    {
        ButtonInit();
        StageSelectableCheck();
    }

    private void StageSelectableCheck()
    {
        //1Stage 클리어 체크시 2스테이지 오픈
        buttons[1].interactable = DungeonManager.is1StageCleared;
        //2Stage 클리어 체크시 3스테이지 오픈
        buttons[2].interactable = DungeonManager.is2StageCleared;
    }


    private void ButtonInit()
    {
        foreach (Button btn in buttons)
        {
            Button curBtn = btn;
            btn.onClick.AddListener(() => TapButton((StageNum)buttons.IndexOf(curBtn)));
        }
        
        okayButton.onClick.AddListener(OkayButton);
        cancelButton.onClick.AddListener(CancelButton);
    }

    private void TapButton(StageNum stage)
    {
        DungeonRoomSpawner.stageNum = stage;

        //마크 다 끄고 선택한거만 On
        for (int i = 0; i < buttonsSelectedMark.Count; i++)
        {
            buttonsSelectedMark[i].gameObject.SetActive(false);
        }
        
        buttonsSelectedMark[(int)stage].gameObject.SetActive(true);
    }

    private void OkayButton()
    {
        door.PlayerInteractionDoor();
    }

    private void CancelButton()
    {
        gameObject.SetActive(false);
    }
}
 
