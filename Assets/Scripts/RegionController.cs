using System;

public class RegionController
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
    public RegionController(int width, int height) { 
        Width = width; 
        Height = height; 
        MapData_Terrain = new TerrainType[Height,Width];

        Array terrainTypes = Enum.GetValues(typeof(TerrainType));
        Random rng = new Random();
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                MapData_Terrain[i,j] = (TerrainType)terrainTypes.GetValue(rng.Next(terrainTypes.Length));
            }
        }
    }
}
