using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadGenerator : MonoBehaviour
{
    public Tilemap roadTilemap;

    [Header("Tile Assets")]
    public RuleTile roadRuleTile;

    [Header("Settings")]
    public int roadWidth = 5;
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
        int hw = roadWidth / 2;

        if (myRoads.north || myRoads.south || myRoads.east || myRoads.west)
            DrawRoadSegment(mid - hw, mid - hw, roadWidth, roadWidth);

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
                roadTilemap.SetTile(new Vector3Int(x, y, 0), roadRuleTile);
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

    private bool GetEdgeHasRoad(Vector2Int chunkCoord, Vector2Int direction)
    {
        int seed = (chunkCoord.x * 73856093) ^ (chunkCoord.y * 19349663) ^ (direction.x * 83492791) ^ (direction.y * 33492857);
        System.Random edgeRng = new System.Random(seed);
        return edgeRng.Next(0, 100) < roadChance;
    }
}