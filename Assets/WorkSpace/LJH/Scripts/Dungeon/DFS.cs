using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DFS : MonoBehaviour
{
    [Header("던전 방 데이터")]
    [SerializeField] private List<Grid> roomList = new List<Grid>();
    [SerializeField] private int bossRoomIndex = 3;
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

        SideRouteMake();

        // 사이드 루트 확인
        Debug.Log("사이드 루트:");
        int sideI = 0;
        foreach (var room in sideRoute)
        {
            Debug.Log($"사이드 루트 {sideI} : {room.name}");
            sideI++;
        }

        DoorCreate();

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
        List<Grid> StartGrid = new List<Grid>();

        foreach (Grid mainRoom in route)
        {
            int index = roomList.IndexOf(mainRoom);
            foreach (Grid neighbor in graph[index])
            {
                if (!visitedRoom.Contains(neighbor) && neighbor != roomList[bossRoomIndex])
                {
                    StartGrid.Add(mainRoom);
                    break;
                }
            }
        }

        if (StartGrid.Count == 0)
        {
            Debug.LogWarning("사이드루트를 만들 수 있는 지점이 없습니다.");
            return;
        }

        Grid entryPoint = StartGrid[Random.Range(0, StartGrid.Count)];
        int entryIndex = roomList.IndexOf(entryPoint);

        sideRoute.Add(entryPoint);

        visitedRoom.Add(entryPoint);

        HashSet<Grid> sideVisited = new HashSet<Grid>();

        sideVisited.Add(entryPoint);

        foreach (Grid neighbor in graph[entryIndex])
        {
            if (!visitedRoom.Contains(neighbor) && neighbor != roomList[bossRoomIndex])
            {
                SideRouteDFS(neighbor, sideVisited);
                break;
            }
        }
    }

    private void SideRouteDFS(Grid current, HashSet<Grid> sideVisited)
    {
        sideRoute.Add(current);
        sideVisited.Add(current);
        visitedRoom.Add(current);

        int index = roomList.IndexOf(current);
        foreach (Grid neighbor in graph[index])
        {
            if (!visitedRoom.Contains(neighbor) && !sideVisited.Contains(neighbor) && neighbor != roomList[bossRoomIndex])
            {
                SideRouteDFS(neighbor, sideVisited);
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

        }
    }

    private void TryAddNeighbor(int from, int to)
    {
        if (to >= 0 && to < roomList.Count)
            graph[from].Add(roomList[to]);
    }

    private void DoorCreate()
    {
        int endRoomIndex = route.Count - 1;

        for (int i = 0; i <= endRoomIndex; i++)
        {
            int firstDoor = -1;
            int secondDoor = -1;

            //진행 방향의 문
            if (i != endRoomIndex)
            {
                Vector2 dir = route[i + 1].transform.position - route[i].transform.position;

                firstDoor = DeltaCalculator(dir);
            }
            //이전 방향의 문
            //첫번째 방의 경우 이전 방향의 문이 없으므로 예외 처리
            if (i != 0)
            {
                Vector2 dir2 = route[i -1].transform.position - route[i].transform.position;

                secondDoor = DeltaCalculator(dir2);
            }

            //시작 방과 끝 방 에외 처리
            if (firstDoor == -1) route[i].GetComponent<Room>().ActivateTheDoor(secondDoor);
            else if (secondDoor == -1) route[i].GetComponent<Room>().ActivateTheDoor(firstDoor);
            else route[i].GetComponent<Room>().ActivateTheDoor(firstDoor, secondDoor);
        }
    }

    /// <summary>
    /// dir, dir2 를 계산해주는 함수
    /// </summary>
    /// <param name="delta"></param>
    /// <returns></returns>
    private int DeltaCalculator(Vector2 delta)
    {
        if (delta.x > 0) return (int)Direction.East;
        else if (delta.x < 0) return (int)Direction.West;
        else if (delta.y > 0) return (int)Direction.North;
        else if (delta.y < 0) return (int)Direction.South;

        return -1;
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