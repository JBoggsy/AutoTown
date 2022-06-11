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
        CreatePerson(new Vector3Int(Width / 2, Height / 2, 0));
    }

    private void GenerateTerrain(int seed)
    {
        Random.InitState(seed);
        mapData_Terrain = new TerrainType[Height, Width];
        float island_radius = Mathf.Min(Height / 2, Width / 2);
        float clearing_altitude = 0.5f;
        int min_resources = 100;
        int max_resources = 1000;
        float scale = 1.0f;
        float resouce_scarcity = 0.6f;

        List<(float freq, float mag)> fourier_series_topogrophy = new List<(float, float)>()
        {
            (0.100f * scale, 0.1f),
            (0.050f * scale, 0.2f),
            (0.010f * scale, 0.4f),
            (0.005f * scale, 0.2f)
        };

        List<(float freq, float mag)> fourier_series_resources = new List<(float, float)>()
        {
            (0.20f * scale, 0.1f),
            (0.10f * scale, 0.2f),
            (0.05f * scale, 0.3f)
        };

        float offset_topogrophy = Random.Range(0f, 10000f);
        float offset_trees = Random.Range(0f, 10000f);
        float offset_rocks = Random.Range(0f, 10000f);

        float altitude_ocean = 0.35f;
        float altitude_water = 0.40f;
        float altitude_grass = 0.65f;
        float altitude_rocks = 1.00f;

        AnimationCurve island_dropoff = new AnimationCurve();
        island_dropoff.AddKey(new Keyframe(0.0f, altitude_rocks));
        island_dropoff.AddKey(new Keyframe(0.7f, altitude_rocks));
        island_dropoff.AddKey(new Keyframe(1.0f, altitude_ocean));

        AnimationCurve center_clearing = new AnimationCurve();
        center_clearing.AddKey(new Keyframe(0.00f, 1.0f));
        center_clearing.AddKey(new Keyframe(0.10f, 1.0f));
        center_clearing.AddKey(new Keyframe(0.20f, 0.0f));
        center_clearing.AddKey(new Keyframe(1.00f, 0.0f));

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                float altitude = Util.PerlinNoiseMultisample(x, y, fourier_series_topogrophy, offset_topogrophy);
                float distance = (new Vector3Int(x - Width / 2, y - Height / 2, 0)).magnitude;
                altitude *= island_dropoff.Evaluate(distance / island_radius);
                float clearing_weight = center_clearing.Evaluate(distance / island_radius);
                altitude = Mathf.Lerp(altitude, clearing_altitude, clearing_weight);

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

                if (terrain != TerrainType.Grass) { continue; }

                float rocks = Util.PerlinNoiseMultisample(x, y, fourier_series_resources, offset_rocks);
                float trees = Util.PerlinNoiseMultisample(x, y, fourier_series_resources, offset_trees);
                rocks = Mathf.Lerp(rocks, 0, clearing_weight);
                trees = Mathf.Lerp(trees, 0, clearing_weight);

                ResourceDepositType type;
                float richness;
                if (rocks > resouce_scarcity)
                {
                    type = ResourceDepositType.Rock;
                    richness = rocks;
                }
                else if (trees > resouce_scarcity)
                {
                    type = ResourceDepositType.Tree;
                    richness = trees;
                }
                else
                {
                    continue;
                }

                richness = (richness - resouce_scarcity) / (1 - resouce_scarcity);
                int quantity = (int)(richness * (max_resources - min_resources)) + min_resources;

                CreateResourceDeposit(quantity, new Vector3Int(y, x, 0), type);
            }
        }
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