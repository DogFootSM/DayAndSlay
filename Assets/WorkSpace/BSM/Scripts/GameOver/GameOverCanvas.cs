using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameOverCanvas : MonoBehaviour
{
    [Inject(Id = "outSideScene")]
    private SceneReference townScene;
    
    [Inject(Id = "LoadingScene")]
    private SceneReference loadingScene;
    
    [SerializeField] private Button confirmButton;
    [SerializeField] private PlayerRoot playerRoot;
    [SerializeField] private PlayerController playerController;
    
    private void Awake()
    {
        confirmButton.onClick.AddListener(MoveToTown);
    }

    private void MoveToTown()
    {
        Debug.Log($"마을로 이동 {playerRoot == null}");
        playerController.PlayerResurrection();
        playerRoot.TranslateScenePosition(new Vector2(0, 0));
        Loading.LoadScene(townScene);
        SceneManager.LoadScene(loadingScene.Name);
    } 
}
