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
        IngameManager.instance.SetStage(stage);
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
 
