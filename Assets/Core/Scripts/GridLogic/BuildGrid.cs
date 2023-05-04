using InterOrbital.WorldSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildGrid : MonoBehaviour
{
    public int width;
    public int height;
    public Tilemap buildLayer;
    private Cell[,] _gridCells;

    private void Awake()
    {
        _gridCells = new Cell[width, height];
    }

    public void Build(Vector3 worldCoords, Tile item)
    {
        Vector3Int coords = buildLayer.WorldToCell(worldCoords);
        if (item != null)
        {
            buildLayer.SetTile(coords, item);
        }
    }

}
