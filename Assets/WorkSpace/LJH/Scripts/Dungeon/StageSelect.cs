using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [SerializeField] List<Button> buttons = new List<Button>();

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
    }

    private void TapButton(StageNum stage)
    {
        IngameManager.instance.SetStage(stage);
    }
}
 
