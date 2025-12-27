using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Room : MonoBehaviour
{
    private bool isBossRoom;
    [SerializeField] private List<GameObject> doors;

    public Tilemap obstacleTilemap;
    public Tilemap mapTilemap;
    public Tilemap roadTilemap;

    public List<GameObject> GetDoorList()
    {
        return new(doors);
    }

    public bool GetBossCheck() => isBossRoom;
    public void SetBossRoom(bool isBossRoom) => this.isBossRoom = isBossRoom;


}
