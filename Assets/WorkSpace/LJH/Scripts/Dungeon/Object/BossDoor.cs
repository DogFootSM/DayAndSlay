using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossDoor : InteractableObj
{
    [Header("로딩 씬")]
    [SerializeField] private SceneReference loadingScene;
    [Header("이동할 씬")]
    [SerializeField] private SceneReference scene;
    
    [SerializeField] private GameObject dungeonExitPopUp;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private PlayerRoot player;
    private MapManager mapManager;
    
    private void Start()
    {
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
        dungeonExitPopUp.SetActive(true);
    }

    public override void UiOnOffMethod(Collider2D collider)
    {
        if(collider.CompareTag("Player"))
            player = collider.GetComponent<PlayerRoot>();
        
        dungeonExitPopUp.SetActive(!dungeonExitPopUp.activeSelf);
    }

    /// <summary>
    /// 팝업_예스 버튼 : 상점씬으로 이동
    /// </summary>
    private void YesButton()
    {
        Loading.LoadScene(scene);
        SceneManager.LoadScene(loadingScene.Name);
        
        //타운씬 던젼도어 아래쪽 위치 강제로 할당
        player?.TranslateScenePosition(new Vector2(-83.75f, -30));
        mapManager.MapChange(MapType.TOWN_OUTSIDE);
    }

    /// <summary>
    /// 팝업_노 버튼 : 팝업 닫기
    /// </summary>
    private void NoButton()
    {
        dungeonExitPopUp.SetActive(false);
    }
}
