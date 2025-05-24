using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializedDictionary("MapType","TileMap")] [SerializeField]
    private SerializedDictionary<MapType, Tilemap> tileMapDict = new SerializedDictionary<MapType, Tilemap>();

    private MapType curMapType;
    
    public Tilemap GetTilemap()
    { 
        return tileMapDict.GetValueOrDefault(curMapType); 
    }

    public void ManChange(MapType mapType)
    {
        curMapType = mapType;
    }
    
}
