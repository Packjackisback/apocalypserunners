using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    public GameObject chunkPrefab;
    public Transform chunksParent;
    public TileBase groundTile;
    public int chunkSize = 16;
    public int spawnRadius = 1;
    
    private Transform player;
    private Dictionary<Vector2Int, GameObject> activeChunks = new Dictionary<Vector2Int, GameObject>();
    private Vector2Int lastPlayerChunk;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        lastPlayerChunk = GetPlayerChunk();
        UpdateChunks();
    }
    
    void Update()
    {
        Vector2Int currentChunk = GetPlayerChunk();
        if (currentChunk != lastPlayerChunk)
        {
            UpdateChunks();
            lastPlayerChunk = currentChunk;
        }
    }
    
    Vector2Int GetPlayerChunk()
    {
        int x = Mathf.FloorToInt(player.position.x / chunkSize);
        int y = Mathf.FloorToInt(player.position.y / chunkSize);
        return new Vector2Int(x, y);
    }
    
    void UpdateChunks()
    {
        Vector2Int playerChunk = GetPlayerChunk();
        List<Vector2Int> neededChunks = new List<Vector2Int>();
        
        for (int x = -spawnRadius; x <= spawnRadius; x++)
        {
            for (int y = -spawnRadius; y <= spawnRadius; y++)
            {
                Vector2Int coord = new Vector2Int(playerChunk.x + x, playerChunk.y + y);
                neededChunks.Add(coord);
                
                if (!activeChunks.ContainsKey(coord))
                    SpawnChunk(coord);
            }
        }
        
        List<Vector2Int> toRemove = new List<Vector2Int>();
        foreach (var key in activeChunks.Keys)
        {
            if (!neededChunks.Contains(key))
            {
                Destroy(activeChunks[key]);
                toRemove.Add(key);
            }
        }
        
        foreach (var key in toRemove)
            activeChunks.Remove(key);
    }
    
    void SpawnChunk(Vector2Int coord)
    {
        Vector3 worldPos = new Vector3(coord.x * chunkSize, coord.y * chunkSize, 0);
        GameObject chunk = Instantiate(chunkPrefab, worldPos, Quaternion.identity, chunksParent);
        chunk.name = $"Chunk_{coord.x}_{coord.y}";
        
        RoadGenerator rg = chunk.GetComponent<RoadGenerator>();
        
        // Fill ground
        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                rg.groundTilemap.SetTile(new Vector3Int(x, y, 0), groundTile);
            }
        }
        
        rg.GenerateRoads(coord, chunkSize);
        
        activeChunks.Add(coord, chunk);
    }
    
    public void ResetWorld()
    {
        RoadGenerator.ClearCache();
        
        foreach (var chunk in activeChunks.Values)
        {
            Destroy(chunk);
        }
        activeChunks.Clear();
        
        UpdateChunks();
    }
}