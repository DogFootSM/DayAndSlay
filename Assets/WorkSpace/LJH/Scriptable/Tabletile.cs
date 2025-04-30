using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "CustomTiles/TableTile")]
public class Tabletile : Tile
{
    public bool isTable = true;

    // 선택: 타일 배치 시 자동으로 동작할 로직도 가능
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
        // 필요시 커스텀 로직
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        // 타일 렌더링 관련 처리도 가능
    }
}
