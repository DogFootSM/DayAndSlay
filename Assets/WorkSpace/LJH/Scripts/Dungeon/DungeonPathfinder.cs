using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DungeonPathfinder : MonoBehaviour
{
    [SerializeField] DungeonRoomSpawner spawner;
    
    [Header("던전 방 데이터")]
    [SerializeField] private List<Grid> roomList = new List<Grid>();
    [SerializeField] private int bossRoomIndex = 2;
    [SerializeField] private int gridWidth = 3; // 3x2 그리드 기준

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
    private List<Grid> reverseRoute = new List<Grid>();

    private Dictionary<int, List<Grid>> graph = new Dictionary<int, List<Grid>>();

    //사이드 루트 경로
    private List<Grid> sideRoute = new List<Grid>();
    private List<Grid> reverseSideRoute = new List<Grid>();

    private Dictionary<Grid, int> roomDict = new Dictionary<Grid, int>();
    private void Start()
    {
        if (roomList.Count <= bossRoomIndex)
        {
            Debug.LogError("roomList에 충분한 방이 없습니다.");
            return;
        }

        GraphModel();

        for (int i = 0; i < roomList.Count; i++)
        {
            roomDict[roomList[i]] = i;
        }

        if (!RouteMake(0))
        {
            Debug.LogError("보스방에 도달하지 못했습니다.");
            return;
        }

        SideRouteMake();
        ReverseRoute();
        StartCoroutine(DoorBuilder());

        DrawRouteLines();
    }

    /// <summary>
    /// 돌아가는 문을 생성하기 위한 루트 뒤집어주는 메서드
    /// </summary>
    private void ReverseRoute()
    {
        List<Grid> tempRoute = new List<Grid>(route);
        tempRoute.Reverse();
        reverseRoute = tempRoute;

        if (sideRoute.Count < 1) return;
        
        List<Grid> tempSideRoute = new List<Grid>(sideRoute);
        tempSideRoute.Reverse();
        reverseSideRoute = tempSideRoute;
    }

    /// <summary>
    /// 문 생성 텀 주기위한 코루틴
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoorBuilder()
    {
        yield return new WaitForSeconds(0.5f);
        
        spawner.BuildDoor(route, false);
        spawner.BuildDoor(sideRoute, false);
        spawner.BuildDoor(reverseRoute, true);
        spawner.BuildDoor(reverseSideRoute, true);
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
            int index = roomDict[neighbor];
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
    
    /// <summary>
    /// 사이드 루트 생성
    /// </summary>
    private void SideRouteMake()
    {
        sideRoute.Clear();
        if (route == null || route.Count == 0) return;

        HashSet<Grid> main = new HashSet<Grid>(route);

        // 사이드 루트 시작할 후보 수집
        List<(Grid entry, Grid neighbor)> candidates = new List<(Grid, Grid)>();
        foreach (Grid r in route)
        {
            if (r == roomList[bossRoomIndex]) continue; // 보스 방은 제외

            int idx = roomDict[r];
            foreach (Grid nb in graph[idx])
            {
                if (!main.Contains(nb) && nb != roomList[bossRoomIndex])
                    candidates.Add((r, nb));
            }
        }

        if (candidates.Count == 0)
        {
            Debug.LogWarning("사이드루트 후보가 없습니다. (메인과 인접한 비-메인 방이 없음)");
            return;
        }

        // 2) 임의의 후보 선택
        var pick = candidates[Random.Range(0, candidates.Count)];
        Grid entry = pick.entry;
        Grid first = pick.neighbor;

        // 3) 선형 경로 구축: 메인과 겹치지 않도록 로컬 visited 사용
        HashSet<Grid> localVisited = new HashSet<Grid>(main); // 메인은 이미 방문 처리
        localVisited.Add(entry);

        sideRoute.Add(entry);                 // 라인렌더러가 메인→사이드로 자연스럽게 이어지게
        BuildSidePathLinear(first, localVisited, main);

        Debug.Log($"SideRoute length = {sideRoute.Count} (entry={roomDict[entry]})");
    }

/// <summary>
/// 사이드루트 경로 생성
/// </summary>
/// <param name="current"></param>
/// <param name="visited"></param>
/// <param name="main"></param>
    private void BuildSidePathLinear(Grid current, HashSet<Grid> visited, HashSet<Grid> main)
    {
        while (true)
        {
            if (!visited.Add(current)) break;     // 이미 방문(또는 메인)이면 종료
            sideRoute.Add(current);

            int idx = roomDict[current];

            // 다음 후보: 아직 방문 안 했고 메인도 아니고 보스도 아닌 이웃들
            List<Grid> nextCandidates = new List<Grid>();
            foreach (Grid nb in graph[idx])
            {
                if (!visited.Contains(nb) && !main.Contains(nb) && nb != roomList[bossRoomIndex])
                    nextCandidates.Add(nb);
            }

            if (nextCandidates.Count == 0) break; // 더 못 뻗으면 종료
            current = nextCandidates[Random.Range(0, nextCandidates.Count)];
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

    /// <summary>
    /// 룸리스트 가져오기
    /// </summary>
    /// <returns></returns>
    public List<Grid> GetRoomList()
    {
        return new List<Grid>(this.roomList);
    }
    
    /// <summary>
    /// 룸리스트 세팅하기
    /// </summary>
    /// <param name="room"></param>
    public void SetRoomList(Grid room)
    {
        roomList.Add(room);
    }

}