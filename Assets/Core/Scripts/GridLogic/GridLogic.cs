using InterOrbital.Combat.Spawner;
using InterOrbital.Item;
using InterOrbital.Player;
using InterOrbital.Spaceship;
using InterOrbital.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using InterOrbital.Others;
using InterOrbital.Recollectables.Spawner;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InterOrbital.WorldSystem
{
    public class GridLogic : MonoBehaviour
    {
        private const float EASY_PERCENTAGE_AREA = 0.4f;
        private const float MEDIUM_PERCENTAGE_AREA = 0.75f;
        private const int BASE_WIDTH = 16;
        private const int BASE_HEIGHT = 16;


        [SerializeField] private bool debug;

        [field: SerializeField] public int width { get; private set; }
        [field: SerializeField] public int height { get; private set; }

        [SerializeField] private Vector3 cellSize = new Vector3(1, 1, 0);
        [SerializeField] private int borderSize;
        [SerializeField] private GameObject _borderPrefab;
        [SerializeField] private GameObject _mapChunkPrefab;
        [SerializeField] private GameObject _spaceship;
        [SerializeField] private Sprite _spaceshipAreaSprite;
        [SerializeField] private Tilemap _spaceshipAreaTilemap;
        [SerializeField] private List<string> _biomes;
        [SerializeField] private List<Region> _regions;
        [SerializeField] private MapEnemySpawnersSO _enemiesSpawners;
        [SerializeField] private MapResourcesSpawnersSO _resourcesSpawners;
        [SerializeField] private Tilemap _animationTilemap;
        [SerializeField] private TileAnimationSO _animationTiles;
        [SerializeField] private TilemapLayer[] _tilemapLayers;
        [SerializeField] private float minCenterSpawnDistance = 38f;

        private List<EnemySpawner> _enemiesSpawnersList = new();
        private List<ResourcesSpawner> _resourcesSpawnersList = new();
        private Cell[,] _gridCells;
        private int _chunkSize = 5;
        private Grid _grid;
        private Dictionary<Vector2Int, Chunk> _chunks;
        private Dictionary<Tilemap, Sprite[,]> _tilemapSprites;

        public static GridLogic Instance;

        public Action OnTilemapFilled;

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
            if (width % _chunkSize != 0)
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
                for (int i = tilemapLayer.biomesTiles.Count - 1;
                     i >= 0;
                     i--) // Recorrer la lista de biomas con tiles de reglas
                {
                    if (!_biomes.Contains(tilemapLayer.biomesTiles[i]
                            .biome)) // Verificar si el bioma ya no est� en la lista de biomas
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

            Gizmos.color = Color.blue;
            Vector3 sizeEasy = new Vector3(width * EASY_PERCENTAGE_AREA, height * EASY_PERCENTAGE_AREA, 0);
            Gizmos.DrawWireCube(center, sizeEasy);

            Gizmos.color = Color.yellow;
            Vector3 sizeMedium = new Vector3(width * MEDIUM_PERCENTAGE_AREA, height * MEDIUM_PERCENTAGE_AREA, 0);
            Gizmos.DrawWireCube(center, sizeMedium);
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
            OnTilemapFilled?.Invoke();
            GenerateEnemySpawners();
            GenerateResourcesSpawners();
            PlayerComponents.Instance.GetComponent<PlayerMovement>().ActivateMinimapDetector();
            if (LevelManager.Instance != null)
                LevelManager.Instance.PlayGame();
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
                    // Calcular la posici�n del chunk
                    int chunkPosX = x * _chunkSize;
                    int chunkPosY = y * _chunkSize;

                    // Crear un nuevo chunk
                    GameObject chunk = Instantiate(_mapChunkPrefab, new Vector3(chunkPosX, chunkPosY, 0),
                        Quaternion.identity, transform);

                    // Ajustar el tama�o del Box Collider 2D al tama�o del chunk
                    chunk.GetComponent<BoxCollider2D>().size = new Vector2(_chunkSize, _chunkSize);
                    chunk.GetComponent<BoxCollider2D>().offset = new Vector2(_chunkSize / 2f, _chunkSize / 2f);
                    chunk.GetComponent<Chunk>().SetChunkPos(chunkPosX, chunkPosY);

                    _chunks.Add(new Vector2Int(chunkPosX, chunkPosY), chunk.GetComponent<Chunk>());
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
                    _gridCells[i, j] = new Cell(i, j, _biomes[0]);
                }
            }
        }

        private void GenerateEnemySpawners()
        {
            Vector2 mapCenter = new Vector2(width / 2, height / 2);

            _enemiesSpawners.ResetSpawners();

            for (int i = 0; i < _enemiesSpawners.easySpawnersAmount; i++)
            {
                Vector2 spawnPosition = GetRandomSpawnerPosition(mapCenter, minCenterSpawnDistance,
                    _enemiesSpawners.distanceBetweenSpawners, "EnemySpawner", DifficultyArea.Easy);
                InstantiateEnemySpawner(_enemiesSpawners.GetEnemySpawnerByDifficultArea(DifficultyArea.Easy), spawnPosition);
            }

            for (int i = 0; i < _enemiesSpawners.mediumSpawnersAmount; i++)
            {
                Vector2 spawnPosition = GetRandomSpawnerPosition(mapCenter, minCenterSpawnDistance,
                    _enemiesSpawners.distanceBetweenSpawners, "EnemySpawner", DifficultyArea.Medium);
                InstantiateEnemySpawner(_enemiesSpawners.GetEnemySpawnerByDifficultArea(DifficultyArea.Medium), spawnPosition);
            }

            for (int i = 0; i < _enemiesSpawners.hardSpawnersAmount; i++)
            {
                Vector2 spawnPosition = GetRandomSpawnerPosition(mapCenter, minCenterSpawnDistance,
                    _enemiesSpawners.distanceBetweenSpawners, "EnemySpawner", DifficultyArea.Hard);
                InstantiateEnemySpawner(_enemiesSpawners.GetEnemySpawnerByDifficultArea(DifficultyArea.Hard), spawnPosition);
            }

            _enemiesSpawners.ResetSpawners();
        }

        private void InstantiateEnemySpawner(GameObject spawner, Vector2 spawnPosition)
        {
            if (spawnPosition.x >= 0 && spawnPosition.y >= 0)
            {
                if (spawner == null) return;
                var spawnerObject = Instantiate(spawner, spawnPosition, Quaternion.identity);
                if (spawnerObject.TryGetComponent(out EnemySpawner enemySpawner))
                    _enemiesSpawnersList.Add(enemySpawner);
            }
        }

        private void InstantiateResourcesSpawner(Vector2 spawnPosition, DifficultyArea area)
        {
            if (spawnPosition.x >= 0 && spawnPosition.y >= 0)
            {
                string currentBiome = _gridCells[(int)spawnPosition.x, (int)spawnPosition.y].biomeType;
                GameObject spawner =
                    _resourcesSpawners.GetResourceSpawnerByDifficultArea(area, currentBiome);
                if (spawner == null) return;
                var spawnerObject = Instantiate(spawner, spawnPosition, Quaternion.identity);
                if (spawnerObject.TryGetComponent(out ResourcesSpawner resourcesSpawner))
                    _resourcesSpawnersList.Add(resourcesSpawner);
            }
        }

        private void GenerateResourcesSpawners()
        {
            Vector2 mapCenter = new Vector2(width / 2, height / 2);

            _resourcesSpawners.ResetSpawners();

            for (int i = 0; i < _resourcesSpawners.easySpawnersAmount; i++)
            {
                Vector2 spawnPosition = GetRandomSpawnerPosition(mapCenter, minCenterSpawnDistance,
                    _resourcesSpawners.distanceBetweenSpawners, "ResourceSpawner", DifficultyArea.Easy);
                InstantiateResourcesSpawner(spawnPosition, DifficultyArea.Easy);
            }

            for (int i = 0; i < _resourcesSpawners.mediumSpawnersAmount; i++)
            {
                Vector2 spawnPosition = GetRandomSpawnerPosition(mapCenter, minCenterSpawnDistance,
                    _resourcesSpawners.distanceBetweenSpawners, "ResourceSpawner", DifficultyArea.Medium);
                InstantiateResourcesSpawner(spawnPosition, DifficultyArea.Medium);
            }

            for (int i = 0; i < _resourcesSpawners.hardSpawnersAmount; i++)
            {
                Vector2 spawnPosition = GetRandomSpawnerPosition(mapCenter, minCenterSpawnDistance,
                    _resourcesSpawners.distanceBetweenSpawners, "ResourceSpawner", DifficultyArea.Hard);
                InstantiateResourcesSpawner(spawnPosition, DifficultyArea.Hard);
            }

            _resourcesSpawners.ResetSpawners();
        }


        private Vector2 GetRandomSpawnerPosition(Vector2 center, float minDistanceFromCenter,
            float minDistanceBetweenSpawners, string tag, DifficultyArea area)
        {
            Vector2 spawnPosition = Vector2.zero;

            bool positionValid = false;

            Vector3Int areaBounds = GetAreaBounds(area);
            Vector3Int innerAreaBounds = GetInnerAreaBounds(area);

            for (int cont = 0; cont < 10000 && !positionValid; cont++)
            {
                do
                {
                    int posX = UnityEngine.Random.Range(areaBounds.x, areaBounds.y);
                    int posY = UnityEngine.Random.Range(areaBounds.x, areaBounds.z);
                    if (posX >= innerAreaBounds.x && posX < innerAreaBounds.y)
                    {
                        int posOutBottomInnerBounds = UnityEngine.Random.Range(areaBounds.x, innerAreaBounds.x);
                        int posOutTopInnerBounds = UnityEngine.Random.Range(innerAreaBounds.z, areaBounds.z);
                        int posOutInnerBounds = UnityEngine.Random.value < 0.5f
                            ? posOutBottomInnerBounds
                            : posOutTopInnerBounds;
                        posY = posOutInnerBounds;
                    }

                    spawnPosition = new Vector2(posX, posY);
                } while (Vector2.Distance(spawnPosition, center) <= minDistanceFromCenter);

                positionValid = true;

                Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, minDistanceBetweenSpawners);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].CompareTag(tag))
                    {
                        positionValid = false;
                        break;
                    }
                }
            }

            if (!positionValid)
            {
                spawnPosition = new Vector2(-1, -1);
            }

            return spawnPosition;
        }

        private Vector3Int GetAreaBounds(DifficultyArea area)
        {
            Vector3Int areaBounds = new Vector3Int(0, width, height);
            if (area == DifficultyArea.Easy)
            {
                float innerWidth = width * EASY_PERCENTAGE_AREA;
                float innerHeight = height * EASY_PERCENTAGE_AREA;
                int leftBotBound = (int)((width / 2) - (innerWidth / 2f));
                int rightBotBound = (int)((width / 2) + (innerWidth / 2f));
                int leftTopBound = (int)((height / 2) + (innerHeight / 2f));

                areaBounds = new Vector3Int(leftBotBound, rightBotBound, leftTopBound);
            }
            else if (area == DifficultyArea.Medium)
            {
                float innerWidth = width * MEDIUM_PERCENTAGE_AREA;
                float innerHeight = height * MEDIUM_PERCENTAGE_AREA;
                int leftBotBound = (int)((width / 2) - (innerWidth / 2f));
                int rightBotBound = (int)((width / 2) + (innerWidth / 2f));
                int leftTopBound = (int)((height / 2) + (innerHeight / 2f));

                areaBounds = new Vector3Int(leftBotBound, rightBotBound, leftTopBound);
            }

            return areaBounds;
        }

        private Vector3Int GetInnerAreaBounds(DifficultyArea area)
        {
            Vector3Int areaBounds = new Vector3Int(0, width, height);
            if (area == DifficultyArea.Easy)
            {
                //BaseArea
                int leftBotBound = (width / 2) - (BASE_WIDTH / 2);
                int rightBotBound = (width / 2) + (BASE_WIDTH / 2);
                int leftTopBound = (height / 2) + (BASE_HEIGHT / 2);
                areaBounds = new Vector3Int(leftBotBound, rightBotBound, leftTopBound);
            }
            else if (area == DifficultyArea.Medium)
            {
                areaBounds = GetAreaBounds(DifficultyArea.Easy);
            }
            else if (area == DifficultyArea.Hard)
            {
                areaBounds = GetAreaBounds(DifficultyArea.Medium);
            }

            return areaBounds;
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
                            if (_gridCells[x, y].HaveAnimationTile())
                            {
                                _animationTilemap.SetTile(position,
                                    _animationTiles.GetRandomTileFromBiome(currentBiome));
                            }
                        }
                    }
                }
            }
        }


        #region Minimap Generation

        private void GenerateMinimapTilemap(Tilemap origin, Tilemap minimap, List<BiomeRuleTile> tiles, int chunkXPos,
            int chunkYPos)
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
                        int biomeIndex = tiles.FindIndex(tiles =>
                            tiles.biome == _gridCells[mapCoordinates.x, mapCoordinates.y].biomeType);

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
            Vector2Int mapCoordinates = new Vector2Int(x, y);
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
                    GenerateMinimapTilemap(tilemapLayer.tilemap, tilemapLayer.minimapTilemap, tilemapLayer.biomesTiles,
                        chunkXPos, chunkYPos);
                }
            }

            chunkFound.SetRevealed(true);
        }

        #endregion


        #region Spaceship

        private void SpawnSpaceship()
        {
            //GameObject spaceship = Instantiate(_spaceship,new Vector3(width/2, height/2, 0), Quaternion.identity);
            SpaceshipComponents.Instance.MoveTo(new Vector3Int(width / 2, height / 2, 0));
            SpawnSpaceshipArea(5);
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
                    widthArea = 8;
                    heightArea = 6;
                    break;
                case 3:
                    widthArea = 10;
                    heightArea = 6;
                    break;
                case 4:
                    widthArea = 10;
                    heightArea = 8;
                    break;
                case 5:
                    widthArea = 12;
                    heightArea = 8;
                    break;
                default: break;
            }

            int startWidthArea = width / 2 - widthArea / 2;
            int startHeightArea = height / 2 - heightArea;

            for (int y = 0; y < heightArea; y++)
            {
                for (int x = 0; x < widthArea; x++)
                {
                    Vector3Int position = new Vector3Int(x + startWidthArea, y + startHeightArea, 0);

                    _spaceshipAreaTilemap.SetTile(position, spaceshipAreaTile);
                    MakeSpaceshipArea(x + startWidthArea, y + startHeightArea);
                }
            }
        }

        #endregion

        private void CreateRegions()
        {
            foreach (Region region in _regions)
            {
                if (region.startPos.x >= 0 && region.startPos.x < width && region.startPos.y >= 0 &&
                    region.startPos.y < height)
                {
                    if (_biomes.Contains(region.biome))
                    {
                        RandomBiomeCreation(region.startPos.x, region.startPos.y, region.biome, region.extension);
                    }
                }
            }
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
                List<Vector2Int> positions = new List<Vector2Int>(); // Lista de posiciones para la extensi�n

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

        private List<Vector2Int> GetValidDirections(Vector2Int pos, string biome)
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

                //neighborPos = UpdatePosToMap(neighborPos);
                if (neighborPos.x < width && neighborPos.x >= 0 && neighborPos.y < height && neighborPos.y >= 0)
                {
                    if (_gridCells[neighborPos.x, neighborPos.y].biomeType != biome)
                    {
                        directions.Add(offset);
                    }
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
                List<Vector2Int> posiblePositions = new List<Vector2Int>(); // Lista de posiciones para la extensi�n

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

                    if (UnityEngine.Random.value < 0.05f)
                    {
                        _gridCells[newPos.x, newPos.y].AddAnimationTile();
                    }

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

        public void RespawnSpawners()
        {
            foreach (var spawner in _enemiesSpawnersList)
                spawner.canSpawn = true;
            foreach (var spawner in _resourcesSpawnersList)
                spawner.canSpawn = true;
        }
        
        public void LockCell(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
                _gridCells[x, y].LockCell();
        }

        public bool IsCellLocked(int x, int y)
        {
            if (x >= 0 && x < width && y >= 0 && y < height)
                return _gridCells[x, y].IsLocked();
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

        public bool IsCellAreaLocked(int x, int y, Vector2 dimensions)
        {
            for (int i = 0; i < dimensions.x; i++)
            {
                for (int j = 0; j < dimensions.y; j++)
                {
                    if (IsCellLocked(x + i, y + j))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsCellAreaSpaceshipArea(int x, int y, Vector2 dimensions)
        {
            for (int i = 0; i < dimensions.x; i++)
            {
                for (int j = 0; j < dimensions.y; j++)
                {
                    if (IsCellSpaceshipArea(x + i, y + j))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion
    }
}