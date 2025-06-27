using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameSaveHandler : MonoBehaviour
{
    [SerializeField] private GameObject saveAskAlert;
    [SerializeField] private SavePromptUI savePromptUI;
    
    
    public void OpenSaveAlert()
    { 
        saveAskAlert.SetActive(true); 
    } 
}
