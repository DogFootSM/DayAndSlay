using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapLoader : MonoBehaviour
{
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap mapTilemap;
    [SerializeField] private Tilemap objectTilemap;

    [SerializeField] private List<Tile> tileList; // index 0=벽, 1=바닥, 2=오브젝트 등

    [SerializeField] private string csvFileName = "Maps/map01"; // Resources 폴더 기준

    void Start()
    {
        LoadMap(csvFileName);
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

                if (int.TryParse(cell, out int tileIndex) && tileIndex >= 0 && tileIndex < tileList.Count)
                {
                    Vector3Int pos = new Vector3Int(x, -y, 0); // y 뒤집는 건 유니티 기준

                    switch (tileIndex)
                    {
                        case 0:
                            wallTilemap.SetTile(pos, tileList[tileIndex]);
                            break;
                        case 1:
                            mapTilemap.SetTile(pos, tileList[tileIndex]);
                            break;
                        case 2:
                            objectTilemap.SetTile(pos, tileList[tileIndex]);
                            break;
                    }
                }
            }
        }
    }
}
