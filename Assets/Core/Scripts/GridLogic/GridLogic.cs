using InterOrbital.Player;
using InterOrbital.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

using UnityEditor;

namespace InterOrbital.WorldSystem
{
    public class GridLogic : MonoBehaviour
    {
        public int width;
        public int height;
        public Vector3 cellSize = new Vector3(1, 1, 0);
        public List<string> biomes;
        public TilemapLayer[] tilemapLayers;

        public static GridLogic Instance;

        private const string TEXTURES_PATH = "Textures/";

        private Cell[,] _gridCells;
        private Grid _grid;

        #region Unity Methods

        protected virtual void Awake()
        {
            if (Instance == null)
                Instance = this;

            _grid = GetComponent<Grid>();
            _grid.cellSize = cellSize;
        }

        private void OnValidate()
        {
            for (int i = 0; i < biomes.Count; i++)
            {
                // Verificar si el bioma ya existe en la lista de biomas con tiles de reglas
                foreach (var tilemapLayer in tilemapLayers)
                {
                    bool existe = false;
                    for (int j = 0; j < tilemapLayer.biomesTiles.Count; j++)
                    {
                        if (tilemapLayer.biomesTiles[j].biome == biomes[i])
                        {
                            existe = true;
                            break;
                        }
                    }

                    // Si el bioma no existe, crear un nuevo struct de bioma con tile de regla y agregarlo a la lista
                    if (!existe)
                    {
                        BiomeRuleTile newBiome = new BiomeRuleTile();
                        newBiome.biome = biomes[i];
                        tilemapLayer.biomesTiles.Add(newBiome);
                    }
                }
            }


            foreach (var tilemapLayer in tilemapLayers)
            {
                for (int i = tilemapLayer.biomesTiles.Count - 1; i >= 0; i--) // Recorrer la lista de biomas con tiles de reglas
                {
                    if (!biomes.Contains(tilemapLayer.biomesTiles[i].biome)) // Verificar si el bioma ya no está en la lista de biomas
                    {
                        tilemapLayer.biomesTiles.RemoveAt(i); // Eliminar el elemento de biomasConTilesDeReglas
                    }
                }
            }

        }

        private void Start()
        {
            InitializeGrid();
            CreateRegions();

            foreach (var tilemapLayer in tilemapLayers)
            {
                FillTilemap(tilemapLayer.tilemap,tilemapLayer.minimapTilemap, tilemapLayer.biomesTiles, tilemapLayer.fillMode);
            }

            for (int i = 5; i < 15 ; i++)
            {
                for (int j = 5; j < 15; j++)
                    _gridCells[i, j].MakeSpaceshipArea();
            }
        }

        #endregion

        #region Private Methods

        private void FillTilemap(Tilemap tilemap, Tilemap minimapTilemap, List<BiomeRuleTile> tiles, FillMode fillMode)
        {
            switch (fillMode)
            {
                case FillMode.None:
                    break;
                case FillMode.Single_All:
                    FillTilemapSingleAll(tilemap, tiles);
                    FillTilemapBorders(tilemap);
                    GenerateMinimapTilemap(tilemap, minimapTilemap);
                    break;
                case FillMode.Multiple_All:
                    break;
                case FillMode.Single_Random:
                    FillTilemapSingleRandom(tilemap, tiles);
                    FillTilemapBorders(tilemap);
                    GenerateMinimapTilemap(tilemap, minimapTilemap);
                    AddAnimatedTiles(tilemap, tiles);
                    break;
                case FillMode.Multiple_Random:
                    break;
            }
        }

        private void FillTilemapSingleAll(Tilemap tilemap, List<BiomeRuleTile> tiles)
        {
            for (int x = 0; x < Mathf.RoundToInt(_grid.cellSize.x) * width; x++)
            {
                for (int y = 0; y < Mathf.RoundToInt(_grid.cellSize.y) * height; y++)
                {
                    // Obtener la posición de la celda en el grid
                    Vector3Int position = new Vector3Int(x, y, 0);

                    int biomeIndex = tiles.FindIndex(tiles => tiles.biome == _gridCells[x, y].biomeType);

                    // Asignar el sprite al Tilemap en la posición correspondiente
                    tilemap.SetTile(position, tiles[biomeIndex].tiles);
                }
            }
        }

