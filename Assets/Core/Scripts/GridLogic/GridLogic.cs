using InterOrbital.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InterOrbital.WorldSystem
{
    public class GridLogic : MonoBehaviour
    {
        private const string TEXTURES_PATH = "Textures/";
        [SerializeField] private bool debug;

        [field: SerializeField] public int width { get; private set; }
        [field: SerializeField]public int height { get; private set; }

        [SerializeField] private Vector3 cellSize = new Vector3(1, 1, 0);
        [SerializeField] private int borderSize;
        [SerializeField] private GameObject _mapChunkPrefab;
        [SerializeField] private GameObject spaceship;
        [SerializeField] private Sprite spaceshipAreaSprite;
        [SerializeField] private Tilemap spaceshipAreaTilemap;
        [SerializeField] private List<string> biomes;
        [SerializeField] private TilemapLayer[] tilemapLayers;

        private Cell[,] _gridCells;
        private int _chunkSize = 10;
        private GameObject _mapChunks;
        private Grid _grid;

        public static GridLogic Instance;

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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector3 center = new Vector3(width / 2, height / 2, 0);
            Vector3 size = new Vector3(width, height, 0);
            Gizmos.DrawWireCube(center, size);
        }

        private void Start()
        {
            if (!debug)
            {
                GenerateWorld();
            }
        }

        #endregion

        #region Private Methods

        private void GenerateWorld()
        {
            CreateMapChunks();
            InitializeGrid();
            CreateRegions();

            foreach (var tilemapLayer in tilemapLayers)
            {
                FillTilemap(tilemapLayer.tilemap, tilemapLayer.minimapTilemap, tilemapLayer.biomesTiles, tilemapLayer.minimapSprite, tilemapLayer.fillMode);
            }

            SpawnSpaceship();
        }

        private void CreateMapChunks()
        {
            int numChunksX = Mathf.CeilToInt((float)width / _chunkSize);
            int numChunksY = Mathf.CeilToInt((float)height / _chunkSize);

            // Dividir el mapa en chunks
            for (int x = 0; x < numChunksX; x++)
            {
                for (int y = 0; y < numChunksY; y++)
                {
                    // Calcular el tamaño real del chunk
                    int chunkWidth = Mathf.Min(_chunkSize, width - x * _chunkSize);
                    int chunkHeight = Mathf.Min(_chunkSize, height - y * _chunkSize);

                    // Calcular la posición del chunk
                    float chunkPosX = x * _chunkSize + chunkWidth * 0.5f;
                    float chunkPosY = y * _chunkSize + chunkHeight * 0.5f;

                    // Crear un nuevo chunk
                    GameObject chunk = Instantiate(_mapChunkPrefab, new Vector3(chunkPosX, chunkPosY, 0), Quaternion.identity, transform);

                    // Ajustar el tamaño del Box Collider 2D al tamaño del chunk
                    chunk.GetComponent<BoxCollider2D>().size = new Vector2(chunkWidth, chunkHeight);
                }
            }
        }

        private void FillTilemap(Tilemap tilemap, Tilemap minimapTilemap, List<BiomeRuleTile> tiles, Sprite minimapSprite, FillMode fillMode)
        {
            switch (fillMode)
            {
                case FillMode.None:
                    break;
                case FillMode.All:
                    FillTilemapAll(tilemap, tiles);
                    FillTilemapBorders(tilemap);
                    GenerateMinimapTilemap(tilemap, minimapTilemap, minimapSprite);
                    AddNoAnimatedTiles(tilemap, tiles);
                    break;
                case FillMode.Random:
                    FillTilemapRandom(tilemap, tiles);
                    FillTilemapBorders(tilemap);
                    GenerateMinimapTilemap(tilemap, minimapTilemap, minimapSprite);
                    AddAnimatedTiles(tilemap, tiles);
                    break;
            }
        }

        private void FillTilemapAll(Tilemap tilemap, List<BiomeRuleTile> tiles)
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

        private void FillTilemapRandom(Tilemap tilemap, List<BiomeRuleTile> tiles)
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

        #region Borders Methods
        private void FillTilemapBorders(Tilemap tilemap)
        {
            if (borderSize > width || borderSize > height)
            {
                borderSize = Mathf.Min(width,height);
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < borderSize; y++)
                {
                    Vector3Int posBottomBorder = new Vector3Int(x, y, 0);
                    TileBase topTile = tilemap.GetTile(posBottomBorder);
                    Vector3Int topSideMapPos = new Vector3Int(x, y + height, 0);
                    tilemap.SetTile(topSideMapPos, topTile);

                    
                    Vector3Int posTopBorder = new Vector3Int(x, y + height - borderSize, 0);
                    TileBase botTile = tilemap.GetTile(posTopBorder);
                    Vector3Int botSideMapPos = new Vector3Int(x, y - borderSize, 0);
                    tilemap.SetTile(botSideMapPos, botTile);

                    if(x < borderSize)
                    {
                        Vector3Int topRightSideMapPos = new Vector3Int(x + width, y + height, 0);
                        tilemap.SetTile(topRightSideMapPos, topTile);

                        Vector3Int botRightSideMapPos = new Vector3Int(x + width, y - borderSize, 0);
                        tilemap.SetTile(botRightSideMapPos, botTile);
                    }

                    if(x >= width - borderSize)
                    {
                        Vector3Int topLeftSideMapPos = new Vector3Int(x - width, y + height, 0);
                        tilemap.SetTile(topLeftSideMapPos, topTile);

                        Vector3Int botLeftSideMapPos = new Vector3Int(x - width, y - borderSize, 0);
                        tilemap.SetTile(botLeftSideMapPos, botTile);
                    }

                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < borderSize; x++)
                {
                    Vector3Int posLeftBorder = new Vector3Int(x, y, 0);
                    TileBase leftTile = tilemap.GetTile(posLeftBorder);
                    Vector3Int leftSideMapPos = new Vector3Int(x + width, y, 0);
                    tilemap.SetTile(leftSideMapPos, leftTile);

                    Vector3Int posRightBorder = new Vector3Int(x + width - borderSize, y, 0);
                    TileBase rightTile = tilemap.GetTile(posRightBorder);
                    Vector3Int rightSideMapPos = new Vector3Int(x - borderSize, y, 0);
                    tilemap.SetTile(rightSideMapPos, rightTile);
                }
            }
        }


        private void FillTilemapBorders(Tilemap tilemap, int x, int y, TileBase tile)
        {
            if(x < borderSize)
            {
                Vector3Int leftSideMapPos = new Vector3Int(x + width, y, 0);
                tilemap.SetTile(leftSideMapPos, tile);
            }
            
            if(x >= width - borderSize)
            {
                Vector3Int rightSideMapPos = new Vector3Int(x - width, y, 0);
                tilemap.SetTile(rightSideMapPos, tile);
            }

            if(y < borderSize)
            {
                Vector3Int topSideMapPos = new Vector3Int(x, y + height, 0);
                tilemap.SetTile(topSideMapPos, tile);
            }
            
            if(y >= height - borderSize)
            {
                Vector3Int botSideMapPos = new Vector3Int(x, y - height, 0);
                tilemap.SetTile(botSideMapPos, tile);
            }

            if (x < borderSize && y < borderSize)
            {
                Vector3Int topRightSideMapPos = new Vector3Int(x + width, y + height, 0);
                tilemap.SetTile(topRightSideMapPos, tile);

            }

            if (x < borderSize && y >= height - borderSize)
            {
                Vector3Int botRightSideMapPos = new Vector3Int(x + width, y - height, 0);
                tilemap.SetTile(botRightSideMapPos, tile);
            }

            if (x >= width - borderSize && y < borderSize)
            {
                Vector3Int topLeftSideMapPos = new Vector3Int(x - width, y + height, 0);
                tilemap.SetTile(topLeftSideMapPos, tile);
            }

            if (x >= width - borderSize && y >= height - borderSize)
            {
                Vector3Int botLeftSideMapPos = new Vector3Int(x - width, y - height, 0);
                tilemap.SetTile(botLeftSideMapPos, tile);
            }
        }

        #endregion

        

        #region Repaint Map Tiles Methods
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

        private void AddNoAnimatedTiles(Tilemap tilemap, List<BiomeRuleTile> tiles)
        {
            Sprite[,] tilemapSprites = GetTilemapSprites(tilemap);
            List<Sprite> spriteList = new List<Sprite>();
            int lastBiomeIndex = -1;

            for (int x = 0; x < Mathf.RoundToInt(_grid.cellSize.x) * width; x++)
            {
                for (int y = 0; y < Mathf.RoundToInt(_grid.cellSize.y) * height; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);

                    int biomeIndex = tiles.FindIndex(tiles => tiles.biome == _gridCells[x, y].biomeType);
                    if (biomeIndex != lastBiomeIndex)
                    {
                        spriteList = GenerateSpriteList(tiles[biomeIndex].animationTiles.textureToChangeRuleTile);
                        lastBiomeIndex = biomeIndex;
                    }

                    Sprite s = spriteList.Find(s => s == tilemapSprites[x, y]);
                    if (s != null)
                    {
                        Tile tile = ScriptableObject.CreateInstance<Tile>();

                        // Asigna el Sprite al Tile
                        tile.sprite = s;
                        tilemap.SetTile(position, tile);
                        FillTilemapBorders(tilemap, x, y, tile);
                    }

                }
            }
        }

        private void AddAnimatedTiles(Tilemap tilemap, List<BiomeRuleTile> tiles)
        {
            Sprite[,] tilemapSprites = GetTilemapSprites(tilemap);
            List<Sprite> spriteList = new List<Sprite>();
            int lastBiomeIndex = -1;

            for (int x = 0; x < Mathf.RoundToInt(_grid.cellSize.x) * width; x++)
            {
                for (int y = 0; y < Mathf.RoundToInt(_grid.cellSize.y) * height; y++)
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
                            FillTilemapBorders(tilemap, x, y, encounteredSprite.animatedTile);
                        }
                    }



                    Sprite s = spriteList.Find(s => s == tilemapSprites[x, y]);
                    if (s != null)
                    {
                        UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();

                        // Asigna el Sprite al Tile
                        tile.sprite = s;
                        tilemap.SetTile(position, tile);
                        FillTilemapBorders(tilemap, x, y, tile);
                    }

                }
            }
        }

        #endregion

        private void GenerateMinimapTilemap(Tilemap origin, Tilemap minimap, Sprite minimapSprite)
        {
            foreach (var pos in origin.cellBounds.allPositionsWithin)
            {
                var sourceTile = origin.GetTile(pos);
                if (sourceTile != null)
                {
                    Tile minimapTile = ScriptableObject.CreateInstance<Tile>();

                    minimapTile.sprite = minimapSprite;
                    minimap.SetTile(pos, minimapTile);
                }
            }
        }

        private void GenerateChunkMinimap()
        {

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

        private void SpawnSpaceship()
        {
            Instantiate(spaceship,new Vector3(width/2, height/2, 0), Quaternion.identity);
            int spaceshipTilesWidth = 6;
            int spaceshipTilesHeight = 6;
            int spaceshipWidth = width / 2 - spaceshipTilesWidth / 2;
            int spaceshipHeight = height / 2;

            Tile spaceshipAreaTile = ScriptableObject.CreateInstance<Tile>();
            spaceshipAreaTile.sprite = spaceshipAreaSprite;

            for (int y = 0; y < spaceshipTilesHeight; y++)
            {
                for (int x = 0; x < spaceshipTilesWidth; x++)
                {
                    if (y == 0)
                    {
                        Vector3Int position = new Vector3Int(x + spaceshipWidth, y + spaceshipHeight, 0);
                        spaceshipAreaTilemap.SetTile(position, spaceshipAreaTile);
                    }
                    LockCell(x + spaceshipWidth, y + spaceshipHeight);
                }
            }

            SpawnSpaceshipArea(2);
        }

        private void SpawnSpaceshipArea(int tier)
        {
            int widthArea = 0;
            int heightArea = 0;

            Tile spaceshipAreaTile = ScriptableObject.CreateInstance<Tile>();
            spaceshipAreaTile.sprite = spaceshipAreaSprite;

            switch (tier)
            {
                case 0:
                    widthArea = 6;
                    heightArea = 4;
                    break;
                case 1:
                    widthArea = 6;
                    heightArea = 6;
                    break;
                case 2:
                    widthArea = 16;
                    heightArea = 10;
                    break;
                default: break;
            }

            int startWidthArea = width / 2 - widthArea/2;
            int startHeightArea = height / 2 - heightArea;

            for (int y = 0; y < heightArea; y++)
            {
                for (int x = 0; x < widthArea; x++)
                {

                    Vector3Int position = new Vector3Int(x + startWidthArea, y + startHeightArea, 0);

                    spaceshipAreaTilemap.SetTile(position,spaceshipAreaTile);
                    MakeSpaceshipArea(x + startWidthArea,y + startHeightArea);
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

        public void MakeSpaceshipArea(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
                _gridCells[x, y].MakeSpaceshipArea();
            
        }

        #endregion

    }
}


