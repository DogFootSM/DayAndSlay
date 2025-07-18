using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Door : InteractableObj
{
    private Animator animator;

    [Inject(Id = "LoadingScene")]
    private SceneReference loadingScene;
    [Inject]
    private StoreManager store;

    [SerializeField] private Transform movePosTrans;
    private Vector2 movePos;
    [SerializeField] private GameObject player;
    [SerializeField] private DoorType doorType;
    [Inject(Id = "PopUp")]
    GameObject popUp;

    void Start()
    {
        animator = GetComponent<Animator>();
        movePos = movePosTrans.position;
        player = GameObject.FindWithTag("Player");
    }

    public override void Interaction()
    {
        animator.Play("DoorOpenAni");
        SceneManager.LoadSceneAsync(loadingScene.Name, LoadSceneMode.Additive);
        player.transform.position = movePos;

    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("NPC"))
        {
            GameObject npcObj = collision.gameObject;
            Npc npc = npcObj.GetComponent<Npc>();

            npcObj.transform.position = movePos;
            npc.SetMoving(false);
            npc.StateMachine.ChangeState(new NpcIdleState(npc));

            store.EnqueueInNpcQue(npc);
            return;
        }


        switch (doorType)
        {
            case DoorType.DOOR:
                popUp.GetComponent<PopUp>().objName = "문";
                break;

            case DoorType.LADDER:
                popUp.GetComponent<PopUp>().objName = "사다리";
                break;

            default:
                popUp.GetComponent<PopUp>().objName = "기타";
                break;
        }
        popUp.SetActive(!popUp.gameObject.activeSelf);
    }




}
