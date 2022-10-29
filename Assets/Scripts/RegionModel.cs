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
    private Dictionary<int, IResourceDeposit> resourceDepositLookup = new Dictionary<int, IResourceDeposit>();

    // Persons layer
    private int nextPersonID = 0;
    private Dictionary<int, PersonModel> personLookup = new Dictionary<int,PersonModel>();

    // Buildings layer
    private int nextBuildingID = 0;
    private Dictionary<int, BuildingModel> buildingLookup = new Dictionary<int,BuildingModel>();

    // Passability layer
    private Dictionary<Vector3Int, bool> passabilityLookup = new Dictionary<Vector3Int, bool>();


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
        
        GenerateTerrain(seed : (int)(Random.value * int.MaxValue));
        CreatePerson(new Vector3Int(Width / 2, Height / 2, 0));
    }

    private void GenerateTerrain(int seed)
    {
        WorldGen.Parameters parameters = WorldGen.Parameters.Default();
        parameters.Width = Width;
        parameters.Height = Height;

        WorldGen.Create(parameters, seed, out mapData_Terrain, out List<WorldGen.ResourceDeposit> deposits);

        foreach (WorldGen.ResourceDeposit deposit in deposits)
        {
            CreateResourceDeposit(deposit.Quantity, (Vector3Int)deposit.Location, deposit.Type);
        }
        CreateResourceDeposit(500, new Vector3Int(11, 11, 0), ResourceDepositType.Tree);
        CreateResourceDeposit(500, new Vector3Int(10, 10, 0), ResourceDepositType.Rock);
        CreatePerson(new Vector3Int(8, 8, 0));
        CreateBuilding(new Vector3Int(12, 10, 0), BuildingType.Town_Center);
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
        personLookup.Add(nextPersonID, newPerson);
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
        resourceDepositLookup.Add(nextResourceDepositID, newResourceDeposit);
        passabilityLookup[position] = false;
        manager.SpawnResourceDeposit(newResourceDeposit, nextResourceDepositID);
        nextResourceDepositID++;
        return newResourceDeposit;
    }

    public BuildingModel CreateBuilding(Vector3Int position, BuildingType type)
    {
        BuildingModel newBuilding;
        switch (type)
        {
            case BuildingType.Town_Center:
                newBuilding = new TownCenterModel(position.x, position.y);
                break;
            default:
                return null;
        }
        buildingLookup.Add(nextBuildingID, newBuilding);
        passabilityLookup[position] = false;
        manager.SpawnBuilding(newBuilding, nextBuildingID);
        nextBuildingID++;
        return newBuilding;
    }

    // STATE RETRIEVAL METHODS
    //////////////////////////

    /// <summary>
    /// Get the terrain type at the give position.
    /// </summary>
    /// <param name="position">A Vector3Int representing the position to query.</param>
    /// <returns>The `TerrainType` of the given position.</returns>
    public TerrainType GetTerrainAt(Vector3Int position)
    {
        return mapData_Terrain[position.y, position.x];
    }
    
    /// <summary>
    /// Indicate whether the specified position is passable or not.
    /// </summary>
    public bool IsPassable(Vector3Int position)
    {
        if (!passabilityLookup.ContainsKey(position))
        {
            passabilityLookup.Add(position, true);
        }
        return passabilityLookup[position];
    }

    // SIMULATION
    /////////////

    public void SimulateOneStep ()
    {
        foreach (PersonModel person in personLookup.Values)
        {
            person.Simulate();
        }
    }
}