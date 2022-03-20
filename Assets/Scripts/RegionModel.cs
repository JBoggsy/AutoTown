using System;
using UnityEngine;

public class RegionModel
{
    public enum TerrainType
    {
        Dirt,
        Grass,
        Rock,
        Water_Shallow,
        Water_Deep,
        Ice
    }

    // PUBLIC VARS
    public int Height { get; private set; }
    public int Width { get; private set; }

    // PRIVATE VARS
    private TerrainType[,] MapData_Terrain;

    // PUBLIC METHODS
    public RegionModel(int width, int height) { 
        Width = width; 
        Height = height;
        MapData_Terrain = new TerrainType[Height,Width];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int location = new Vector3Int(x - width / 2, y - height / 2, 0);

                if (location.magnitude < 5)
                {
                    MapData_Terrain[y, x] = TerrainType.Grass;
                }
                else
                {
                    MapData_Terrain[y, x] = TerrainType.Water_Shallow;
                }
            }
        }
    }

    public TerrainType GetTerrainAt(int y, int x)
    {
        return MapData_Terrain[y, x];
    }
}
