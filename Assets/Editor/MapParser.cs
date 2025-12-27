using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class MapCsvImporter
{
    [MenuItem("Tools/Import CSV/Map Data (Create Prefab)")]
    public static void ImportMapData()
    {
        string csvFileName = "Maps/Stage2_BossMap01";
        string prefabOutputPath = $"Assets/WorkSpace/LJH/Prefabs/Maps/{Path.GetFileName(csvFileName)}.prefab";
        
        RuleTile wallTile = AssetDatabase.LoadAssetAtPath<RuleTile>("Assets/WorkSpace/LJH/Tile/WallRuleTile.asset");
        RuleTile objectTile = AssetDatabase.LoadAssetAtPath<RuleTile>("Assets/WorkSpace/LJH/Tile/ObstacleRuleTile.asset");
        RuleTile waterTile = AssetDatabase.LoadAssetAtPath<RuleTile>("Assets/WorkSpace/LJH/Tile/WaterRuleTile.asset");
        List<Tile> floorTileList = LoadTilesFromFolder("Assets/WorkSpace/LJH/Tile/Tiles");

        if (wallTile == null || objectTile == null || waterTile == null || floorTileList.Count == 0)
        {
            string message;
            if (wallTile == null)
                message = "wallTile이 null입니다";
            else if (objectTile == null)
                    message = "objectTile이 null입니다";
            else if (waterTile == null)
                message = "waterTile이 null입니다";
            else
                message = "floorTileList가 없습니다";
            
            Debug.LogError($"필요한 타일 리소스를 찾지 못했습니다. 타입 : {message}");
            return;
        }

        TextAsset mapData = Resources.Load<TextAsset>(csvFileName);
        if (mapData == null)
        {
            Debug.LogError("CSV 파일을 Resources 폴더에서 찾을 수 없습니다: " + csvFileName);
            return;
        }

        GameObject mapRoot = new GameObject("GeneratedMap");
        mapRoot.AddComponent<Grid>();

        Tilemap wall = CreateTilemap(mapRoot, "Wall");
        Tilemap floor = CreateTilemap(mapRoot, "Floor");
        Tilemap water = CreateTilemap(mapRoot, "Water");

        string[] lines = mapData.text.Split('\n');
        for (int y = 0; y < lines.Length; y++)
        {
            string[] row = lines[y].Trim().Split(',');
            for (int x = 0; x < row.Length; x++)
            {
                string cell = row[x].Trim();
                if (string.IsNullOrWhiteSpace(cell)) cell = "1";

                if (int.TryParse(cell, out int tileIndex))
                {
                    Vector3Int pos = new Vector3Int(x - 20, -y + 20, 0); // 유니티 좌표 기준으로 변환

                    switch (tileIndex)
                    {
                        case 0:
                            wall.SetTile(pos, wallTile);
                            break;
                        case 1:
                            floor.SetTile(pos, floorTileList[Random.Range(0, floorTileList.Count)]);
                            break;
                        case 2:
                            floor.SetTile(pos, floorTileList[Random.Range(0, floorTileList.Count)]);
                            wall.SetTile(pos, objectTile);
                            break;
                        case 3:
                            water.SetTile(pos, waterTile);
                            break;
                    }
                }
            }
        }

        // 프리팹 저장
        Directory.CreateDirectory("Assets/GeneratedMaps");
        PrefabUtility.SaveAsPrefabAsset(mapRoot, prefabOutputPath);
        Object.DestroyImmediate(mapRoot);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"맵 프리팹 생성 완료: {prefabOutputPath}");
    }

    private static Tilemap CreateTilemap(GameObject parent, string name)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent.transform);
        Tilemap tilemap = go.AddComponent<Tilemap>();
        go.AddComponent<TilemapRenderer>();
        return tilemap;
    }

    private static List<Tile> LoadTilesFromFolder(string folderPath)
    {
        var tileList = new List<Tile>();
        string[] guids = AssetDatabase.FindAssets("t:Tile", new[] { folderPath });
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Tile tile = AssetDatabase.LoadAssetAtPath<Tile>(path);
            if (tile != null)
                tileList.Add(tile);
        }
        return tileList;
    }
}