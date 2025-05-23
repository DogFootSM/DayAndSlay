using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

public class Node
{
    public Node parentNode; 
    public Vector2Int curPosition;
    
    //출발 노드 -> 현재 노드까지의 거리
    public int gCost;
    //현재 노드 -> 목표 노드까지 거리
    public int hCost;
    
    public int fCost  => gCost + hCost; 
}

public class AstarPath : MonoBehaviour
{
    [SerializeField] private Grid mapGrid;
    [SerializeField] private Tilemap mapTileMap;
    [SerializeField] private Tilemap obstacleTileMap;
     
    private Vector2Int[] calculateNeighborsPos = new Vector2Int[]
    {
        Vector2Int.left,
        Vector2Int.right,
        Vector2Int.up,
        Vector2Int.down,
    };

    private Monster monster;
    
    private Dictionary<Vector2Int, Node> neighborsDict = new Dictionary<Vector2Int, Node>();
    private List<Node> openList = new List<Node>();
    private List<Node> closedList = new List<Node>();
    
    private Node startNode;
    private Node currentNode;
     
    public Vector2Int startPos;
    public Vector2Int targetPos;

    private void Awake()
    {
        monster = GetComponentInParent<Monster>();
        
        //현재 몬스터의 스폰 위치
        startPos = (Vector2Int)mapGrid.WorldToCell(transform.position);
        
        startNode = new Node();
        currentNode = new Node(); 
    }
    
    /// <summary>
    /// Player 감지 시마다 시작 위치, 목표 위치 설정
    /// </summary>
    /// <param name="startPos">몬스터 현재 위치</param>
    /// <param name="targetPos">이동 목표 위치</param>
    public void DetectTarget(Vector2 startPos, Vector2 targetPos)
    {
        this.startPos = (Vector2Int)mapGrid.WorldToCell(startPos);
        this.targetPos = (Vector2Int)mapGrid.WorldToCell(targetPos);
        Debug.Log("플레이어 감지, 플레이어 추적 시작");
        
        FindPath(this.startPos, this.targetPos);
    }

    /// <summary>
    /// 경로 탐색
    /// </summary>
    private void FindPath(Vector2Int startPos, Vector2Int targetPos)
    {
        openList.Clear();
        closedList.Clear();
        neighborsDict.Clear(); 
        
        //시작 노드 초기화
        startNode.curPosition = startPos;
        startNode.gCost = 0;
        startNode.hCost = Heuristic(startPos, targetPos);
        
        openList.Add(startNode);
         
        while (openList.Count > 0)
        { 
            currentNode = openList[0];
            
            for (int i = 0; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost ||
                    openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }
            
            openList.Remove(currentNode);
            closedList.Add(currentNode);
             
            //현재 노드의 위치가 타겟 위치일 경우 경로 추적 종료
            if (currentNode.curPosition == targetPos)
            {
                //경로 검색 완료
                Trace();
                return;
            }
              
            foreach (Node neighbors in GetNeighbors(currentNode))
            { 
                if(closedList.Contains(neighbors)) continue;
                
                //출발지 - 현재 노드까지 이동 가중치에 다음 노드로 이동할 + 10
                int gCost = currentNode.gCost + 10;
                
                if (gCost < neighbors.gCost || !openList.Contains(neighbors))
                {
                    neighbors.gCost = gCost;
                    neighbors.hCost = Heuristic(neighbors.curPosition, targetPos);
                    neighbors.parentNode = currentNode;

                    if (!openList.Contains(neighbors))
                    { 
                        openList.Add(neighbors);
                    }
                } 
            } 
        } 
    }
 
    /// <summary>
    /// 이웃 노드 반환
    /// </summary>
    /// <param name="node">현재 노드</param>
    /// <returns></returns>
    private List<Node> GetNeighbors(Node node)
    { 
        List<Node> neighbors = new List<Node>();

        foreach (Vector2Int neighborPos in calculateNeighborsPos)
        {
            //현재 노드의 주변 노드 위치 값
            Vector2Int nextPos = node.curPosition + neighborPos;

            if(!IsWalkable(nextPos)) continue; 
            
            Node neighborNode = GetNextNode(nextPos); 
            neighbors.Add(neighborNode); 
        }
        
        return neighbors;    
    }

    /// <summary>
    /// 이웃 노드 객체 반환
    /// </summary>
    /// <param name="nodePos"></param>
    /// <returns></returns>
    private Node GetNextNode(Vector2Int nodePos)
    {
        if (!neighborsDict.ContainsKey(nodePos))
        {
            Node node = new Node();
            neighborsDict[nodePos] = node;
        }
        
        neighborsDict[nodePos].curPosition = nodePos; 
        return neighborsDict[nodePos];
    }
    
    /// <summary>
    /// 이동 가능 경로 확인
    /// </summary>
    /// <param name="nextPos">이동할 다음 위치</param>
    /// <returns></returns>
    private bool IsWalkable(Vector2Int nextPos)
    {
        //맵의 범위를 넘어서지 않는지?
        if (!mapTileMap.cellBounds.Contains((Vector3Int)nextPos)) return false;
        
        //장애물 위치인지?
        if (obstacleTileMap.HasTile((Vector3Int)nextPos)) return false;

        return true;
    } 
    
    /// <summary>
    /// 캐릭터 추적
    /// </summary>
    private void Trace()
    {
        Debug.Log("추적");
        List<Vector3> path = new List<Vector3>();
        
        Node curNode = currentNode;
        
        while (curNode != null)
        {
            Vector3 worldPos = mapTileMap.CellToWorld((Vector3Int)curNode.curPosition);
            path.Add(worldPos + new Vector3(0.5f, 0.5f,0));
            curNode = curNode.parentNode;
        }
        
        path.Reverse();

        //StartCoroutine(Move(path));
        for (int i = 0; i < path.Count - 1; i++)
        {
            Debug.Log(path[i]);
            Debug.DrawLine(path[i], path[i + 1], Color.red, 2f);
        }
    }

    // private IEnumerator Move(List<Vector3> path)
    // {
    //      
    //     
    // }
    
    /// <summary>
    /// 거리 계산, 가로, 세로 이동
    /// </summary>
    /// <param name="beforePos"></param>
    /// <param name="afterPos"></param>
    /// <returns></returns>
    private int Heuristic(Vector2Int x1, Vector2Int x2)
    {
        return (Mathf.Abs(x1.x - x2.x) + Mathf.Abs(x1.y - x2.y)) * 10;
    }
    
    
}