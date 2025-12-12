using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Zenject;

public class MapManager : MonoBehaviour
{
    [SerializedDictionary("MapType", "MapSize")] [SerializeField]
    private SerializedDictionary<MapType, List<Vector2>> mapLimitSizeDict = new SerializedDictionary<MapType, List<Vector2>>();

    [SerializedDictionary("MapType", "MapSize")] [SerializeField]
    private SerializedDictionary<MapType, Tilemap> tileMapsDict = new SerializedDictionary<MapType, Tilemap>();

    public FollowCamera FollowCamera;

    private MapType curMapType;


    public List<Vector2> GetMapBoundary()
    {
        return mapLimitSizeDict.GetValueOrDefault(curMapType);
    }

    /// <summary>
    /// 씬 이동 시 타입 변경
    /// </summary>
    /// <param name="mapType"></param>
    public void MapChange(MapType mapType)
    {
        curMapType = mapType;
        FollowCamera?.OnMapChanged?.Invoke();
    }

    public void TileMapDictInit(List<Tilemap> tileMaps)
    {
        tileMapsDict[MapType.DUNGEON_0] = tileMaps[0];
        tileMapsDict[MapType.DUNGEON_1] = tileMaps[1];
        tileMapsDict[MapType.DUNGEON_2] = tileMaps[2];
        tileMapsDict[MapType.DUNGEON_3] = tileMaps[3];
        tileMapsDict[MapType.DUNGEON_4] = tileMaps[4];
        tileMapsDict[MapType.DUNGEON_BOSS] = tileMaps[5];

    }

    public void MapDictInit()
    {
        for (int i = 0; i < 6; i++)
        {
            Bounds bounds = tileMapsDict[(MapType)i + 3].localBounds;
            Vector3 value = Vector3.zero;
            if ((MapType)i + 3 == MapType.DUNGEON_0)
            {
                value = new Vector3(0, 0, 0);
            }
           
            if ((MapType)i + 3 == MapType.DUNGEON_1)
            {
                value = new Vector3(54, 0, 0);
            }
           
            if ((MapType)i + 3 == MapType.DUNGEON_2)
            {
                value = new Vector3(108, 0, 0);
            }
           
            if ((MapType)i + 3 == MapType.DUNGEON_3)
            {
                value = new Vector3(0, -60, 0);
            }
           
            if ((MapType)i + 3 == MapType.DUNGEON_4)
            {
                value = new Vector3(54, -60, 0);
            }
           
            if ((MapType)i + 3 == MapType.DUNGEON_BOSS)
            {
                value = new Vector3(108, -60, 0);
            }

            Vector2 bottomLeft = bounds.min + new Vector3(9f, 5f, 0) + value; // 좌하단
            Vector2 topRight = bounds.max + new Vector3(-9f, -5f, 0) + value; // 우상단

            List<Vector2> mapBoundary = new List<Vector2>();
            mapBoundary.Add(bottomLeft);
            mapBoundary.Add(topRight);
            mapLimitSizeDict[(MapType)i + 3] = mapBoundary;
        }

    }
}