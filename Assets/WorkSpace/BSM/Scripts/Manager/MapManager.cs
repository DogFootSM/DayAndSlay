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
    [SerializedDictionary("MapType","MapSize")] [SerializeField]
    private SerializedDictionary<MapType, List<Vector2>> mapLimitSizeDict = new SerializedDictionary<MapType, List<Vector2>>();
    
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


}
