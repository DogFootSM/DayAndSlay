using AYellowpaper.SerializedCollections.Editor.Search;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class TargetSensorInNPC : MonoBehaviour
{
    [Header("Grid & TileMap / 0 = outside, 1 = store")]
    [SerializeField] private List<Grid> gridList;
    [SerializeField] private List<Tilemap> mapTile;
    [SerializeField] private List<Tilemap> obstacleTile;
    private int outsideNum = 0;
    private int storeNum = 1;

    [Header("Component")]
    [SerializeField] private NPC npc;
    [SerializeField] private AstarPath astar;
    [SerializeField] private NpcStateMachine state;

    [Header("Object")]
    [SerializeField] private GameObject castleDoor;
    [SerializeField] private GameObject outsideDoor;
    [SerializeField] private GameObject storeDoor;
    [SerializeField] private GameObject table;
    [SerializeField] private GameObject player;

    [Header("PosByObject")]
    [SerializeField] private Vector3 castleDoorPos;
    [SerializeField] private Vector3 outsideDoorPos;
    [SerializeField] private Vector3 storeDoorPos;
    [SerializeField] private Vector3 tablePos;
    [SerializeField] private Vector3 playerPos;

    public Vector3 targetPos;

    private void Start()
    {
        //엔피씨가 생성되는거라서 자동으로 찾아오게 해줘야함 

        //테스트 코드 : 추후 더 좋은 방법이 있다면 교체할 것
        player = GameObject.FindWithTag("Player");

        castleDoorPos = castleDoor.transform.position;
        outsideDoorPos = outsideDoor.transform.position;
        storeDoorPos = storeDoor.transform.position;
        playerPos = player.transform.position;

        Set_Target();

        StartCoroutine(ChangeGridCoroutine());

    }
    public void InjectTable(Table table)
    {
        this.table = table.gameObject;
        tablePos = table.transform.position;
    }

    private void Set_Target()
    {
        Debug.Log("셋타겟 실행");
        //임시 bool 변수
        bool npcPosisStore = true;

        Debug.Log($"npc는 ? :{npc.name}");

        if (npcPosisStore)
        { 
            Debug.Log("1단게는 뚫림");
            //상태 패턴 또는 분기 전환식으로 타겟 바꿔주면 됨
            if (npc.wantItem != null)
            {
                if (table != null)
                {
                    Debug.Log("테이블 실행됨");
                    state.ChangeState(new NpcMoveState(npc, table));
                }
                else if (table == null)
                {
                    Debug.Log("플레이어 실행됨");
                    state.ChangeState(new NpcMoveState(npc, player));
                }
            }
            else
            {
                Debug.Log("바깥문 실행됨");
                //if(바깥에서 문을 대상으로 해야할때
                state.ChangeState(new NpcMoveState(npc, outsideDoor));
            }
        }
        else
        {
            Debug.Log("상점문 실행됨");
            //if(상점 내부 문을 대상으로 해야할때
            state.ChangeState(new NpcMoveState(npc, storeDoor));
        }
        Debug.Log(targetPos);
        astar.DetectTarget(transform.position, targetPos);

    }

    private void ChangeGrid()
    {
        
        astar.SetGridAndTilemap(gridList[outsideNum], outsideNum);

        astar.SetGridAndTilemap(gridList[storeNum], storeNum);
    }

    private IEnumerator ChangeGridCoroutine()
    {
        //시작하자마자 이거부터 확인할것 
        int num = 0;
        while (true)
        {
            yield return new WaitForSeconds(1f);

            //moveDir = (direction.x > 0) ? Vector3.right : Vector3.left;
            astar.SetGridAndTilemap(gridList[num], num);

            num = (num == 0) ? 1 : 0;

        }

    }

    public void TableButton()
    {
        state.ChangeState(new NpcMoveState(npc, table));
    }
    public void PlayerButton()
    {
        state.ChangeState(new NpcMoveState(npc, player));
    }
    public void storeDoorButton()
    {
        state.ChangeState(new NpcMoveState(npc, storeDoor));
    }
    public void OutDoorButton()
    {
        state.ChangeState(new NpcMoveState(npc, outsideDoor));
    }
}
