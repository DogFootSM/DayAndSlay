using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class DungeonEnterDoor : InteractableObj
{
    [Inject(Id = "LoadingScene")] [Header("로딩 씬")]
    private SceneReference loadingScene;

    [Inject(Id = "DungeonScene")] [Header("이동할 씬")]
    private SceneReference scene;

    [Inject(Id = "PopUp")] private GameObject popUp;
    [SerializeField] private GameObject stagePopUp;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interaction()
    {
        if (DayManager.instance.GetDayOrNight() != DayAndNight.NIGHT) return;
        
        stagePopUp.SetActive(!stagePopUp.activeSelf);
    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        if (DayManager.instance.GetDayOrNight() != DayAndNight.NIGHT) return;
        
        if (collision.gameObject.CompareTag("Player"))
        {   
            popUp.GetComponent<PopUp>().SetText($"스페이스바를 눌러서 입장할 던젼을 선택하세요.");

            popUp.SetActive(!popUp.gameObject.activeSelf);
        }
    }
    
    public void PlayerInteractionDoor()
    {
        Loading.LoadScene(scene);
        SceneManager.LoadScene(loadingScene.Name);
    }
}