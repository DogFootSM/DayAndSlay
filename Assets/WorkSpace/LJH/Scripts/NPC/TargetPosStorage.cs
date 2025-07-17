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

    [SerializeField] private List<GameObject> randomPosList;
    [SerializeField] private GameObject randomPos;

    public Vector3 StoreDoorPos => StoreDoor.transform.position;
    public Vector3 OutsideDoorPos => OutsideDoor.transform.position;
    public Vector3 CastleDoor => castleDoor.transform.position;
    public Vector3 PlayerPos => playerPos;
    public Vector3 RandomPos => randomPos.transform.position;

    private Vector3 playerPos;

    WaitForSeconds delay = new WaitForSeconds(1f);

    private void Start()
    {
        StartCoroutine(SearchPlayer());
        StartCoroutine(ShuffleRandomPosCoroutine());
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

    private IEnumerator ShuffleRandomPosCoroutine()
    {
        while (true)
        {
            randomPos = randomPosList[Random.Range(0, randomPosList.Count - 1)];

            yield return delay;
        }
    }

    public List<Grid> GetGridList() => gridList;
    public List<Tilemap> GetMapTileList() => mapTile;
    public List<Tilemap> GetObstacleTileList() => obstacleTile;
}
