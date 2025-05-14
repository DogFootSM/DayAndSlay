using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DFS : MonoBehaviour
{
    /// <summary>
    /// 던전을 구성할 방 프리팹 리스트 (Grid는 각 방의 위치 및 정보)
    /// </summary>
    [SerializeField] private List<Grid> roomList = new List<Grid>();

    /// <summary>
    /// DFS로 방문한 방들을 저장하는 해시셋 (중복 방지)
    /// </summary>
    private HashSet<Grid> visitedRoom = new HashSet<Grid>();

    /// <summary>
    /// DFS로 순차 방문한 방들을 저장하는 리스트 (경로 시각화용)
    /// </summary>
    private List<Grid> route = new List<Grid>();

    /// <summary>
    /// 각 방 인덱스에 연결된 이웃 방 목록 (그래프 구조)
    /// </summary>
    private Dictionary<int, List<Grid>> graph = new Dictionary<int, List<Grid>>();

    /// <summary>
    /// DFS 경로를 라인으로 표시할 라인렌더러
    /// </summary>
    [SerializeField] private LineRenderer lineRenderer;

    /// <summary>
    /// 사이드 루트를 시각화할 라인렌더러 (색상 다르게 표현)
    /// </summary>
    [SerializeField] private LineRenderer sideLineRenderer;

    /// <summary>
    /// 도달해야 할 보스방 인덱스
    /// </summary>
    [SerializeField] private int bossRoomIndex = 3;

    /// <summary>
    /// DFS 루트 외에 추가로 연결할 사이드 루트 개수
    /// </summary>
    [SerializeField] private int extraConnections = 3;

    /// <summary>
    /// 연결된 사이드 루트 저장용 리스트 (쌍으로 저장)
    /// </summary>
    private List<(Grid, Grid)> extraConnectionsList = new List<(Grid, Grid)>();

    private void Start()
    {
        GraphModel();

        if (!RootMake(0))
        {
            Debug.LogError("보스방에 도달하지 못했습니다.");
        }

        AddExtraConnections();
        DrawRouteLine();
    }



    /// <summary>
    /// DFS 기반으로 보스방까지 경로를 재귀적으로 생성
    /// </summary>
    /// <summary>
    /// DFS를 통해 보스방까지 도달하는 경로를 생성하며, 성공 시 true 반환
    /// </summary>
    bool RootMake(int curRoomIndex)
    {
        Grid current = roomList[curRoomIndex];
        visitedRoom.Add(current);
        route.Add(current);

        if (curRoomIndex == bossRoomIndex)
        {
            Debug.Log("보스방 도착");
            return true; // 도착 성공
        }

        List<Grid> neighbors = graph[curRoomIndex];
        List<int> unvisitedIndices = new List<int>();

        foreach (Grid neighbor in neighbors)
        {
            int index = roomList.IndexOf(neighbor);
            if (!visitedRoom.Contains(neighbor))
                unvisitedIndices.Add(index);
        }

        // 랜덤한 순서로 시도
        Shuffle(unvisitedIndices);

        foreach (int nextIndex in unvisitedIndices)
        {
            if (RootMake(nextIndex)) // 재귀 성공 시 그대로 리턴
                return true;
        }

        // 실패했으면 백트래킹
        visitedRoom.Remove(current);
        route.Remove(current);
        return false;
    }

    /// <summary>
    /// 리스트의 순서를 랜덤하게 섞는다 (Fisher-Yates)
    /// </summary>
    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    /// <summary>
    /// 메인 루트 외에도 무작위로 방들 사이에 연결을 추가하여 던전을 복잡하게 만듦
    /// </summary>
    void AddExtraConnections()
    {
        int count = 0;
        while (count < extraConnections)
        {
            int a = Random.Range(0, roomList.Count);
            int b = Random.Range(0, roomList.Count);

            if (a == b || a == bossRoomIndex || b == bossRoomIndex) continue;
            if (!IsAdjacentInGrid(a, b)) continue; // 대각선 방지

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
    }

    /// <summary>
    /// 두 인덱스가 상하좌우 인접한지 확인 (4x3 그리드 기준)
    /// </summary>
    bool IsAdjacentInGrid(int a, int b)
    {
        int width = 4; // 가로 너비 (고정값 or 변수로 바꿔도 됨)

        // 좌우
        if (Mathf.Abs(a - b) == 1 && Mathf.Min(a, b) / width == Mathf.Max(a, b) / width)
            return true;

        // 상하
        if (Mathf.Abs(a - b) == width)
            return true;

        return false;
    }

    /// <summary>
    /// DFS로 탐색된 경로를 라인렌더러를 통해 시각적으로 표시
    /// </summary>
    void DrawRouteLine()
    {
        // 메인 루트
        lineRenderer.positionCount = route.Count;
        for (int i = 0; i < route.Count; i++)
        {
            lineRenderer.SetPosition(i, route[i].transform.position);
        }

        // 사이드 루트 라인 초기화
        sideLineRenderer.positionCount = extraConnectionsList.Count * 2;

        int idx = 0;
        foreach (var pair in extraConnectionsList)
        {
            sideLineRenderer.SetPosition(idx, pair.Item1.transform.position);
            sideLineRenderer.SetPosition(idx + 1, pair.Item2.transform.position);
            idx += 2;
        }
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
