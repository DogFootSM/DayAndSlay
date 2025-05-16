using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestDFS : MonoBehaviour
{
    [Header("던전 방 데이터")]
    [SerializeField] private List<Grid> roomList = new List<Grid>();
    [SerializeField] private int bossRoomIndex = 3;
    [SerializeField] private int extraConnections = 3;
    [SerializeField] private int gridWidth = 4; // 4x3 그리드 기준

    [Header("시각화")]
    [SerializeField] private LineRenderer mainRouteLineRenderer;
    [SerializeField] private LineRenderer sideRouteLineRenderer;

    /// <summary>
    /// 방문한 방 HashSet
    /// </summary>
    private HashSet<Grid> visitedRoom = new HashSet<Grid>();

    /// <summary>
    /// 보스방 까지의 경로 List
    /// </summary>
    private List<Grid> route = new List<Grid>();
    
    private Dictionary<int, List<Grid>> graph = new Dictionary<int, List<Grid>>();
    private List<(Grid, Grid)> extraConnectionsList = new List<(Grid, Grid)> ();

    //사이드 루트 담아주는 List
    private List<Grid> sideRoomList = new List<Grid> ();

    //사이드 루트 경로
    private List<Grid> sideRoute = new List<Grid> ();
    private void Start()
    {
        if (roomList.Count <= bossRoomIndex)
        {
            Debug.LogError("roomList에 충분한 방이 없습니다.");
            return;
        }

        GraphModel();

        if (!RouteMake(0))
        {
            Debug.LogError("보스방에 도달하지 못했습니다.");
            return;
        }

        Debug.Log("메인 루트:");
        foreach (var room in route)
        {
            Debug.Log(room.name); // 또는 roomList.IndexOf(room)
        }

        SideRouteMake();

        // 사이드 루트 확인
        Debug.Log("사이드 루트:");
        foreach (var room in sideRoute)
        {
            Debug.Log(room.name); // 또는 roomList.IndexOf(room)
        }

        DrawRouteLines();
    }

    /// <summary>
    /// DFS로 보스방까지의 경로를 생성한다 (재귀 + 백트래킹)
    /// </summary>
    private bool RouteMake(int curRoomIndex)
    {
        //현재 방을 current에 넣어줌
        Grid current = roomList[curRoomIndex];
        //방문한 방 목록에 current 를 넣어줌
        visitedRoom.Add(current);
        //경로 목록에 current를 넣어줌
        route.Add(current);

        //보스방에 도착한 경우 DFS알고리즘 끝냄
        if (curRoomIndex == bossRoomIndex)
        {
            Debug.Log("보스방 도착");
            return true;
        }

        //현재 방에 인접한 방들을 neighbors에 넣어줌
        List<Grid> neighbors = graph[curRoomIndex];
        //방문하지 않은 방 목록 생성
        List<int> unvisitedIndices = new List<int>();

        //현재 방에 인접한 방 중 방문하지 않은 방을 방문하지 않은 방 목록에 넣어줌
        foreach (Grid neighbor in neighbors)
        {
            int index = roomList.IndexOf(neighbor);
            if (!visitedRoom.Contains(neighbor))
                unvisitedIndices.Add(index);
        }

        //방문하지 않은 방 목록을 섞어줌
        Shuffle(unvisitedIndices);

        //방문하지 않은 방 목록마다 RootMake 재귀
        foreach (int nextIndex in unvisitedIndices)
        {
            if (RouteMake(nextIndex))
                return true;
        }

        // 조건이 맞지 않았다면 방문한 방 목록, 경로에서 현재 방을 제거하고 false로 리턴(백트래킹)
        visitedRoom.Remove(current);
        route.Remove(current);
        return false;
    }

    private void SideRouteMake()
    {
        sideRoomList = roomList;

        Dictionary<Grid, int> roomDict = new Dictionary<Grid, int>();

        for (int i = 0; i < sideRoomList.Count; i++)
        {
            roomDict.Add(sideRoomList[i], i);
        }


        // 메인 루트에 포함된 방은 제거
        for (int i = sideRoomList.Count - 1; i >= 0; i--)
        {
            if (route.Contains(sideRoomList[i]))
            {
                roomDict.Remove(sideRoomList[i]); // Index 접근 필요 없음
                sideRoomList.RemoveAt(i);
            }
        }

        //방문하지 않은 방중에 첫번째 방과 그래프 연결된 방 중 하나를 50퍼 확률로 루트에 넣어줘야함

        if (sideRoute.Count == 0)
        {
            for (int i = 0; i < sideRoomList.Count; i++)
            {
                if (Random.Range(0, 2) == 1)
                {
                    int graphKey = roomDict[sideRoomList[i]];

                    if (graph[graphKey].Count == 0)
                    {
                        Debug.Log("에러 : 그래프의 내용물이 비어있습니다.");
                        return;
                    }

                    Grid randGrid = graph[graphKey][Random.Range(0, graph[graphKey].Count)];

                    Debug.Log($"첫번째 사이드 루트 그리드의 이름 {randGrid.name}");

                    sideRoute.Add(randGrid);
                    visitedRoom.Add(randGrid);

                    Debug.Log($"두번째 사이드 루트 그리드의 이름 {sideRoomList[i].name}");

                    sideRoute.Add(sideRoomList[i]);
                    visitedRoom.Add(sideRoomList[i]);


                }
            }
        }


    }


    /// <summary>
    /// 각 방을 기준으로 상하좌우 이웃 연결 그래프 생성
    /// </summary>
    private void GraphModel()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            graph[i] = new List<Grid>();

            int up = i - gridWidth;
            int down = i + gridWidth;
            int left = (i % gridWidth != 0) ? i - 1 : -1;
            int right = (i % gridWidth != gridWidth - 1) ? i + 1 : -1;

            TryAddNeighbor(i, up);
            TryAddNeighbor(i, down);
            TryAddNeighbor(i, left);
            TryAddNeighbor(i, right);

            for (int j = 0; j < graph[i].Count; j++)
            {
                Debug.Log($"{i}번 그래프 : {graph[i][j]}");
            }
        }
    }

    private void TryAddNeighbor(int from, int to)
    {
        if (to >= 0 && to < roomList.Count)
            graph[from].Add(roomList[to]);
    }



    /// <summary>
    /// DFS 경로 + 사이드 루트 경로 라인 렌더링
    /// </summary>
    private void DrawRouteLines()
    {
        // 메인 루트 그리기
        mainRouteLineRenderer.positionCount = route.Count;
        for (int i = 0; i < route.Count; i++)
        {
            mainRouteLineRenderer.SetPosition(i, route[i].transform.position);
        }

        // 사이드 루트 그리기 (중간 연결)
        sideRouteLineRenderer.positionCount = sideRoute.Count;
        for (int i = 0; i < sideRoute.Count; i++)
        {
            sideRouteLineRenderer.SetPosition(i, sideRoute[i].transform.position);
        }
    }

    /// <summary>
    /// 리스트 셔플
    /// </summary>
    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}