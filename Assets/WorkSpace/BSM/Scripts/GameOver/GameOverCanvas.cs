using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameOverCanvas : MonoBehaviour
{
    
    [SerializeField] private SceneReference townScene;
    [SerializeField] private SceneReference loadingScene;
    
    [SerializeField] private Button confirmButton;
    [SerializeField] private PlayerRoot playerRoot;
    [SerializeField] private PlayerController playerController;

    [Inject] private MapManager mapManager;
    
    private void Awake()
    {
        confirmButton.onClick.AddListener(MoveToTown);
        ProjectContext.Instance.Container.Inject(this);
    }

    private void MoveToTown()
    {
        Debug.Log($"마을로 이동 {playerRoot == null}");
        playerController.PlayerResurrection();
        playerRoot.TranslateScenePosition(new Vector2(-47.22f, -16f));
        mapManager.MapChange(MapType.TOWN_OUTSIDE);
        Loading.LoadScene(townScene);
        SceneManager.LoadScene(loadingScene.Name);
    } 
}
