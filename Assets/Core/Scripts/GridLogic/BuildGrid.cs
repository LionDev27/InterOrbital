using InterOrbital.Player;
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
    public Tilemap highlightBuildLayer;

    public TileBase TileToBuild;
    public TileBase highlightTile;

    private Cell[,] _gridCells;

    private int buildRange = 5;

    private Vector3Int playerPos;
    private Vector3Int highlightedTilePos;
    private bool buildMode;

    private void Awake()
    {
        _gridCells = new Cell[width, height];
    }

    private void Update()
    {
        
        TestBuild();
        if (buildMode)
        {
            playerPos = buildLayer.WorldToCell(PlayerComponents.Instance.GetPlayerPosition());

            if (highlightTile != null)
            {
                HighlightTile(highlightTile);
            }

            if (Input.GetKeyDown(KeyCode.R))
                Build(highlightedTilePos, TileToBuild);
        }
    }

    private void TestBuild()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if(buildMode)
            {
                DesactivateBuildMode();
            }
            else
            {
                ActivateBuildMode();
            }


        }
    }

    private Vector3Int GetMouseOnGridPos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mouseCellPos = buildLayer.WorldToCell(mousePos);
        mouseCellPos.z = 0;

        return mouseCellPos;
    }

    private void HighlightTile(TileBase t)
    {
        Vector3Int mouseGridPos = GetMouseOnGridPos();

        if(highlightedTilePos != mouseGridPos)
        {
            highlightBuildLayer.SetTile(highlightedTilePos, null);

            if (InRangeToBuild(playerPos, mouseGridPos, buildRange))
            {

                highlightBuildLayer.SetTile(mouseGridPos, t);
                highlightedTilePos = mouseGridPos;
            }   
        }
    }

    private bool InRangeToBuild(Vector3Int posA, Vector3Int posB, int range)
    {
        Vector3Int distance = posA - posB;

        if (Mathf.Abs(distance.x) > range || Mathf.Abs(distance.y) > range)
        {
            highlightedTilePos = default;
            return false;
        }

        return true;
    }

    public void Build(Vector3 worldCoords, TileBase item)
    {
        Vector3Int coords = buildLayer.WorldToCell(worldCoords);
        if (item != null)
        {
            buildLayer.SetTile(coords, item);
        }
    }

    public void ActivateBuildMode()
    {
        buildMode = true;
    }

    public void DesactivateBuildMode()
    {
        buildMode = false;
        highlightBuildLayer.SetTile(highlightedTilePos, null);
    }

}
