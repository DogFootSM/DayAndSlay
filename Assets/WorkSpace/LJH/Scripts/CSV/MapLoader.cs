using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLoader : MonoBehaviour
{
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap mapTilemap;
    [SerializeField] private Tilemap objectTilemap;
    [SerializeField] private Tilemap waterTilemap;

    [SerializeField] private List<Tile> wallTileList; // index 0=벽, 1=바닥, 2=오브젝트 3=물
    [SerializeField] private List<Tile> floorTileList; // index 0=벽, 1=바닥, 2=오브젝트 3=물
    [SerializeField] private List<Tile> objectTileList; // index 0=벽, 1=바닥, 2=오브젝트 3=물
    [SerializeField] private List<Tile> waterTileList; // index 0=벽, 1=바닥, 2=오브젝트 3=물

    private DictList<List<Tile>> tileDictList = new DictList<List<Tile>>();
    [SerializeField] private List<string> csvFileNames;

    void Start()
    {
        TileDictMaker();
        LoadMap(csvFileNames[Random.Range(0, csvFileNames.Count)]);
    }

    void LoadMap(string fileName)
    {
        TextAsset mapData = Resources.Load<TextAsset>(fileName);
        string[] lines = mapData.text.Split('\n');

        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = lines[y].Trim().Split(',');

            for (int x = 0; x < row.Length; x++)
            {
                string cell = row[x].Trim();
                if (string.IsNullOrWhiteSpace(cell)) cell = "1"; // 빈칸은 바닥 처리

                if (int.TryParse(cell, out int tileIndex) && tileIndex >= 0 && tileIndex < tileDictList.Count)
                {
                    Vector3Int pos = new Vector3Int(x - 20, -y + 20, 0); // y 뒤집는 건 유니티 기준

                    switch (tileIndex)
                    {
                        case 0:
                            wallTilemap.SetTile(pos, tileDictList[tileIndex][Random.Range(0, wallTileList.Count)]);
                            break;
                        case 1:
                            mapTilemap.SetTile(pos, tileDictList[tileIndex][Random.Range(0, floorTileList.Count)]);
                            break;
                        case 2:
                            objectTilemap.SetTile(pos, tileDictList[tileIndex][Random.Range(0, objectTileList.Count)]);
                            break;
                        case 3:
                            waterTilemap.SetTile(pos, tileDictList[tileIndex][Random.Range(0, waterTileList.Count)]);
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 타일 리스트에 들어있는 요소를 딕녀너리로 넣어줌
    /// </summary>
    void TileDictMaker()
    {
            tileDictList.Add($"wall", wallTileList);
            tileDictList.Add($"floor", floorTileList);
            tileDictList.Add($"object", objectTileList);
            tileDictList.Add($"water", waterTileList);
    }
}
