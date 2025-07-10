using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.PlayerSettings;

public class TargetSensorInNPC : TargetSensor
{

    private GameObject target;
    private GameObject table;
    private Grid currentGrid;
    private List<Grid> grids;

    private bool isBuying = false; // 예시용, 실제로는 NPC 상태값으로 바인딩해야 함

    private void Start()
    {
        findRange = 20f;
        grids = FindObjectsOfType<Grid>().ToList();
        SetCurrentGridAndTargets();
    }

    private void SetCurrentGridAndTargets()
    {
        Vector3 curPos = transform.position;

        foreach (Grid grid in grids)
        {
            Tilemap tilemap = grid.GetComponentInChildren<Tilemap>();
            if (tilemap == null) continue;

            BoundsInt cellBounds = tilemap.cellBounds;
            Vector3Int cellPos = grid.WorldToCell(curPos);

            if (cellBounds.Contains(cellPos))
            {
                currentGrid = grid;

                if (grid.name == "OutsideGrid")
                {
                    target = GameObject.Find("OutsideDoor");
                }
                else if (grid.name == "Store1fGrid")
                {
                    //Todo : NPC가 물건을 구매 완료함 or 시간이 지남 일땐  문으로 이동
                    Table table = GetComponentInParent<NPC>()._table;

                    if (table != null)
                    {
                        target = table.gameObject;
                    }
                    else
                    {
                        //target = player.gameObject;
                    }

                    target = GameObject.Find("StoreDoor");
                }


                ChangeGridInAstar(grid);
                break;
            }
        }
    }

    private void ChangeGridInAstar(Grid grid)
    {
        astar.SetGridAndTilemap(grid);
        this.grid = grid;
    }

    protected override void TriggerEnterMethod(Collider2D other)
    {
        Vector3 targetPos = GetTargetPosition();

        lastPlayerCell = grid.WorldToCell(targetPos);

        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            Debug.Log("디텍트타겟 실행");
            astar.DetectTarget(transform.position, other.gameObject.transform.position);
        }
    }

    protected override void TriggerStayMethod(Collider2D collision)
    {
        if (target == player)
        {
            Vector3 targetPos = GetTargetPosition();
            Vector3Int currentCell = grid.WorldToCell(targetPos);

            if (Time.time >= nextCheckTime)
            {
                if (currentCell != lastPlayerCell)
                {
                    astar.DetectTarget(transform.position, targetPos);
                    lastPlayerCell = currentCell;
                }

                nextCheckTime = Time.time + interval;
            }
        }
        else
        {
            if (Time.time >= nextCheckTime)
            {
                Debug.Log(target);
                astar.DetectTarget(transform.position, target.transform.position);
                nextCheckTime = Time.time + interval;
            }
        }
    }

    private Vector3 GetTargetPosition()
    {
        if (currentGrid == null) return transform.position;

        if (currentGrid.name == "OutsideGrid")
        {
            return target != null ? target.transform.position : transform.position;
        }
        else if (currentGrid.name == "Store1fGrid")
        {
            if (isBuying && table != null)
            {
                return table.transform.position;
            }
            else if (target != null)
            {
                return target.transform.position;
            }
        }

        return transform.position;
    }
}
