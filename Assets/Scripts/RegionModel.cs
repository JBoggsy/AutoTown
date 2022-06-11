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
        
        GenerateTerrain(seed : (int)(Random.value * int.MaxValue));
        CreatePerson(new Vector3Int(Width / 2, Height / 2, 0));
    }

    private void GenerateTerrain(int seed)
    {
        WorldGen.Create(Width, Height, seed, out mapData_Terrain, out List<WorldGen.ResourceDeposit> deposits);

        foreach (WorldGen.ResourceDeposit deposit in deposits)
        {
            CreateResourceDeposit(deposit.Quantity, (Vector3Int)deposit.Location, deposit.Type);
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