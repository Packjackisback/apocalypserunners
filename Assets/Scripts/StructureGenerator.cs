using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class StructureGenerator : MonoBehaviour
{
    private Tilemap roadTilemap;
    public Tilemap structureTilemap;

    [Header("Structure List")]
    public List<Tilemap> buildings;

    // Update is called once per frame
    void Start()
    {
        RoadGenerator roadGenerator = GetComponent<RoadGenerator>();
        roadTilemap = roadGenerator.roadTilemap;
    }

    public void placeBuildings()
    {
        return;
    }
}
