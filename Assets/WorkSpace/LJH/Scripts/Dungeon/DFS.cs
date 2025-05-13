using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : MonoBehaviour
{
    /// <summary>
    /// 전체 방 목록
    /// </summary>
    [SerializeField] private List<Grid> roomList = new List<Grid> ();

    /// <summary>
    /// 중복을 허용하지 않는 hashset으로 방문한 방 목록 관리
    /// </summary>
    private HashSet<Grid> visitedRoom = new HashSet<Grid> ();

    int bossRoomIndex = 3;

    /// <summary>
    /// 방에 목록된 노드 목록
    /// </summary>
    Dictionary<int, List<Grid>> graph = new Dictionary<int, List<Grid>>();

    [SerializeField] private LineRenderer lineRenderer;

    // 0  1  2   3
    // 4  5  6   7
    // 8  9  10  11
    //

    private void Start()
    {
        GraphModel();
        RootMake(0);
        DrawRouteLine();

    }
    void DrawRouteLine()
    {
        // 순서 보장 위해 HashSet → List로 변환
        List<Grid> orderedRoute = new List<Grid>();

        foreach (Grid grid in roomList)
        {
            if (visitedRoom.Contains(grid))
                orderedRoute.Add(grid);
        }

        lineRenderer.positionCount = orderedRoute.Count;

        for (int i = 0; i < orderedRoute.Count; i++)
        {
            lineRenderer.SetPosition(i, orderedRoute[i].transform.position);
        }
    }


    void RootMake(int curRoomIndex)
    {
        // 현재 방 방문 처리
        visitedRoom.Add(roomList[curRoomIndex]);

        // 보스방이면 끝
        if (curRoomIndex == bossRoomIndex)
        {
            Debug.Log("보스방에 도착했음");
            return;
        }
        // 연결된 방 중에서 방문 안 한 방만 추림
        List<Grid> neighbors = graph[curRoomIndex];

        List<int> unvisitedIndices = new List<int>();

        foreach (Grid neighbor in neighbors)
        {
            int index = roomList.IndexOf(neighbor);

            if (!visitedRoom.Contains(neighbor))
            {
                unvisitedIndices.Add(index);
            }
        }

        // 더 갈 곳이 없으면 리턴 (막다른 길)
        if (unvisitedIndices.Count == 0)
            return;

        // 무작위로 다음 방 선택
        int nextIndex = unvisitedIndices[Random.Range(0, unvisitedIndices.Count)];

        // 재귀
        RootMake(nextIndex);
    }

    void SudoCode()
    {
        /*
        방을 입력

        방이 보스방이라면 탐색 종료
        
        이웃한 방들 중 방문하지 않은 방은 후보에 넣어줌
        이웃한 방이 방문한 방이라면 제외

        이웃한 방중 방문하지 않은 방이 있다면 해당 방의 인덱스로 함수 재귀 실행
        이웃한 방중 방문하지 않은 방이 없다면 함수 종료하고 처음부터 다시 실행

        ------------------------------------------------------------------------------------------------

        인수에 방을 넣어준 상태로 함수 실행

        방의 인덱스가 보스방의 인덱스와 같다면 탐색 종료

        방에 이웃한 방 중 하나를 골라줌

        이웃한 방이 방문하지 않은 방이라면 후보 방 리스트에 추가
        이웃한 방이 방문한 방이라면 리스트에 추가하지 않음

        이웃한 방중 후보 방 리스트에 내용물이 있다면 그 중 랜덤으로 선택하여 함수 재귀 실행
        없다면 함수 종료하고 처음부터 다시 호출

        */
    }


    void GraphModel()
    {
        graph[0] = new List<Grid> { roomList[1], roomList[4] };
        graph[1] = new List<Grid> { roomList[0], roomList[2], roomList[5] };
        graph[2] = new List<Grid> { roomList[1], roomList[3], roomList[6] };
        graph[3] = new List<Grid> { roomList[2], roomList[7] };
                            
        graph[4] = new List<Grid> { roomList[0], roomList[5], roomList[8] };
        graph[5] = new List<Grid> { roomList[1], roomList[4], roomList[6], roomList[9] };
        graph[6] = new List<Grid> { roomList[2], roomList[5], roomList[7], roomList[10] };
        graph[7] = new List<Grid> { roomList[3], roomList[6], roomList[11] };
                            
        graph[8] = new List<Grid> { roomList[4], roomList[9] };
        graph[9] = new List<Grid> { roomList[5], roomList[8], roomList[10] };
        graph[10] = new List<Grid> { roomList[6], roomList[9], roomList[11] };
        graph[11] = new List<Grid> { roomList[7], roomList[10] };
    }

}
