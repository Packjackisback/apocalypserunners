using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoadGenerator : MonoBehaviour
{
    public Tilemap groundTilemap;
    public Tilemap roadTilemap;
    public TileBase roadTile;
    
    [Range(1, 4)]
    public int minExitsPerEdge = 1;
    [Range(1, 4)]
    public int maxExitsPerEdge = 2;
    
    [Range(0, 100)]
    public int internalRoadChance = 30;
    
    public class ChunkEdges
    {
        public List<int> north = new List<int>();
        public List<int> south = new List<int>();
        public List<int> east = new List<int>();
        public List<int> west = new List<int>();
    }
    
    private System.Random rng;
    private static Dictionary<Vector2Int, ChunkEdges> globalEdgeCache = new Dictionary<Vector2Int, ChunkEdges>();
    private Vector2Int myCoord;
    
    public void GenerateRoads(Vector2Int chunkCoord, int chunkSize)
    {
        myCoord = chunkCoord;
        int seed = chunkCoord.x * 73856093 ^ chunkCoord.y * 19349663;
        rng = new System.Random(seed);
        
        ChunkEdges myEdges = new ChunkEdges();
        
        ChunkEdges northNeighbor = GetOrCreateEdges(chunkCoord + Vector2Int.up, chunkSize);
        ChunkEdges southNeighbor = GetOrCreateEdges(chunkCoord + Vector2Int.down, chunkSize);
        ChunkEdges eastNeighbor = GetOrCreateEdges(chunkCoord + Vector2Int.right, chunkSize);
        ChunkEdges westNeighbor = GetOrCreateEdges(chunkCoord + Vector2Int.left, chunkSize);
        
        myEdges.north = new List<int>(northNeighbor.south);
        myEdges.south = new List<int>(southNeighbor.north);
        myEdges.east = new List<int>(eastNeighbor.west);
        myEdges.west = new List<int>(westNeighbor.east);
        
        GenerateRoadNetwork(myEdges, chunkSize);
        
        globalEdgeCache[chunkCoord] = myEdges;
    }
    
    private ChunkEdges GetOrCreateEdges(Vector2Int coord, int chunkSize)
    {
        if (globalEdgeCache.ContainsKey(coord))
        {
            return globalEdgeCache[coord];
        }
        
        ChunkEdges edges = new ChunkEdges();
        
        int seed = coord.x * 73856093 ^ coord.y * 19349663;
        System.Random localRng = new System.Random(seed);
        
        int numExits = localRng.Next(minExitsPerEdge, maxExitsPerEdge + 1);
        
        HashSet<int> usedPositions = new HashSet<int>();
        
        for (int i = 0; i < numExits; i++)
        {
            edges.north.Add(GetUniquePosition(localRng, chunkSize, usedPositions));
            usedPositions.Clear();
            edges.south.Add(GetUniquePosition(localRng, chunkSize, usedPositions));
            usedPositions.Clear();
            edges.east.Add(GetUniquePosition(localRng, chunkSize, usedPositions));
            usedPositions.Clear();
            edges.west.Add(GetUniquePosition(localRng, chunkSize, usedPositions));
            usedPositions.Clear();
        }
        
        return edges;
    }
    
    private int GetUniquePosition(System.Random rng, int chunkSize, HashSet<int> used)
    {
        int position;
        int attempts = 0;
        do
        {
            position = rng.Next(2, chunkSize - 2);
            attempts++;
        } while (used.Contains(position) && attempts < 20);
        
        used.Add(position);
        return position;
    }
    
    private void GenerateRoadNetwork(ChunkEdges edges, int chunkSize)
    {
        List<Vector2Int> connectionPoints = new List<Vector2Int>();
        
        foreach (int x in edges.north) connectionPoints.Add(new Vector2Int(x, chunkSize - 1));
        foreach (int x in edges.south) connectionPoints.Add(new Vector2Int(x, 0));
        foreach (int y in edges.east) connectionPoints.Add(new Vector2Int(chunkSize - 1, y));
        foreach (int y in edges.west) connectionPoints.Add(new Vector2Int(0, y));
        
        if (connectionPoints.Count == 0) return;
        
        Vector2Int hub = new Vector2Int(chunkSize / 2, chunkSize / 2);
        
        foreach (Vector2Int point in connectionPoints)
        {
            DrawPath(point, hub, chunkSize);
        }
        
        // Optionally add random internal roads for variety
        if (rng.Next(100) < internalRoadChance)
        {
            int numInternalRoads = rng.Next(1, 3);
            for (int i = 0; i < numInternalRoads; i++)
            {
                Vector2Int randomStart = new Vector2Int(rng.Next(4, chunkSize - 4), rng.Next(4, chunkSize - 4));
                Vector2Int randomEnd = new Vector2Int(rng.Next(4, chunkSize - 4), rng.Next(4, chunkSize - 4));
                DrawPath(randomStart, randomEnd, chunkSize);
            }
        }
    }
    
    private void DrawPath(Vector2Int start, Vector2Int end, int chunkSize)
    {
        List<Vector2Int> path = ManhattanPath(start, end, chunkSize);
        
        foreach (Vector2Int point in path)
        {
            // Use local tile coordinates (0 to chunkSize-1)
            if (point.x >= 0 && point.x < chunkSize && point.y >= 0 && point.y < chunkSize)
            {
                roadTilemap.SetTile(new Vector3Int(point.x, point.y, 0), roadTile);
            }
        }
    }
    
    private List<Vector2Int> ManhattanPath(Vector2Int start, Vector2Int end, int chunkSize)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int current = start;
        
        int maxSteps = chunkSize * 4; // Safety limit
        int steps = 0;
        
        while (current != end && steps < maxSteps)
        {
            path.Add(current);
            
            int dx = end.x - current.x;
            int dy = end.y - current.y;
            
            bool preferX = Mathf.Abs(dx) > Mathf.Abs(dy);
            
            if (rng.Next(100) < 20)
            {
                preferX = !preferX;
            }
            
            if (preferX && dx != 0)
            {
                current.x += dx > 0 ? 1 : -1;
            }
            else if (dy != 0)
            {
                current.y += dy > 0 ? 1 : -1;
            }
            else if (dx != 0)
            {
                current.x += dx > 0 ? 1 : -1;
            }
            
            steps++;
        }
        
        path.Add(end);
        return path;
    }
    
    public static void ClearCache()
    {
        globalEdgeCache.Clear();
    }
}