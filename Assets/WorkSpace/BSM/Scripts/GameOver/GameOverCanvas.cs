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
        playerController.PlayerResurrection();
        playerRoot.TranslateScenePosition(new Vector2(59f, -34f));
        mapManager.MapChange(MapType.TOWN_STORE2F);
        Loading.LoadScene(townScene);
        SceneManager.LoadScene(loadingScene.Name);
    } 
}