        private void FillTilemapBorders(Tilemap tilemap)
        {

            Sprite[,] tilemapSprites = GetTilemapSprites(tilemap);

            for (int x = 0; x < Mathf.RoundToInt(_grid.cellSize.x) * width; x++)
            {
                for (int y = 0; y < Mathf.RoundToInt(_grid.cellSize.y) * height; y++)
                {
                    // Obtener la posición de la celda en el grid
                    Vector3Int position = new Vector3Int(x, y, 0);
                    Vector3Int rightSideMapPos = new Vector3Int(x + width, y, 0);
                    Vector3Int leftSideMapPos = new Vector3Int(x - width, y, 0);
                    Vector3Int topSideMapPos = new Vector3Int(x, y + height, 0);
                    Vector3Int botSideMapPos = new Vector3Int(x, y - height, 0);
                    Vector3Int topLeftSideMapPos = new Vector3Int(x - width, y + height, 0);
                    Vector3Int topRightSideMapPos = new Vector3Int(x + width, y + height, 0);
                    Vector3Int botLeftSideMapPos = new Vector3Int(x - width, y - height, 0);
                    Vector3Int botRightSideMapPos = new Vector3Int(x + width, y - height, 0);
                    

                   
                    Tile tile = ScriptableObject.CreateInstance<Tile>();

                        // Asigna el Sprite al Tile
                    tile.sprite = tilemap.GetSprite(position);                     

                    // Asignar el sprite al Tilemap en la posición correspondiente
                    tilemap.SetTile(rightSideMapPos, tile);
                    tilemap.SetTile(leftSideMapPos, tile);
                    tilemap.SetTile(topSideMapPos, tile);
                    tilemap.SetTile(botSideMapPos, tile);
                    tilemap.SetTile(botLeftSideMapPos, tile);
                    tilemap.SetTile(botRightSideMapPos, tile);
                    tilemap.SetTile(topLeftSideMapPos, tile);
                    tilemap.SetTile(topRightSideMapPos, tile);
                }
            }
        }

        private void FillTilemapSingleRandom(Tilemap tilemap, List<BiomeRuleTile> tiles)
        {
            int detailZones = width + height / 2;

            for (int i = 0; i < detailZones; i++)
            {
                int x = UnityEngine.Random.Range(0, width);
                int y = UnityEngine.Random.Range(0, height);
                int minExt = UnityEngine.Random.Range(1, detailZones / 2);
                int maxExt = UnityEngine.Random.Range(detailZones / 2, detailZones);

                RandomDetail(x, y, minExt, maxExt);
            }

            for (int x = 0; x < Mathf.RoundToInt(_grid.cellSize.x) * width; x++)
            {
                for (int y = 0; y < Mathf.RoundToInt(_grid.cellSize.y) * height; y++)
                {
                    if (!_gridCells[x, y].HaveDetail())
                    {
                        continue;
                    }
                    // Obtener la posición de la celda en el grid
                    Vector3Int position = new Vector3Int(x, y, 0);


                    int biomeIndex = tiles.FindIndex(tiles => tiles.biome == _gridCells[x, y].biomeType);

                    tilemap.SetTile(position, tiles[biomeIndex].tiles);
                }
            }
        }

        private Sprite[,] GetTilemapSprites(Tilemap tilemap)
        {
            Sprite[,] tilemapSprites = new Sprite[width,height];

            for (int x = 0; x < Mathf.RoundToInt(_grid.cellSize.x) * width; x++)
            {
                for (int y = 0; y < Mathf.RoundToInt(_grid.cellSize.y) * height; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);

                    tilemapSprites[x,y] = tilemap.GetSprite(position);
                }
            }

            return tilemapSprites;

        }

        private List<Sprite> GenerateSpriteList(Texture2D texture)
        {
            List<Sprite> spriteList = new List<Sprite>();

            string texturePath = TEXTURES_PATH + texture.name;
            // Obtiene todos los sprites generados por el Sprite Editor
            Sprite[] sprites = Resources.LoadAll<Sprite>(texturePath); // Asigna el nombre de la textura como nombre de la carpeta de recursos

            // Agrega los sprites a la lista
            spriteList.AddRange(sprites);

            return spriteList;
        }


