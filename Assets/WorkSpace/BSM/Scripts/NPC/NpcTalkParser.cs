using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(NpcTalkParser))]
public class NpcSheetDownButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        NpcTalkParser npcTalkParser = (NpcTalkParser)target;

    }
    
}

public class NpcTalkParser : MonoBehaviour
{

    private const string sheetPath = "";
    
    public void StartDownload(bool renameFiles)
    {
        
    }
    
    
}
