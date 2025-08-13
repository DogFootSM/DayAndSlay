using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    private bool isBossRoom;
    [SerializeField] private List<GameObject> doors;

    public Tilemap obstacleTilemap;
    public Tilemap mapTilemap;

    public List<GameObject> GetDoorList()
    {
        return new(doors);
    }

    public bool GetBossCheck() => isBossRoom;
    public void SetBossRoom(bool isBossRoom) => this.isBossRoom = isBossRoom;


}
