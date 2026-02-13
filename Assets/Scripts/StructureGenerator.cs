using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class StructureGenerator : MonoBehaviour
{
    private Tilemap roadTilemap;
    public Tilemap structureTilemap;

    [Header("Structure List")]
    public List<Tilemap> buildings;

    public void PlaceBuildings(Vector2Int chunkCoord, int chunkSize)
    {
        RoadGenerator roadGenerator = GetComponent<RoadGenerator>();
        roadTilemap = roadGenerator.roadTilemap;
        structureTilemap.ClearAllTiles();

        int chunkSeed = (chunkCoord.x * 3125) ^ (chunkCoord.y * 9187);
        System.Random rng = new System.Random(chunkSeed);
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                Vector3Int currentPos = new Vector3Int(x, y, 0);

                if (IsPositionEmpty(currentPos) && IsNextToRoad(currentPos))
                {
                    TryFitRandomBuilding(currentPos, rng);
                }
            }
        }
    }

    private void TryFitRandomBuilding(Vector3Int origin, System.Random rng)
    {
        List<Tilemap> shuffledBuildings = new List<Tilemap>(buildings);
        for (int i = shuffledBuildings.Count - 1; i > 0; i--)
        {
            int k = rng.Next(i + 1);
            var value = shuffledBuildings[k];
            shuffledBuildings[k] = shuffledBuildings[i];
            shuffledBuildings[i] = value;
        }

        foreach (Tilemap template in shuffledBuildings)
        {
            if (CanBuildingFit(template, origin))
            {
                StampBuilding(template, origin);
                break;
            }
        }
    }

    private bool CanBuildingFit(Tilemap template, Vector3Int origin)
    {
        BoundsInt bounds = template.cellBounds;

        foreach (var pos in bounds.allPositionsWithin)
        {
            if (template.HasTile(pos))
            {
                Vector3Int targetPos = origin + (pos - bounds.min);

                if (!IsPositionEmpty(targetPos)) return false;
            }
        }
        return true;
    }

    private void StampBuilding(Tilemap template, Vector3Int origin)
    {
        BoundsInt bounds = template.cellBounds;
        foreach (var pos in bounds.allPositionsWithin)
        {
            TileBase tile = template.GetTile(pos);
            if (tile != null)
            {
                Vector3Int targetPos = origin + (pos - bounds.min);
                structureTilemap.SetTile(targetPos, tile);
            }
        }
    }

    private bool IsPositionEmpty(Vector3Int pos)
    {
        return roadTilemap.GetTile(pos) == null && structureTilemap.GetTile(pos) == null;
    }

    private bool IsNextToRoad(Vector3Int pos)
    {
        Vector3Int[] neighbors = { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        foreach (var offset in neighbors)
        {
            if (roadTilemap.HasTile(pos + offset)) return true;
        }
        return false;
    }
}