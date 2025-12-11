using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField] private List<Button> buttons = new List<Button>();
    [SerializeField] private Button okayButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private DungeonEnterDoor door;
    
    private void Start()
    {
        ButtonInit();
        StageSelectableCheck();

        DungeonManager.is1StageCleared = true;
        DungeonManager.is2StageCleared = true;
        DungeonManager.is3StageCleared = true;

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
        //IngameManager.instance.SetStage(stage);
        DungeonRoomSpawner.stageNum = stage;
        //for (int i = 0; i < buttons.Count - 1; i++)
        //{
        //    buttons[i].GetComponent<Image>().color = Color.white;
        //}
        //
        //buttons[(int)stage].GetComponent<Image>().color = Color.yellow;
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
 
