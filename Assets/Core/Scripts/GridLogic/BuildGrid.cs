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


    private int _buildRange = 10;
    private ItemScriptableObject _itemToBuild;
    private Vector3Int _playerPos;
    private Vector3Int _highlightedTilePos;
    private bool _buildMode;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if (_buildMode)
        {
            _playerPos = buildLayer.WorldToCell(PlayerComponents.Instance.GetPlayerPosition());

            if (_itemToBuild != null)
            {
                HighlightTile();
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

        if(_highlightedTilePos != mouseGridPos)
        {
            highlightBuildLayer.SetTile(_highlightedTilePos, null);

            if (InRangeToBuild(_playerPos, mouseGridPos, _buildRange))
            {
                Tile newTile = ScriptableObject.CreateInstance<Tile>();
                if (IsMoreThanTileBigger(_itemToBuild.buildHighlightSprite))
                    newTile.sprite = ChangeSpritePivot(_itemToBuild.buildHighlightSprite);
                else 
                    newTile.sprite = _itemToBuild.buildHighlightSprite; 

                if (CanBuild(mouseGridPos.x,mouseGridPos.y))
                {
                    newTile.color = Color.green;
                }
                else
                {
                    newTile.color = Color.red;
                }


                highlightBuildLayer.SetTile(mouseGridPos, newTile);
                _highlightedTilePos = mouseGridPos;
            }   
        }
    }

    private bool CanBuild(int x, int y)
    {
        bool canBuild = true;
        if (IsMoreThanTileBigger(_itemToBuild.buildHighlightSprite))
        {
            int tileWidth = (int)_itemToBuild.buildHighlightSprite.bounds.size.x;
            int tileHeight = (int)_itemToBuild.buildHighlightSprite.bounds.size.y;

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

    private void LockCellsOnBuild(Vector3Int coords)
    {
        if (IsMoreThanTileBigger(_itemToBuild.buildHighlightSprite))
        {
            int tileWidth = (int)_itemToBuild.buildHighlightSprite.bounds.size.x;
            int tileHeight = (int)_itemToBuild.buildHighlightSprite.bounds.size.y;

            for (int j = 0; j < tileHeight; j++)
            {
                for (int i = 0; i < tileWidth; i++)
                {
                    GridLogic.Instance.LockCell(coords.x + i, coords.y + j);
                }
            }
        }
        else
        {
            GridLogic.Instance.LockCell(coords.x, coords.y);
        }
    }

    private bool InRangeToBuild(Vector3Int posA, Vector3Int posB, int range)
    {
        Vector3Int distance = posA - posB;

        if (Mathf.Abs(distance.x) > range || Mathf.Abs(distance.y) > range)
        {
            _highlightedTilePos = default;
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
        if (IsMoreThanTileBigger(_itemToBuild.buildHighlightSprite))
        {
            float offsetX = _itemToBuild.buildHighlightSprite.bounds.size.x / 2;
            float offsetY = _itemToBuild.buildHighlightSprite.bounds.size.y / 2;

            return new Vector3(coords.x + offsetX, coords.y + offsetY, 0);
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
        if (_itemToBuild != null && _itemToBuild.buildPrefab != null && CanBuild(coords.x,coords.y))
        {
            Instantiate(_itemToBuild.buildPrefab, buildPos, Quaternion.identity);

            LockCellsOnBuild(coords);

            PlayerComponents.Instance.Inventory.SubstractUsedItem();
            if(!PlayerComponents.Instance.Inventory.CanUseMore())
                DesactivateBuildMode();
        }
    }

    public void ActivateBuildMode(ItemScriptableObject item)
    {
        _itemToBuild = item;
        _buildMode = true;
    }

    public void DesactivateBuildMode()
    {
        _itemToBuild = null;
        _buildMode = false;
        highlightBuildLayer.SetTile(_highlightedTilePos, null);
    }

    public bool IsBuilding()
    {
        return _buildMode;
    }

    public Vector3Int GetPosToBuild()
    {
        return _highlightedTilePos;
    }
}
