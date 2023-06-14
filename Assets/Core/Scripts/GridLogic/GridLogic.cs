using InterOrbital.Player;
using InterOrbital.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Sprites;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

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
        [SerializeField] private GameObject _borderPrefab;
        [SerializeField] private GameObject _mapChunkPrefab;
        [SerializeField] private GameObject _spaceship;
        [SerializeField] private Sprite _spaceshipAreaSprite;
        [SerializeField] private Tilemap _spaceshipAreaTilemap;
        [SerializeField] private List<string> _biomes;
        [SerializeField] private TilemapLayer[] _tilemapLayers;

        private Cell[,] _gridCells;
        private int _chunkSize = 5;
        private Grid _grid;
        private Dictionary<Vector2Int,Chunk> _chunks;
        private Dictionary<Tilemap,Sprite[,]> _tilemapSprites;

        public static GridLogic Instance;

        #region Unity Methods

        protected virtual void Awake()
        {
            if (Instance == null)
                Instance = this;

            _grid = GetComponent<Grid>();
            _grid.cellSize = cellSize;
            _tilemapSprites = new Dictionary<Tilemap, Sprite[,]>();
        }

        private void OnValidate()
        {
            if(width % _chunkSize != 0)
            {
                width += _chunkSize - (width % _chunkSize);
            }

            if (height % _chunkSize != 0)
            {
                height += _chunkSize - (height % _chunkSize);
            }

            if (borderSize % _chunkSize != 0)
            {
                borderSize += _chunkSize - (borderSize % _chunkSize);
            }

            for (int i = 0; i < _biomes.Count; i++)
            {
                // Verificar si el bioma ya existe en la lista de biomas con tiles de reglas
                foreach (var tilemapLayer in _tilemapLayers)
                {
                    bool existe = false;
                    for (int j = 0; j < tilemapLayer.biomesTiles.Count; j++)
                    {
                        if (tilemapLayer.biomesTiles[j].biome == _biomes[i])
                        {
                            existe = true;
                            break;
                        }
                    }

                    // Si el bioma no existe, crear un nuevo struct de bioma con tile de regla y agregarlo a la lista
                    if (!existe)
                    {
                        BiomeRuleTile newBiome = new BiomeRuleTile();
                        newBiome.biome = _biomes[i];
                        tilemapLayer.biomesTiles.Add(newBiome);
                    }
                }
            }


            foreach (var tilemapLayer in _tilemapLayers)
            {
                for (int i = tilemapLayer.biomesTiles.Count - 1; i >= 0; i--) // Recorrer la lista de biomas con tiles de reglas
                {
                    if (!_biomes.Contains(tilemapLayer.biomesTiles[i].biome)) // Verificar si el bioma ya no está en la lista de biomas
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
                StartCoroutine(GenerateWorld());
            }
        }

        #endregion

        #region Private Methods

        private IEnumerator GenerateWorld()
        {
            CreateMapChunks();
            CreateMapBorders();
            InitializeGrid();
            CreateRegions();

            foreach (var tilemapLayer in _tilemapLayers)
            {
                FillTilemap(tilemapLayer.tilemap, tilemapLayer.biomesTiles, tilemapLayer.fillMode);
            }

            SpawnSpaceship();
            yield return new WaitForSeconds(0.1f);
            PlayerComponents.Instance.GetComponent<PlayerMovement>().ActivateMinimapDetector();
        }

        private void CreateMapBorders()
        {
            for (int x = -1; x < width + 1; x++)
            {
                Vector3 positionBot = new Vector3(x, -1f, 0); 
                Instantiate(_borderPrefab, positionBot, Quaternion.identity);
                Vector3 positionTop = new Vector3(x, height, 0);
                Instantiate(_borderPrefab, positionTop, Quaternion.identity);
            }

            for (int y = 0; y < height; y++)
            {
                Vector3 positionLeft = new Vector3(-1, y, 0); 
                Instantiate(_borderPrefab, positionLeft, Quaternion.identity);
                Vector3 positionRight = new Vector3(width, y, 0); 
                Instantiate(_borderPrefab, positionRight, Quaternion.identity);
            }
        }
        private void CreateMapChunks()
        {
            _chunks = new Dictionary<Vector2Int, Chunk>();

            int numChunksX = Mathf.CeilToInt((float)width / _chunkSize);
            int numChunksY = Mathf.CeilToInt((float)height / _chunkSize);
            // Dividir el mapa en chunks
            for (int x = 0; x < numChunksX; x++)
            {
                for (int y = 0; y < numChunksY; y++)
                {
                    // Calcular la posición del chunk
                    int chunkPosX = x * _chunkSize;
                    int chunkPosY = y * _chunkSize;

                    // Crear un nuevo chunk
                    GameObject chunk = Instantiate(_mapChunkPrefab, new Vector3(chunkPosX, chunkPosY, 0), Quaternion.identity, transform);

                    // Ajustar el tamaño del Box Collider 2D al tamaño del chunk
                    chunk.GetComponent<BoxCollider2D>().size = new Vector2(_chunkSize, _chunkSize);
                    chunk.GetComponent<BoxCollider2D>().offset = new Vector2(_chunkSize / 2f, _chunkSize / 2f);
                    chunk.GetComponent<Chunk>().SetChunkPos(chunkPosX, chunkPosY);

                    _chunks.Add(new Vector2Int(chunkPosX,chunkPosY),chunk.GetComponent<Chunk>());
                }   
            }
        }

        private void FillTilemap(Tilemap tilemap, List<BiomeRuleTile> tiles, FillMode fillMode)
        {
            switch (fillMode)
            {
                case FillMode.None:
                    break;
                case FillMode.All:
                    FillTilemapAll(tilemap, tiles);
                    break;
                case FillMode.Random:
                    FillTilemapRandom(tilemap, tiles);
                    break;
            }
        }

        private void FillTilemapAll(Tilemap tilemap, List<BiomeRuleTile> tiles)
        {
            int gridSizeX = Mathf.RoundToInt(_grid.cellSize.x) * width;
            int gridSizeY = Mathf.RoundToInt(_grid.cellSize.y) * height;

            Dictionary<string, int> biomeIndexMap = new Dictionary<string, int>();

            for (int i = 0; i < tiles.Count; i++)
            {
                biomeIndexMap[tiles[i].biome] = i;
            }

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);
                    string currentBiome = _gridCells[x, y].biomeType;

                    if (biomeIndexMap.TryGetValue(currentBiome, out int biomeIndex))
                    {
                        if (tiles[biomeIndex].tiles != null)
                        {
                            tilemap.SetTile(position, tiles[biomeIndex].tiles);
                        }
                    }
                }
            }
        }

        private void FillTilemapRandom(Tilemap tilemap, List<BiomeRuleTile> tiles)
        {
            int detailZones = width + height / 2;
            int gridSizeX = Mathf.RoundToInt(_grid.cellSize.x) * width;
            int gridSizeY = Mathf.RoundToInt(_grid.cellSize.y) * height;
            int tilesCount = tiles.Count;
            int halfDetailZones = detailZones / 2;

            for (int i = 0; i < detailZones; i++)
            {
                int x = UnityEngine.Random.Range(0, width);
                int y = UnityEngine.Random.Range(0, height);
                int extension = UnityEngine.Random.Range(1, halfDetailZones);

                RandomDetail(x, y, extension);
            }

            Dictionary<string, int> biomeIndexMap = new Dictionary<string, int>(tilesCount);

            for (int i = 0; i < tilesCount; i++)
            {
                biomeIndexMap[tiles[i].biome] = i;
            }

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    if (!_gridCells[x, y].HaveDetail())
                    {
                        continue;
                    }

                    Vector3Int position = new Vector3Int(x, y, 0);
                    string currentBiome = _gridCells[x, y].biomeType;

                    if (biomeIndexMap.TryGetValue(currentBiome, out int biomeIndex))
                    {
                        if (tiles[biomeIndex].tiles != null)
                        {
                            tilemap.SetTile(position, tiles[biomeIndex].tiles);
                        }
                    }
                }
            }
        }

        #region Borders Methods
        private void FillTilemapBorders(Tilemap tilemap)
        {
            if (borderSize > width || borderSize > height)
            {
                borderSize = Mathf.Min(width, height);
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

                    if (x < borderSize)
                    {
                        Vector3Int topRightSideMapPos = new Vector3Int(x + width, y + height, 0);
                        tilemap.SetTile(topRightSideMapPos, topTile);

                        Vector3Int botRightSideMapPos = new Vector3Int(x + width, y - borderSize, 0);
                        tilemap.SetTile(botRightSideMapPos, botTile);
                    }

                    if (x >= width - borderSize)
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
            if (x < borderSize || x >= width - borderSize || y < borderSize || y >= height - borderSize) {
                if (x < borderSize && y >= borderSize && y < height - borderSize)
                {
                    Vector3Int leftSideMapPos = new Vector3Int(x + width, y, 0);
                    tilemap.SetTile(leftSideMapPos, tile);
                }

                if (x >= width - borderSize && y >= borderSize && y < height - borderSize)
                {
                    Vector3Int rightSideMapPos = new Vector3Int(x - width, y, 0);
                    tilemap.SetTile(rightSideMapPos, tile);
                }

                if (y < borderSize)
                {
                    Vector3Int topSideMapPos = new Vector3Int(x, y + height, 0);
                    tilemap.SetTile(topSideMapPos, tile);
                }

                if (y >= height - borderSize)
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
        }

        #endregion



        #region Repaint Map Tiles Methods
        private void GetTilemapSprites(Tilemap tilemap)
        {
            Sprite[,] tilemapSprites = new Sprite[tilemap.size.x, tilemap.size.y];

            for (int x = 0; x < tilemap.size.x; x++)
            {
                for (int y = 0; y < tilemap.size.y; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);

                    tilemapSprites[x, y] = tilemap.GetSprite(position);
                }
            }

            _tilemapSprites.Add(tilemap, tilemapSprites);
        }

        private List<Sprite> GenerateSpriteList(Texture2D texture)
        {
            List<Sprite> spriteList = new List<Sprite>();

            string texturePath = TEXTURES_PATH + texture.name;
            // Obtiene todos los sprites generados por el Sprite Editor
            Sprite[] sprites = Resources.LoadAll<Sprite>(texturePath); // Asigna el nombre de la textura como nombre de la carpeta de recursos

            if (sprites != null)
            {
                spriteList.AddRange(sprites);
            }

            return spriteList;
        }

        public void SubstituteRuleTiles(int chunkX, int chunkY)
        {
            foreach (var tilemapLayer in _tilemapLayers)
            {
                _tilemapSprites.TryGetValue(tilemapLayer.tilemap, out Sprite[,] tilemapSprites);

                for (int x = chunkX; x < chunkX + _chunkSize; x++)
                {
                    for (int y = chunkY; y < chunkY + _chunkSize; y++)
                    {
                        Vector3Int position = new Vector3Int(x, y, 0);

                        Sprite sprite = tilemapSprites[x, y];
                        if (sprite != null)
                        {
                            UnityEngine.Tilemaps.Tile tile = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
                            // Asigna el Sprite al Tile
                            tile.sprite = sprite;
                            tilemapLayer.tilemap.SetTile(position, tile);
                            FillTilemapBorders(tilemapLayer.tilemap, x, y, tile);
                        }

                    }
                }
            }
        }

       /* private void AddNoAnimatedTiles(Tilemap tilemap, List<BiomeRuleTile> tiles)
        {
           /Sprite[,] tilemapSprites = GetTilemapSprites(tilemap);
            int gridSizeX = Mathf.RoundToInt(_grid.cellSize.x) * width;
            int gridSizeY = Mathf.RoundToInt(_grid.cellSize.y) * height;
            Dictionary<string, List<Sprite>> spriteDictionary = new Dictionary<string, List<Sprite>>();

            foreach (BiomeRuleTile tile in tiles)
            {
                if (tile.animationTiles != null)
                {
                    List<Sprite> spriteList = GenerateSpriteList(tile.animationTiles.textureToChangeRuleTile);
                    if (!spriteDictionary.ContainsKey(tile.biome))
                    {
                        spriteDictionary.Add(tile.biome, spriteList);
                    }
                }
            }

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);

                    spriteDictionary.TryGetValue(_gridCells[x, y].biomeType, out List<Sprite> spriteList);
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

        private void AddAnimatedTiles(Tilemap tilemap, List<BiomeRuleTile> tiles)
        {
            Sprite[,] tilemapSprites = GetTilemapSprites(tilemap);
            int gridSizeX = Mathf.RoundToInt(_grid.cellSize.x) * width;
            int gridSizeY = Mathf.RoundToInt(_grid.cellSize.y) * height;
            Dictionary<string, List<Sprite>> spriteDictionary = new Dictionary<string, List<Sprite>>();
            Dictionary<string, List<SpriteAnimatedTile>> spriteAnimatedDictionary = new Dictionary<string, List<SpriteAnimatedTile>>();

            foreach (BiomeRuleTile tile in tiles)
            {
                if (tile.animationTiles != null)
                {
                    List<Sprite> spriteList = GenerateSpriteList(tile.animationTiles.textureToChangeRuleTile);
                    if (!spriteDictionary.ContainsKey(tile.biome))
                    {
                        spriteDictionary.Add(tile.biome, spriteList);
                    }
                    if (!spriteAnimatedDictionary.ContainsKey(tile.biome) && tile.animationTiles.spriteToAnimatedTiles != null)
                    {
                        spriteAnimatedDictionary.Add(tile.biome, tile.animationTiles.spriteToAnimatedTiles);
                    }
                }
            }

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);

                    spriteAnimatedDictionary.TryGetValue(_gridCells[x, y].biomeType, out List<SpriteAnimatedTile> spriteAnimatedList);
                    if (spriteAnimatedList != null)
                    {
                        SpriteAnimatedTile encounteredSprite = spriteAnimatedList.Find(spriteAnimado => spriteAnimado.sprite == tilemapSprites[x, y]);

                        if (encounteredSprite.sprite != null)
                        {
                            tilemap.SetTile(position, encounteredSprite.animatedTile);
                            FillTilemapBorders(tilemap, x, y, encounteredSprite.animatedTile);
                        }
                    }


                    spriteDictionary.TryGetValue(_gridCells[x, y].biomeType, out List<Sprite> spriteList);
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
       */
        #endregion


        private void GenerateMinimapTilemap(Tilemap origin, Tilemap minimap, List<BiomeRuleTile> tiles, int chunkXPos, int chunkYPos)
        {

            for (int j = chunkYPos; j < (chunkYPos + _chunkSize); j++)
            {
                for (int i = chunkXPos; i < (chunkXPos + _chunkSize); i++)
                {
                    Vector3Int pos = new Vector3Int(i, j, 0);

                    var sourceTile = origin.GetTile(pos);
                    if (sourceTile != null)
                    {
                        Vector2Int mapCoordinates = BorderIntoMapCoordinates(i, j);
                        int biomeIndex = tiles.FindIndex(tiles => tiles.biome == _gridCells[mapCoordinates.x, mapCoordinates.y].biomeType);

                        if (tiles[biomeIndex].minimapSprite != null)
                        {
                            Tile minimapTile = ScriptableObject.CreateInstance<Tile>();
                            minimapTile.sprite = tiles[biomeIndex].minimapSprite;
                            minimap.SetTile(pos, minimapTile);
                        }
                    }
                }

            }

        }

        private Vector2Int BorderIntoMapCoordinates(int x, int y)
        {
            Vector2Int mapCoordinates = new Vector2Int(x,y);
            if (x < 0)
            {
                mapCoordinates.x += width;
            }

            if (x >= width)
            {
                mapCoordinates.x -= width;
            }

            if (y < 0)
            {
                mapCoordinates.y += height;
            }

            if (y >= height)
            {
                mapCoordinates.y -= height;
            }

            return mapCoordinates;
        }

        public void GenerateChunkMinimap(int chunkXPos, int chunkYPos)
        {
            Vector2Int chunkPos = new Vector2Int(chunkXPos, chunkYPos);

            _chunks.TryGetValue(chunkPos, out Chunk chunkFound);

            if (chunkFound != null && !chunkFound.IsRevealed())
            {
                foreach (var tilemapLayer in _tilemapLayers)
                {
                    GenerateMinimapTilemap(tilemapLayer.tilemap, tilemapLayer.minimapTilemap, tilemapLayer.biomesTiles, chunkXPos, chunkYPos);
                }
            }
            chunkFound.SetRevealed(true);

        }

        private void InitializeGrid()
        {
            _gridCells = new Cell[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _gridCells[i, j] = new Cell(i, j, _biomes[0]);
                }
            }
        }

        #region Spaceship

        private void SpawnSpaceship()
        {
            Instantiate(_spaceship,new Vector3(width/2, height/2, 0), Quaternion.identity);
            int spaceshipTilesWidth = 6;
            int spaceshipTilesHeight = 6;
            int spaceshipWidth = width / 2 - spaceshipTilesWidth / 2;
            int spaceshipHeight = height / 2;

            Tile spaceshipAreaTile = ScriptableObject.CreateInstance<Tile>();
            spaceshipAreaTile.sprite = _spaceshipAreaSprite;

            for (int y = 0; y < spaceshipTilesHeight; y++)
            {
                for (int x = 0; x < spaceshipTilesWidth; x++)
                {
                    if (y == 0)
                    {
                        Vector3Int position = new Vector3Int(x + spaceshipWidth, y + spaceshipHeight, 0);
                        _spaceshipAreaTilemap.SetTile(position, spaceshipAreaTile);
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
            spaceshipAreaTile.sprite = _spaceshipAreaSprite;

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

                    _spaceshipAreaTilemap.SetTile(position,spaceshipAreaTile);
                    MakeSpaceshipArea(x + startWidthArea,y + startHeightArea);
                }
            }
        }

        #endregion

        private void CreateRegions()
        {
            RandomBiomeCreation(10, 40, _biomes[1], 450);
            RandomBiomeCreation(20, 10, _biomes[2], 450);
            RandomBiomeCreation(40, 10, _biomes[3], 450);
        }

        private Vector2Int UpdatePosToMap(Vector2Int pos)
        {
            Vector2Int mapPos = new Vector2Int(pos.x, pos.y);

            if (mapPos.x < 0)
            {
                mapPos.x = width;
            }

            if (mapPos.x >= width)
            {
                mapPos.x = 0;
            }

            if (mapPos.y < 0)
            {
                mapPos.y = height;
            }

            if (mapPos.y >= height)
            {
                mapPos.y = 0;
            }

            return mapPos;
        }

        private void RandomBiomeCreation(int x, int y, string biome, int extension)
        {
            if (x >= 0 && x < width && y >= 0 && y < height && extension < (height * width))
            {
                HashSet<Vector2Int> visited = new HashSet<Vector2Int>(); // Conjunto de casillas visitadas
                List<Vector2Int> positions = new List<Vector2Int>(); // Lista de posiciones para la extensión

                Vector2Int startPos = new Vector2Int(x, y);
                positions.Add(startPos);
                visited.Add(startPos);
                _gridCells[startPos.x, startPos.y].biomeType = biome;

                while (positions.Count > 0 && visited.Count < extension)
                {
                    int randomIndex = UnityEngine.Random.Range(0, positions.Count);
                    Vector2Int currentPos = positions[randomIndex];

                    List<Vector2Int> directions = GetValidDirections(currentPos, biome);
                    if (directions.Count == 0)
                    {
                        positions.RemoveAt(randomIndex);
                        continue;
                    }

                    int randomDirectionIndex = UnityEngine.Random.Range(0, directions.Count);
                    Vector2Int direction = directions[randomDirectionIndex];
                    Vector2Int newPos = currentPos + direction;

                    newPos = UpdatePosToMap(newPos);

                    visited.Add(newPos);

                    _gridCells[newPos.x, newPos.y].biomeType = biome;

                    List<Vector2Int> newDirections = GetValidDirections(newPos, biome);
                    if (newDirections.Count > 0)
                    {
                        positions.Add(newPos);
                    }
                }
            }
        }

        private List<Vector2Int> GetValidDirections(Vector2Int pos,string biome)
        {
            List<Vector2Int> directions = new List<Vector2Int>();

            Vector2Int[] offsets =
            {
        new Vector2Int(0, 1), // Arriba
        new Vector2Int(0, -1), // Abajo
        new Vector2Int(1, 0), // Derecha
        new Vector2Int(-1, 0) // Izquierda
    };

            foreach (Vector2Int offset in offsets)
            {
                Vector2Int neighborPos = pos + offset;

                neighborPos = UpdatePosToMap(neighborPos);

                if (_gridCells[neighborPos.x, neighborPos.y].biomeType != biome)
                {
                    directions.Add(offset);
                }
            }

            return directions;
        }

        private List<Vector2Int> GetDirectionsWithoutDetail(Vector2Int pos)
        {
            List<Vector2Int> directions = new List<Vector2Int>();

            Vector2Int[] offsets =
            {
        new Vector2Int(0, 1), // Arriba
        new Vector2Int(0, -1), // Abajo
        new Vector2Int(1, 0), // Derecha
        new Vector2Int(-1, 0) // Izquierda
    };

            foreach (Vector2Int offset in offsets)
            {
                Vector2Int neighborPos = pos + offset;

                neighborPos = UpdatePosToMap(neighborPos);

                if (!_gridCells[neighborPos.x, neighborPos.y].HaveDetail())
                {
                    directions.Add(offset);
                }
            }
            return directions;
        }

        private void RandomDetail(int x, int y, int extension)
        {
            if (x >= 0 && x < width && y >= 0 && y < height && extension < (height * width))
            {
                HashSet<Vector2Int> visited = new HashSet<Vector2Int>(); // conjunto de casillas visitadas
                List<Vector2Int> posiblePositions = new List<Vector2Int>(); // Lista de posiciones para la extensión

                Vector2Int startPos = new Vector2Int(x, y);
                posiblePositions.Add(startPos);
                visited.Add(startPos);
                _gridCells[startPos.x, startPos.y].AddDetail();

                while (posiblePositions.Count > 0 && visited.Count < extension)
                {
                    int randomIndex = UnityEngine.Random.Range(0, posiblePositions.Count);
                    Vector2Int currentPos = posiblePositions[randomIndex];

                    List<Vector2Int> directions = GetDirectionsWithoutDetail(currentPos);
                    if (directions.Count == 0)
                    {
                        posiblePositions.RemoveAt(randomIndex);
                        continue;
                    }

                    int randomDirectionIndex = UnityEngine.Random.Range(0, directions.Count);
                    Vector2Int direction = directions[randomDirectionIndex];
                    Vector2Int newPos = currentPos + direction;

                    newPos = UpdatePosToMap(newPos);

                    visited.Add(newPos);
                    _gridCells[newPos.x, newPos.y].AddDetail();

                    List<Vector2Int> newDirections = GetDirectionsWithoutDetail(newPos);
                    if (newDirections.Count > 0)
                    {
                        posiblePositions.Add(newPos);
                    }
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

        public int GetChunkSize()
        {
            return _chunkSize;
        }

        #endregion

    }
}


