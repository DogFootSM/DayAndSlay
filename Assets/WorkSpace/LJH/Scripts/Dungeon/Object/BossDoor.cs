using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class BossDoor : InteractableObj
{
    [Header("·Îµù ¾À")]
    [SerializeField] private SceneReference loadingScene;
    [Header("ÀÌµ¿ÇÒ ¾À")]
    [SerializeField] private SceneReference scene;
    
    [SerializeField] private GameObject dungeonExitPopUp;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private PlayerRoot player;
    [Inject]
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
    /// ÆË¾÷_¿¹½º ¹öÆ° : »óÁ¡¾ÀÀ¸·Î ÀÌµ¿
    /// </summary>
    private void YesButton()
    {
        player?.TranslateScenePosition(new Vector2(-83.75f, -30));
        mapManager.MapChange(MapType.TOWN_OUTSIDE);
        
        Loading.LoadScene(scene);
        SceneManager.LoadScene(loadingScene.Name);
        
    }

    /// <summary>
    /// ÆË¾÷_³ë ¹öÆ° : ÆË¾÷ ´Ý±â
    /// </summary>
    private void NoButton()
    {
        dungeonExitPopUp.SetActive(false);
    }
}
