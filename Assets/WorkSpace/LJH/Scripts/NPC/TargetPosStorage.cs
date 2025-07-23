using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TargetPosStorage : MonoBehaviour
{
    [Header("Grid & TileMap / 0 = outside, 1 = store")]
    [SerializeField] private List<Grid> gridList;
    [SerializeField] private List<Tilemap> mapTile;
    [SerializeField] private List<Tilemap> obstacleTile;

    [SerializeField] private GameObject storeDoor;
    [SerializeField] private GameObject outsideDoor;
    [SerializeField] private GameObject castleDoor;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject desk;
    [SerializeField] private GameObject fishingPos;
    [SerializeField] private GameObject loggingPos;

    [SerializeField] private List<GameObject> fishingRandomPosList;
    [SerializeField] private GameObject fishingRandomPos;
    [SerializeField] private List<GameObject> loggingRandomPosList;
    [SerializeField] private GameObject loggingRandomPos;

    [SerializeField] private List<GameObject> randomPosList;
    [SerializeField] private GameObject randomPos;
    [SerializeField] private List<GameObject> randomPosInStoreList;
    [SerializeField] private GameObject randomPosInStore;
    public Vector3 StoreDoorPos => storeDoor.transform.position;
    public Vector3 OutsideDoorPos => outsideDoor.transform.position;
    public Vector3 CastleDoorPos => castleDoor.transform.position + new Vector3(0, -4, 0);
    public Vector3 PlayerPos => playerPos;
    public Vector3 DeskPos => desk.transform.position + new Vector3(-1.5f, 0.5f, 0);
    public Vector3 FishingPos => fishingPos.transform.position;
    public Vector3 LoggingPos => loggingPos.transform.position;
    public Vector3 RandomPos => randomPos.transform.position;
    public Vector3 RandomPosInStore => randomPosInStore.transform.position;

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
            randomPosInStore = randomPosInStoreList[Random.Range(0, randomPosInStoreList.Count - 1)];

            yield return delay;
        }
    }

    public List<Grid> GetGridList() => gridList;
    public List<Tilemap> GetMapTileList() => mapTile;
    public List<Tilemap> GetObstacleTileList() => obstacleTile;
}
