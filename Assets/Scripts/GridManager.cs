using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{/*
    [SerializeField] public int width, height;
    [SerializeField] private Tile tilePrefab;

    private Dictionary<Vector2, Tile> tiles;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateGrid()
    {
        tiles = new Dictionary<Vector2, Tile>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector3 pos = new(i, j);
                Tile spawnedTile = Instantiate(tilePrefab, pos, Quaternion.identity);
                spawnedTile.name = "Tile " + i + "-" + j;

                bool isOffset = (i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0);
                spawnedTile.Init(isOffset);

                tiles[new Vector2(i, j)] = spawnedTile;
            }
        }
    }

    public Tile GetTileAtPos(Vector2 pos)
    {
        if (tiles.TryGetValue(pos, out Tile tile))
        {
            return tile;
        }
        else
        {
            return null;
        }
    }*/
}
