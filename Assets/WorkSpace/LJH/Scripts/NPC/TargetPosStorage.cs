using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TargetPosStorage : MonoBehaviour
{
    [Header("Grid & TileMap / 0 = outside, 1 = store")]
    [SerializeField] private List<Grid> gridList;
    [SerializeField] private List<Tilemap> mapTile;
    [SerializeField] private List<Tilemap> obstacleTile;

    [SerializeField] private GameObject StoreDoor;
    [SerializeField] private GameObject OutsideDoor;
    [SerializeField] private GameObject castleDoor;
    [SerializeField] private GameObject player;

    public Vector3 StoreDoorPos => StoreDoor.transform.position;
    public Vector3 OutsideDoorPos => OutsideDoor.transform.position;
    public Vector3 CastleDoor => castleDoor.transform.position;
    public Vector3 PlayerPos => playerPos;

    private Vector3 playerPos;

    WaitForSeconds delay = new WaitForSeconds(1f);

    private void Start()
    {
        Debug.Log(StoreDoorPos);
        StartCoroutine(SearchPlayer());
    }

    private IEnumerator SearchPlayer()
    {
        yield return delay;

        while (player == null)
        {
            yield return delay;
            player = GameObject.FindWithTag("Player");
        }

        while (true)
        {
            if(Vector3.Distance(PlayerPos, player.transform.position) >= 0.1f)
            {
                playerPos = player.transform.position;
            }

            yield return delay;
        }
    }

    public List<Grid> SetGrid(List<Grid> gridList) => gridList = this.gridList;
    public List<Tilemap> SetMaptile(List<Tilemap> mapTile) => mapTile = this.mapTile;
    public List<Tilemap> SetObstacletile(List<Tilemap> obstacleTile) => obstacleTile = this.obstacleTile;
}
