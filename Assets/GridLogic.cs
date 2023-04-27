using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Cell
{
    public int x;
    public int y;
    public string biomeType;
    public bool occuped;

    public Cell(int x, int y)
    {
        this.x = x;
        this.y = y;
        biomeType = null;
        occuped = false;
    }

    public Cell(int x, int y, string biomeType)
    {
        this.x = x;
        this.y = y;
        this.biomeType = biomeType;
        occuped = false;
    }
}


public class GridLogic : MonoBehaviour
{
    public int width;
    public int height;
    public Vector3 cellSize = new Vector3(1,1,0);
    public enum FillMode
    {
        None,
        Single_All,
        Multiple_All,
        Single_Random,
        Multiple_Random
    }

    [Serializable]

    public struct BiomeRuleTile
    {
        public string biome;
        public RuleTile tiles;
    }


    [Serializable]
    public struct TilemapLayer
    {
        public string name;
        public Tilemap tilemap;
        public List<BiomeRuleTile> biomesTiles;
        public FillMode fillMode;
    }

    public List<string> biomes;

    public TilemapLayer[] tilemapLayers;

    private Cell[,] _gridCells;
    private Grid _grid;

    private void Awake()
    {
        _grid = GetComponent<Grid>();
        _grid.cellSize = cellSize;
    }

    private void OnValidate()
    {
        for (int i = 0; i < biomes.Count; i++)
        {
            // Verificar si el bioma ya existe en la lista de biomas con tiles de reglas
            foreach (var tl in tilemapLayers)
            {
                bool existe = false;
                for (int j = 0; j < tl.biomesTiles.Count; j++)
                {
                    if (tl.biomesTiles[j].biome == biomes[i])
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
                    tl.biomesTiles.Add(newBiome);
                }
            }
        }


        foreach (var tl in tilemapLayers)
        {
            for (int i = tl.biomesTiles.Count - 1; i >= 0; i--) // Recorrer la lista de biomas con tiles de reglas
            {
                if (!biomes.Contains(tl.biomesTiles[i].biome)) // Verificar si el bioma ya no est� en la lista de biomas
                {
                    tl.biomesTiles.RemoveAt(i); // Eliminar el elemento de biomasConTilesDeReglas
                }
            }
        }
        
    }

    private void Start()
    {
        InitializeGrid();
        CreateRegions();

        foreach (var tl in tilemapLayers)
        {
            FillTilemap(tl.tilemap, tl.biomesTiles, tl.fillMode);
        }
    }

    public void FillTilemap(Tilemap tilemap, List<BiomeRuleTile> tiles , FillMode fillMode)
    {
        switch (fillMode)
        {
            case FillMode.None:
                break;
            case FillMode.Single_All:
                FillTilemapSingleAll(tilemap, tiles);
                break;
            case FillMode.Multiple_All:
                break;
            case FillMode.Single_Random:
                FillTilemapSingleRandom(tilemap, tiles);
                break;
            case FillMode.Multiple_Random:
                break;
        }
    }

    public void FillTilemapSingleAll(Tilemap tilemap, List<BiomeRuleTile> tiles)
    {
        for (int x = 0; x < Mathf.RoundToInt(_grid.cellSize.x) * width; x++)
        {
            for (int y = 0; y < Mathf.RoundToInt(_grid.cellSize.y) * height; y++)
            {
                // Obtener la posici�n de la celda en el grid
                Vector3Int position = new Vector3Int(x, y, 0);

                int biomeIndex = tiles.FindIndex(tiles => tiles.biome == _gridCells[x, y].biomeType);

                // Asignar el sprite al Tilemap en la posici�n correspondiente
                tilemap.SetTile(position, tiles[biomeIndex].tiles);
            }
        }
    }

    public void FillTilemapSingleRandom(Tilemap tilemap, List<BiomeRuleTile> tiles)
    {
        for (int x = 0; x < Mathf.RoundToInt(_grid.cellSize.x) * width; x++)
        {
            for (int y = 0; y < Mathf.RoundToInt(_grid.cellSize.y) * height; y++)
            {
                // Obtener la posici�n de la celda en el grid
                Vector3Int position = new Vector3Int(x, y, 0);

                
                int biomeIndex = tiles.FindIndex(tiles => tiles.biome == _gridCells[x, y].biomeType);

                // Asignar el sprite al Tilemap en la posici�n correspondiente
                if(UnityEngine.Random.value < 0.4f )
                    tilemap.SetTile(position, tiles[biomeIndex].tiles);
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

        RandomBiomeCreation(10, 40, biomes[1], 400, 450);

        /*// Generar biomas aleatoriamente
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (_gridCells[i, j].biomeType != null) continue; // Saltar si la casilla ya tiene un bioma asignado

                string biome = biomes[UnityEngine.Random.Range(0, biomes.Count)]; // Seleccionar bioma aleatorio
                int tamRegion = 0; // Tama�o actual de la regi�n
                RellenaRegion(i, j, biome, ref tamRegion); // Rellenar regi�n a partir de la casilla actual
            }
        }*/
    }

    private void RellenaRegion(int x, int y, string bioma, ref int tamRegion)
    {
        if (x < 0 || x >= width || y < 0 || y >= height) return; // Verificar l�mites de la matriz
        if (_gridCells[x, y].biomeType != null || tamRegion >= 10) return; // Verificar si la casilla ya tiene un bioma asignado o si se alcanz� el tama�o m�ximo de regi�n

        _gridCells[x, y].biomeType = bioma; // Asignar bioma a la casilla actual
        tamRegion++; // Incrementar tama�o de la regi�n

        // Rellenar regiones adyacentes recursivamente
        RellenaRegion(x - 1, y, bioma, ref tamRegion);
        RellenaRegion(x + 1, y, bioma, ref tamRegion);
        RellenaRegion(x, y - 1, bioma, ref tamRegion);
        RellenaRegion(x, y + 1, bioma, ref tamRegion);
    }

    private void RandomBiomeCreation(int x, int y, string biome, int minExtension, int maxExtension)
    {
        if(x >= 0 && x < width && y >= 0 && y < height)
        {
            HashSet<Vector2Int> visited = new HashSet<Vector2Int>(); // conjunto de casillas visitadas
            int extension = UnityEngine.Random.Range(minExtension, maxExtension + 1); // determinar la extension aleatoria
            for (int i = 0; i < extension; i++)
            {
                Vector2Int currentPos = new Vector2Int(x, y); // posici�n actual

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
                                currentPos.y = height -1;
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

}


