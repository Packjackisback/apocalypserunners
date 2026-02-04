using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadGenerator : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap roadTilemap;

    [Header("Tile Assets")]
    public TileBase centerTile;
    public TileBase manholeTile;
    public TileBase edgeTop;
    public TileBase edgeBottom;
    public TileBase edgeLeft;
    public TileBase edgeRight;
    public TileBase cornerTopLeft;
    public TileBase cornerTopRight;
    public TileBase cornerBottomLeft;
    public TileBase cornerBottomRight;

    [Header("Settings")]
    public int roadWidth = 8;
    [Range(0, 100)]
    public int roadChance = 60;

    private struct ChunkRoads
    {
        public bool north, south, east, west;
    }

    public void GenerateRoads(Vector2Int chunkCoord, int chunkSize)
    {
        roadTilemap.ClearAllTiles();
        ChunkRoads myRoads = GetRoadConfig(chunkCoord);

        int mid = chunkSize / 2;
        int hw = roadWidth / 2; // Half-width

        if (myRoads.north || myRoads.south || myRoads.east || myRoads.west)
        {
            DrawRoadSegment(mid - hw, mid - hw, roadWidth, roadWidth);
        }

        if (myRoads.north) DrawRoadSegment(mid - hw, mid, roadWidth, chunkSize - mid);
        if (myRoads.south) DrawRoadSegment(mid - hw, 0, roadWidth, mid);
        if (myRoads.east) DrawRoadSegment(mid, mid - hw, chunkSize - mid, roadWidth);
        if (myRoads.west) DrawRoadSegment(0, mid - hw, mid, roadWidth);
    }

    private void DrawRoadSegment(int startX, int startY, int width, int height)
    {
        for (int x = startX; x < startX + width; x++)
        {
            for (int y = startY; y < startY + height; y++)
            {
                bool isLeft = (x == startX);
                bool isRight = (x == startX + width - 1);
                bool isBottom = (y == startY);
                bool isTop = (y == startY + height - 1);

                TileBase selectedTile = centerTile;



                if (isLeft && isTop) selectedTile = cornerTopLeft;
                else if (isRight && isTop) selectedTile = cornerTopRight;
                else if (isLeft && isBottom) selectedTile = cornerBottomLeft;
                else if (isRight && isBottom) selectedTile = cornerBottomRight;
                else if (isTop) selectedTile = edgeTop;
                else if (isBottom) selectedTile = edgeBottom;
                else if (isLeft) selectedTile = edgeLeft;
                else if (isRight) selectedTile = edgeRight;
                else if (GetRNG().Next(0, 100) < holeChance) ;

                Vector3Int pos = new Vector3Int(x, y, 0);
                if (!roadTilemap.HasTile(pos) || selectedTile != centerTile)
                {
                    roadTilemap.SetTile(pos, selectedTile);
                }
            }
        }
    }

    private ChunkRoads GetRoadConfig(Vector2Int coord)
    {
        return new ChunkRoads
        {
            north = GetEdgeHasRoad(coord, Vector2Int.up),
            south = GetEdgeHasRoad(coord + Vector2Int.down, Vector2Int.up),
            east = GetEdgeHasRoad(coord, Vector2Int.right),
            west = GetEdgeHasRoad(coord + Vector2Int.left, Vector2Int.right)
        };
    }

    private System.Random GetRNG(Vector2Int chunkCoord, Vector2Int direction)
    {
        int seed = (chunkCoord.x * 73856093) ^ (chunkCoord.y * 19349663) ^ (direction.x * 83492791) ^ (direction.y * 33492857);
        return new System.Random(seed);
    }

    private bool GetEdgeHasRoad(Vector2Int chunkCoord, Vector2Int direction)
    {
        int seed = (chunkCoord.x * 73856093) ^ (chunkCoord.y * 19349663) ^ (direction.x * 83492791) ^ (direction.y * 33492857);
        System.Random edgeRng = new System.Random(seed);
        return edgeRng.Next(0, 100) < roadChance;
    }

    public static void ClearCache() { }
}