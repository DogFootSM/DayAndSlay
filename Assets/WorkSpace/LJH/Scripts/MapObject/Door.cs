using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
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

    [SerializeField] private List<Grid> gridList;
    private Grid grid;

    void Start()
    {
        animator = GetComponent<Animator>();
        movePos = movePosTrans.position;

        grid = GetCurrentGrid(transform.position);
    }

    public override void Interaction()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        
        animator.Play("DoorOpenAni");
        //SceneManager.LoadSceneAsync(loadingScene.Name, LoadSceneMode.Additive);
        player.transform.position = movePos;

    }

    public override void UiOnOffMethod(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            if(doorType == DoorType.DOOR)
            {
                Npc npc = collision.gameObject.GetComponent<Npc>();
                npc.StopMove();
                npc.transform.position = movePos;
                npc.StateMachine.ChangeState(new NpcIdleState(npc));

                if (grid == gridList[0])
                {
                    store.EnqueueInNpcQue(npc);
                    return;
                }
            }
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
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
    
    public override void UiOnOffMethod(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("NPC"))
        {
            if(doorType == DoorType.DOOR)
            {
                Npc npc = collider.gameObject.GetComponent<Npc>();
                npc.StopMove();
                npc.transform.position = movePos;
                npc.StateMachine.ChangeState(new NpcIdleState(npc));

                if (grid == gridList[0])
                {
                    store.EnqueueInNpcQue(npc);
                    return;
                }
            }
        }

        else if (collider.gameObject.CompareTag("Player"))
        {
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

    private Grid GetCurrentGrid(Vector3 worldPos)
    {
        foreach (var grid in gridList)
        {
            Tilemap tilemap = grid.transform.GetChild(0).GetComponent<UnityEngine.Tilemaps.Tilemap>();
            Vector3Int cell = grid.WorldToCell(worldPos);
            if (tilemap.cellBounds.Contains(cell)) return grid;
        }
        return null;
    }




}
