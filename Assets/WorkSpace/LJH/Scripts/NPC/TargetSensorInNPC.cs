using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class TargetSensorInNPC : MonoBehaviour
{
    [SerializeField] private AstarPath astar;

    [SerializeField] private GameObject outsideDoor;
    [SerializeField] private GameObject storeDoor;
    [SerializeField] private GameObject table;
    [SerializeField] private GameObject player;

    [SerializeField] private Vector3 outsideDoorPos;
    [SerializeField] private Vector3 storeDoorPos;
    [SerializeField] private Vector3 tablePos;
    [SerializeField] private Vector3 playerPos;

    private void Start()
    {
        //아웃사이드 도어의 경우 그냥 고정된 값 넣어주면 됨
        //스토어 도어의 경우 그냥 고정된 값 넣어주면 됨
        //테이블의 경우 NPC에서 계산때려서 나온 테이블의 위치값 넣어주면 됨
        //플레이어의 경우 플레이어 감지해서 플레이어의 위치를 계속 받아와야 함
        
        //테스트 코드 : 추후 더 좋은 방법이 있다면 교체할 것
        player = GameObject.FindWithTag("Player");

        outsideDoorPos = outsideDoor.transform.position;
        storeDoorPos = storeDoor.transform.position;
        playerPos = player.transform.position;

        Set_Target();
        
    }
    public void InjectTable(Table table)
    {
        this.table = table.gameObject;
        tablePos = table.transform.position;
    }

    private void Set_Target()
    {
        //상태 패턴 또는 분기 전환식으로 타겟 바꿔주면 됨
        Vector3 targetPos = tablePos;

        astar.DetectTarget(transform.position, targetPos);
    }

}
