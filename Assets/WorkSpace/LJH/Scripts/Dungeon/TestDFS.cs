using System.Collections;
using System.Collections.Generic;
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

    private HashSet<Grid> visitedRoom = new HashSet<Grid>();
    private List<Grid> route = new List<Grid>();
    private Dictionary<int, List<Grid>> graph = new Dictionary<int, List<Grid>>();
    private List<(Grid, Grid)> extraConnectionsList = new List<(Grid, Grid)> ();

    private void Start()
    {
        if (roomList.Count <= bossRoomIndex)
        {
            Debug.LogError("roomList에 충분한 방이 없습니다.");
            return;
        }

        GraphModel();

        if (!RootMake(0))
        {
            Debug.LogError("보스방에 도달하지 못했습니다.");
            return;
        }

        AddExtraConnections();
        DrawRouteLines();
    }

    /// <summary>
    /// DFS로 보스방까지의 경로를 생성한다 (재귀 + 백트래킹)
    /// </summary>
    private bool RootMake(int curRoomIndex)
    {
        Grid current = roomList[curRoomIndex];
        visitedRoom.Add(current);
        route.Add(current);

        if (curRoomIndex == bossRoomIndex)
        {
            Debug.Log("보스방 도착");
            return true;
        }

        List<Grid> neighbors = graph[curRoomIndex];
        List<int> unvisitedIndices = new List<int>();

        foreach (Grid neighbor in neighbors)
        {
            int index = roomList.IndexOf(neighbor);
            if (!visitedRoom.Contains(neighbor))
                unvisitedIndices.Add(index);
        }

        Shuffle(unvisitedIndices);

        foreach (int nextIndex in unvisitedIndices)
        {
            if (RootMake(nextIndex))
                return true;
        }

        visitedRoom.Remove(current);
        route.Remove(current);
        return false;
    }

    /// <summary>
    /// 사이드 루트 추가 (보스방 제외, 상하좌우만 연결 허용)
    /// </summary>
    private void AddExtraConnections()
    {
        int count = 0;
        int maxAttempts = 1000;
        int attempts = 0;

        while (count < extraConnections && attempts < maxAttempts)
        {
            attempts++;

            int a = Random.Range(0, roomList.Count);
            int b = Random.Range(0, roomList.Count);

            if (a == b || a == bossRoomIndex || b == bossRoomIndex)
                continue;

            if (!IsAdjacentInGrid(a, b))
                continue;

            Grid roomA = roomList[a];
            Grid roomB = roomList[b];

            if (!graph[a].Contains(roomB))
            {
                graph[a].Add(roomB);
                graph[b].Add(roomA);
                extraConnectionsList.Add((roomA, roomB));
                Debug.Log($"사이드 루트 연결: {a} ↔ {b}");
                count++;
            }
        }

        if (attempts >= maxAttempts)
            Debug.LogWarning("사이드 루트 추가 시도 제한 도달");
    }

    /// <summary>
    /// 각 방을 기준으로 상하좌우 이웃 연결 그래프 생성
    /// </summary>
    private void GraphModel()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            graph[i] = new List<Grid>();

            // 상하좌우 인접 인덱스 계산
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
    /// 두 인덱스가 상하좌우 인접한지 확인
    /// </summary>
    private bool IsAdjacentInGrid(int a, int b)
    {
        // 좌우
        if (Mathf.Abs(a - b) == 1 && a / gridWidth == b / gridWidth)
            return true;

        // 상하
        if (Mathf.Abs(a - b) == gridWidth)
            return true;

        return false;
    }

    /// <summary>
    /// DFS 경로 + 사이드 루트 경로 라인 렌더링
    /// </summary>
    private void DrawRouteLines()
    {
        mainRouteLineRenderer.positionCount = route.Count;
        for (int i = 0; i < route.Count; i++)
        {
            mainRouteLineRenderer.SetPosition(i, route[i].transform.position);
        }

        sideRouteLineRenderer.positionCount = extraConnectionsList.Count * 2;
        int idx = 0;
        foreach (var pair in extraConnectionsList)
        {
            sideRouteLineRenderer.SetPosition(idx, pair.Item1.transform.position);
            sideRouteLineRenderer.SetPosition(idx + 1, pair.Item2.transform.position);
            idx += 2;
        }
    }

    /// <summary>
    /// 리스트 셔플 (Fisher-Yates)
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