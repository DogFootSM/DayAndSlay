using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class BossDoor : InteractableObj
{
    [Header("로딩 씬")]
    [SerializeField] private SceneReference loadingScene;
    [Header("이동할 씬")]
    [SerializeField] private SceneReference scene;
    
    [SerializeField] private GameObject dungeonExitPopUp;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;
    [SerializeField] private GameObject BlackWindow;

    [SerializeField] private PlayerRoot player;
    [Inject]
    private MapManager mapManager;

    private void OnDisable()
    {
        BlackWindow.SetActive(false);
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerRoot>();
        yesButton.onClick.AddListener(YesButton);
        noButton.onClick.AddListener(NoButton);
        StartCoroutine(IndependentCoroutine());
    }

    private IEnumerator IndependentCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        transform.SetParent(null);
    }

    public override void Interaction()
    {
    }

    public override void UiOnOffMethod(Collider2D collider)
    {
        if(!transform.GetChild(0).gameObject.activeSelf)
            dungeonExitPopUp.SetActive(!dungeonExitPopUp.activeSelf);
    }

    /// <summary>
    /// 팝업_예스 버튼 : 상점씬으로 이동
    /// </summary>
    private void YesButton()
    {
        BlackWindow.SetActive(true);
        Respawn();
        Loading.LoadScene(scene);
        SceneManager.LoadScene(loadingScene.Name);
    }

    /// <summary>
    /// 팝업_노 버튼 : 팝업 닫기
    /// </summary>
    private void NoButton()
    {
        dungeonExitPopUp.SetActive(false);
    }

    /// <summary>
    /// 보스 클리어 후 캐릭터 마을 이동
    /// </summary>
    private void Respawn()
    {
        player.TranslateScenePosition(new Vector2(59f, -34f));
        mapManager.MapChange(MapType.TOWN_STORE2F);
        GameManager.Instance.SearchDayManager();
    }
    
}
