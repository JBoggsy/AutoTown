using System;
using System.Collections.Generic;
using UnityEngine;

public class RegionModel
{
    // PUBLIC VARS
    public int Height { get; private set; }
    public int Width { get; private set; }

    // PRIVATE VARS
    private TownSceneManager Manager;

    private TerrainType[,] MapData_Terrain;

    private int NextResourceDepositID = 0;
    private Dictionary<int, IResourceDepositModel> ResourceDepositMap = new Dictionary<int, IResourceDepositModel>();


    // PUBLIC METHODS
    public RegionModel(int width, int height) {
        Manager = TownSceneManager.Instance;

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
        CreateResourceDeposit(500, new Vector3Int(11, 11, 0), ResourceDepositType.Tree);
        CreateResourceDeposit(500, new Vector3Int(10, 10, 0), ResourceDepositType.Rock);
    }


    public IResourceDepositModel CreateResourceDeposit(int amount, Vector3Int Position, ResourceDepositType type)
    {
        IResourceDepositModel newResourceDeposit;
        switch (type)
        {
            case ResourceDepositType.Tree:
                newResourceDeposit = new TreeModel(amount, Position.x, Position.y);
                break;
            case ResourceDepositType.Rock:
                newResourceDeposit = new RockModel(amount, Position.x, Position.y);
                break;
            default:
                return null;
        }
        ResourceDepositMap.Add(NextResourceDepositID, newResourceDeposit);
        Manager.SpawnResourceDeposit(newResourceDeposit, NextResourceDepositID);
        NextResourceDepositID++;
        return newResourceDeposit;
    }

    public TerrainType GetTerrainAt(int y, int x)
    {
        return MapData_Terrain[y, x];
    }
}
