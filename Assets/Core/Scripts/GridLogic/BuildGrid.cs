using InterOrbital.Player;
using InterOrbital.WorldSystem;
using InterOrbital.Item;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildGrid : MonoBehaviour
{
    public int width;
    public int height;
    public Tilemap buildLayer;
    public Tilemap highlightBuildLayer;

    public ItemScriptableObject itemToBuild;

    private int buildRange = 10;

    private Vector3Int playerPos;
    private Vector3Int highlightedTilePos;
    private bool buildMode;

    private void Update()
    {
        TestBuild();
        if (buildMode)
        {
            playerPos = buildLayer.WorldToCell(PlayerComponents.Instance.GetPlayerPosition());

            if (itemToBuild != null)
            {
                HighlightTile();
            }

            if (Input.GetKeyDown(KeyCode.R))
                Build(highlightedTilePos);
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

    private void HighlightTile()
    {
        Vector3Int mouseGridPos = GetMouseOnGridPos();

        if(highlightedTilePos != mouseGridPos)
        {
            highlightBuildLayer.SetTile(highlightedTilePos, null);

            if (InRangeToBuild(playerPos, mouseGridPos, buildRange))
            {
                Tile newTile = ScriptableObject.CreateInstance<Tile>();
                newTile.sprite = itemToBuild.itemSprite;

                if (CanBuild(mouseGridPos.x,mouseGridPos.y))
                {
                    newTile.color = Color.green;
                }
                else
                {
                    newTile.color = Color.red;
                }


                highlightBuildLayer.SetTile(mouseGridPos, newTile);
                highlightedTilePos = mouseGridPos;
            }   
        }
    }

    private bool CanBuild(int x, int y)
    {
        return !GridLogic.Instance.IsCellLocked(x, y) && GridLogic.Instance.IsCellSpaceshipArea(x, y);
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

    public void Build(Vector3 worldCoords)
    {
        Vector3Int coords = buildLayer.WorldToCell(worldCoords);
        if (itemToBuild != null && CanBuild(coords.x,coords.y))
        {
            Tile newTile = ScriptableObject.CreateInstance<Tile>();//TODO INSTANCIAR GAMEOBJECTS
            newTile.sprite = itemToBuild.itemSprite;
            buildLayer.SetTile(coords, newTile);

            GridLogic.Instance.LockCell(coords.x, coords.y);
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
