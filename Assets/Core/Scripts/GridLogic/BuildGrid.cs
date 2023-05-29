using InterOrbital.Player;
using InterOrbital.WorldSystem;
using InterOrbital.Item;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildGrid : MonoBehaviour
{
    public Tilemap buildLayer;
    public Tilemap highlightBuildLayer;

    public static BuildGrid Instance;


    private int buildRange = 10;
    private ItemScriptableObject itemToBuild;
    private Vector3Int playerPos;
    private Vector3Int highlightedTilePos;
    private bool buildMode;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
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
                if (IsMoreThanTileBigger(itemToBuild.itemSprite))
                    newTile.sprite = ChangeSpritePivot(itemToBuild.itemSprite);
                else 
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
        bool canBuild = true;
        if (IsMoreThanTileBigger(itemToBuild.itemSprite))
        {
            int tileWidth = (int)itemToBuild.itemSprite.bounds.size.x;
            int tileHeight = (int)itemToBuild.itemSprite.bounds.size.y;

            for (int j = 0; j < tileHeight; j++)
            {
                for (int i = 0; i < tileWidth; i++)
                {
                    if(GridLogic.Instance.IsCellLocked(x + i, y + j) || !GridLogic.Instance.IsCellSpaceshipArea(x + i, y + j))
                    {
                        canBuild = false;
                    }
                }
            }
        }
        else
        {
            if(GridLogic.Instance.IsCellLocked(x, y) || !GridLogic.Instance.IsCellSpaceshipArea(x, y))
                canBuild = false;
        }

        return canBuild;
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

    private bool IsMoreThanTileBigger(Sprite sprite)
    {
        if(sprite.bounds.size.x > 1 || sprite.bounds.size.y > 1)
            return true; 
        else
            return false;
    }

    private Sprite ChangeSpritePivot(Sprite sprite)
    {
        int spriteWidth = (int)sprite.bounds.size.x;
        int spriteHeight = (int)sprite.bounds.size.y;
        // Tamaño de cada tile individual
        int tileWidth = 16;
        int tileHeight = 16;

        Vector2 pivot = new Vector2(0.5f * (tileWidth /(float) (tileWidth * spriteWidth)), 0.5f * (tileHeight / (float)(tileHeight * spriteHeight)));

        // Calcular las coordenadas de recorte en la textura
        Rect rect = new Rect(0, 0, tileWidth* spriteWidth, tileHeight* spriteHeight);

        // Crear un nuevo sprite individual usando las coordenadas de recorte
        return Sprite.Create(sprite.texture,rect, pivot,16);
    }

    private Vector3 BuildPosition(Vector3Int coords)
    {
        if (IsMoreThanTileBigger(itemToBuild.itemSprite))
        {
            float offsetX = itemToBuild.itemSprite.bounds.size.x / 2;
            float offsetY = itemToBuild.itemSprite.bounds.size.y / 2;

            return new Vector3(coords.x + offsetX + 0.5f, coords.y + offsetY + 0.5f, 0);
        }
        else
        {
            return new Vector3(coords.x + 0.5f, coords.y + 0.5f, 0);
        }
    }

    public void Build(Vector3 worldCoords)
    {
        Vector3Int coords = buildLayer.WorldToCell(worldCoords);
        Vector3 buildPos = BuildPosition(coords);
        if (itemToBuild != null && itemToBuild.buildPrefab != null && CanBuild(coords.x,coords.y))
        {
            Instantiate(itemToBuild.buildPrefab, buildPos, Quaternion.identity);

            GridLogic.Instance.LockCell(coords.x, coords.y);
            PlayerComponents.Instance.Inventory.SubstractUsedItem();
            if(!PlayerComponents.Instance.Inventory.CanUseMore())
                DesactivateBuildMode();
        }
    }

    public void ActivateBuildMode(ItemScriptableObject item)
    {
        itemToBuild = item;
        buildMode = true;
    }

    public void DesactivateBuildMode()
    {
        itemToBuild = null;
        buildMode = false;
        highlightBuildLayer.SetTile(highlightedTilePos, null);
    }

    public bool IsBuilding()
    {
        return buildMode;
    }
}
