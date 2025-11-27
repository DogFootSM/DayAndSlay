using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameOverCanvas : MonoBehaviour
{
    [Inject(Id = "TownScene")] [Header("이동할 씬")]
    private SceneReference scene;
    
    [SerializeField] private Button confirmButton;
    [SerializeField] private PlayerRoot playerRoot;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private SceneReference townScene;
    
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
    } 
}
