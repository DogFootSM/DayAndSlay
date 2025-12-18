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
        Loading.LoadScene(townScene);
        SceneManager.LoadScene(loadingScene.Name);

        playerController.PlayerResurrection();
        Invoke(nameof(Respawn), 0.1f);
    }

    /// <summary>
    /// History : 2025.12.18
    /// 작성자 : 이재호
    /// 날짜 추가해야됨을 알려주는 static 변수 조작 추가
    /// </summary>
    private void Respawn()
    {
        IngameManager.shouldAddDay = true;
        playerRoot.TranslateScenePosition(new Vector2(59f, -34f));
        mapManager.MapChange(MapType.TOWN_STORE2F);
    }
}
