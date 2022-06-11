//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RegionModel
{
    /////////////////
    // PROPERTIES //
    ///////////////

    // UNITY ACCESSORS
    //////////////////
    
    private TownSceneManager manager;

    // SIMULATION PROPERTIES
    ////////////////////////
    
    private bool runningSimulation;

    // MAP DATA
    ///////////
        
    public int Height { get; private set; }
    public int Width { get; private set; }

    // Terrain layer
    private TerrainType[,] mapData_Terrain;

    // Resources layer
    private int nextResourceDepositID = 0;
    private Dictionary<int, IResourceDeposit> resourceDepositMap = new Dictionary<int, IResourceDeposit>();

    // Persons layer
    private int nextPersonID = 0;
    private Dictionary<int, PersonModel> personMap = new Dictionary<int,PersonModel>();


    //////////////
    // METHODS //
    ////////////

    // CONSTRUCTOR
    //////////////

    /// <summary>
    /// Create a new world region with the given width and height.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public RegionModel(int width, int height) {
        manager = TownSceneManager.Instance;
        runningSimulation = false;

        Width = width; 
        Height = height;
        
        int seed = (int)(Random.value * int.MaxValue);
        GenerateTerrain(seed);
        CreatePerson(new Vector3Int(8, 8, 0));
    }

    private void GenerateTerrain(int Seed)
    {
        Random.InitState(Seed);
        mapData_Terrain = new TerrainType[Height, Width];

        List<(float freq, float mag)> fourier_series = new List<(float, float)>()
        {
            (0.1f, 0.1f),
            (0.05f, 0.2f),
            (0.01f, 0.3f)
        };

        float noise_offset = Random.Range(0f, 10000f);
        float altitude_ocean = 0.35f;
        float altitude_water = 0.40f;
        float altitude_grass = 0.65f;

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                float altitude = Util.PerlinNoiseMultisample(x, y, fourier_series, noise_offset);
                float distance = (new Vector3Int(x - Width / 2, y - Height / 2, 0)).magnitude;

                /*
                if (distance < 5)
                {
                    mapData_Terrain[y, x] = TerrainType.Grass;
                }
                else
                {
                    mapData_Terrain[y, x] = TerrainType.Water_Shallow;
                }
                */
                TerrainType terrain;
                if (altitude < altitude_ocean)
                {
                    terrain = TerrainType.Water_Deep;
                }
                else if (altitude < altitude_water)
                {
                    terrain = TerrainType.Water_Shallow;
                }
                else if (altitude < altitude_grass)
                {
                    terrain = TerrainType.Grass;
                }
                else
                {
                    terrain = TerrainType.Rock;
                }

                mapData_Terrain[x, y] = terrain;
            }
        }
        CreateResourceDeposit(500, new Vector3Int(11, 11, 0), ResourceDepositType.Tree);
        CreateResourceDeposit(500, new Vector3Int(10, 10, 0), ResourceDepositType.Rock);
    }

    // ENTITY CREATION METHODS
    //////////////////////////

    /// <summary>
    /// Create a new Person entity.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public PersonModel CreatePerson(Vector3Int position)
    {
        PersonModel newPerson = new PersonModel(position.x, position.y);
        personMap.Add(nextPersonID, newPerson);
        manager.SpawnPerson(newPerson, nextPersonID);
        nextPersonID++;
        return newPerson;
    }

    /// <summary>
    /// Create a new resource deposit entity.
    /// </summary>
    /// <details>
    /// Resource deposits are one of:
    /// <ul>
    ///     <li>Rock</li>
    ///     <li>Tree</li>
    /// </ul>
    /// </details>
    /// <param name="amount">Amount of resources held in the deposit.</param>
    /// <param name="position">The X,Y coordinate of the deposit (Z should always be 0).</param>
    /// <param name="type">The type of the deposit.</param>
    /// <returns></returns>
    public IResourceDeposit CreateResourceDeposit(int amount, Vector3Int position, ResourceDepositType type)
    {
        IResourceDeposit newResourceDeposit;
        switch (type)
        {
            case ResourceDepositType.Tree:
                newResourceDeposit = new TreeModel(amount, position.x, position.y);
                break;
            case ResourceDepositType.Rock:
                newResourceDeposit = new RockModel(amount, position.x, position.y);
                break;
            default:
                return null;
        }
        resourceDepositMap.Add(nextResourceDepositID, newResourceDeposit);
        manager.SpawnResourceDeposit(newResourceDeposit, nextResourceDepositID);
        nextResourceDepositID++;
        return newResourceDeposit;
    }


    // STATE RETRIEVAL METHODS
    //////////////////////////

    public TerrainType GetTerrainAt(int y, int x)
    {
        return mapData_Terrain[y, x];
    }


    // SIMULATION
    /////////////

    public void SimulateOneStep ()
    {
        foreach (PersonModel person in personMap.Values)
        {
            person.Simulate();
        }
    }
}