using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : Singleton<MapManager>
{
    [SerializeField] private Tilemap groundTile;
    [SerializeField] private Tilemap probTile;

    public Tilemap GroundTile => groundTile;
    public Tilemap ProbTile => probTile;

    public TileBase GetGroundTile(Vector3Int pos) => groundTile.GetTile(pos);
    public TileBase GetProbTile(Vector3Int pos) => probTile.GetTile(pos);

    public Vector3Int GetTilePos(Vector2 pos) => groundTile.WorldToCell(pos);
    public Vector2 GetWorldPos(Vector3Int pos) => groundTile.CellToWorld(pos);

}