        private void AddAnimatedTiles(Tilemap tilemap, List<BiomeRuleTile> tiles)
        {
            Sprite[,] tilemapSprites = GetTilemapSprites(tilemap);
            List<Sprite> spriteList = new List<Sprite>();
            int lastBiomeIndex = -1;

            for (int x = -Mathf.RoundToInt(_grid.cellSize.x) * width; x < Mathf.RoundToInt(_grid.cellSize.x) * width*2; x++)
            {
                for (int y = -Mathf.RoundToInt(_grid.cellSize.y) * height; y < Mathf.RoundToInt(_grid.cellSize.y) * height*2; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);

                    int biomeIndex = tiles.FindIndex(tiles => tiles.biome == _gridCells[x, y].biomeType);
                    if(biomeIndex != lastBiomeIndex) 
                    {
                        spriteList = GenerateSpriteList(tiles[biomeIndex].animationTiles.textureToChangeRuleTile);
                        lastBiomeIndex = biomeIndex;
                    }


                    if (tiles[biomeIndex].animationTiles != null)
                    {
                        SpriteAnimatedTile encounteredSprite = tiles[biomeIndex].animationTiles.spriteToAnimatedTiles.Find(spriteAnimado => spriteAnimado.sprite == tilemapSprites[x, y]);

                        if (encounteredSprite.sprite != null)
                        {
                            tilemap.SetTile(position, encounteredSprite.animatedTile);
                        }
                    }



                    Sprite s = spriteList.Find(s => s == tilemapSprites[x, y]);
                    if (s != null)
                    {
                        Tile tile = ScriptableObject.CreateInstance<Tile>();

                        // Asigna el Sprite al Tile
                        tile.sprite = s;
                        tilemap.SetTile(position, tile);
                    }

                }
            }
        }

        private void GenerateMinimapTilemap(Tilemap origin, Tilemap minimap)
        {
            foreach (var pos in origin.cellBounds.allPositionsWithin)
            {
                var sourceTile = origin.GetTile(pos);
                if (sourceTile != null)
                {
                    minimap.SetTile(pos, sourceTile);
                }
            }
        }

        private void InitializeGrid()
        {
            _gridCells = new Cell[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _gridCells[i, j] = new Cell(i, j, biomes[0]);
                }
            }
        }

        private void CreateRegions()
        {

            //RandomBiomeCreation(10, 40, biomes[1], 400, 450);
        }

        private void RandomBiomeCreation(int x, int y, string biome, int minExtension, int maxExtension)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                HashSet<Vector2Int> visited = new HashSet<Vector2Int>(); // conjunto de casillas visitadas
                int extension = UnityEngine.Random.Range(minExtension, maxExtension + 1); // determinar la extension aleatoria
                for (int i = 0; i < extension; i++)
                {
                    Vector2Int currentPos = new Vector2Int(x, y); // posición actual

                    while (visited.Contains(currentPos) || (currentPos.x < 0 || currentPos.x >= width || currentPos.y < 0 || currentPos.y >= height))
                    {
                        int direction = UnityEngine.Random.Range(0, 4);
                        switch (direction)
                        {
                            case 0: // arriba
                                currentPos.y++;
                                if (currentPos.y >= height)
                                    currentPos.y = 0;
                                break;
                            case 1: // abajo
                                currentPos.y--;
                                if (currentPos.y < 0)
                                    currentPos.y = height - 1;
                                break;
                            case 2: // izquierda
                                currentPos.x--;
                                if (currentPos.x >= width)
                                    currentPos.x = 0;
                                break;
                            case 3: // derecha
                                currentPos.x++;
                                if (currentPos.x < 0)
                                    currentPos.x = width - 1;
                                break;
                        }
                    }
                    _gridCells[currentPos.x, currentPos.y].biomeType = biome; // Asignar bioma a la casilla actual

                    visited.Add(currentPos); // agregar la casilla actual al conjunto de casillas visitadas

                }
            }
        }

        private void RandomDetail(int x, int y, int minExtension, int maxExtension)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
            {
                HashSet<Vector2Int> visited = new HashSet<Vector2Int>(); // conjunto de casillas visitadas
                int extension = UnityEngine.Random.Range(minExtension, maxExtension + 1); // determinar la extension aleatoria
                for (int i = 0; i < extension; i++)
                {
                    Vector2Int currentPos = new Vector2Int(x, y); // posición actual

                    while (visited.Contains(currentPos) || (currentPos.x < 0 || currentPos.x >= width || currentPos.y < 0 || currentPos.y >= height))
                    {
                        int direction = UnityEngine.Random.Range(0, 4);
                        switch (direction)
                        {
                            case 0: // arriba
                                currentPos.y++;
                                if (currentPos.y >= height)
                                    currentPos.y = 0;
                                break;
                            case 1: // abajo
                                currentPos.y--;
                                if (currentPos.y < 0)
                                    currentPos.y = height - 1;
                                break;
                            case 2: // izquierda
                                currentPos.x--;
                                if (currentPos.x >= width)
                                    currentPos.x = 0;
                                break;
                            case 3: // derecha
                                currentPos.x++;
                                if (currentPos.x < 0)
                                    currentPos.x = width - 1;
                                break;
                        }
                    }
                    _gridCells[currentPos.x, currentPos.y].AddDetail(); // Asignar bioma a la casilla actual

                    visited.Add(currentPos); // agregar la casilla actual al conjunto de casillas visitadas

                }
            }
        }

        #endregion

        #region Public Methods

        public void LockCell(int x, int y)
        {
            if(x >= 0 && x < width && y >= 0 && y < height)
                _gridCells[x, y].LockCell();
        }

        public bool IsCellLocked(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
                return _gridCells[x,y].IsLocked();
            else return false;
        }

        public bool IsCellSpaceshipArea(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
                return _gridCells[x, y].IsSpaceShipArea();
            else return false;
        }

        #endregion

    }
}


