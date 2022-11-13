//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    public RectInt RegionBox { get; private set; }

    // Terrain layer
    private TerrainType[,] mapData_Terrain;

    // Resources layer
    private int nextResourceDepositID = 0;
    private Dictionary<int, IResourceDepositEntity> resourceDepositLookup = new Dictionary<int, IResourceDepositEntity>();
    private Dictionary<Vector3Int, int> resourceDepositMap = new Dictionary<Vector3Int, int>();

    // Persons layer
    private int nextPersonID = 0;
    private Dictionary<int, PersonEntity> personLookup = new Dictionary<int,PersonEntity>();

    // Buildings layer
    private int nextBuildingID = 0;
    private Dictionary<int, BuildingEntity> buildingLookup = new Dictionary<int,BuildingEntity>();

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
        RegionBox = new RectInt(0, 0, Width, Height);
        
        GenerateTerrain(seed : (int)(UnityEngine.Random.value * int.MaxValue));
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
        //CreateResourceDeposit(500, new Vector3Int(11, 11, 0), ResourceDepositType.Tree);
        //CreateResourceDeposit(500, new Vector3Int(10, 10, 0), ResourceDepositType.Rock);
        //CreatePerson(new Vector3Int(8, 8, 0));
        //CreateBuilding(new Vector3Int(12, 10, 0), BuildingType.Town_Center);
    }

    // ENTITY CREATION METHODS
    //////////////////////////

    /// <summary>
    /// Create a new Person entity.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public PersonEntity CreatePerson(Vector3Int position)
    {
        PersonEntity newPerson = new PersonEntity(position.x, position.y);
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
    public IResourceDepositEntity CreateResourceDeposit(int amount, Vector3Int position, ResourceDepositType type)
    {
        IResourceDepositEntity newResourceDeposit;
        switch (type)
        {
            case ResourceDepositType.Tree:
                newResourceDeposit = new TreeEntity(amount, position.x, position.y);
                break;
            case ResourceDepositType.Rock:
                newResourceDeposit = new RockEntity(amount, position.x, position.y);
                break;
            default:
                return null;
        }
        resourceDepositLookup.Add(nextResourceDepositID, newResourceDeposit);
        resourceDepositMap.Add(position, nextResourceDepositID);
        passabilityLookup[position] = false;
        manager.SpawnResourceDeposit(newResourceDeposit, nextResourceDepositID);
        nextResourceDepositID++;
        return newResourceDeposit;
    }

    public BuildingEntity CreateBuilding(Vector3Int position, BuildingType type)
    {
        BuildingEntity newBuilding;
        switch (type)
        {
            case BuildingType.Town_Center:
                newBuilding = new TownCenterEntity(position.x, position.y);
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

    /// <summary>
    /// Provides the location of the nearest resource to `position` of the given `depositType`.
    /// </summary>
    /// <param name="position">Location from which to find closest resource</param>
    /// <param name="depositType">Type of resource deposit to find</param>
    /// <returns>The location of the closest deposit of theg iven type if one exists, otherwise
    /// (-1, -1, -1).</returns>
    public Vector3Int GetNearestResourceDeposit(Vector3Int position, ResourceDepositType depositType)
    {
        Queue<Vector3Int> fringe = new Queue<Vector3Int>();
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        fringe.Enqueue(position);

        Vector3Int active_position = new Vector3Int(-1, -1, -1);
        bool resource_found = false;
        while (!resource_found && fringe.Count > 0)
        {
            active_position = fringe.Dequeue();
            visited.Add(active_position);

            if (resourceDepositMap.ContainsKey(active_position) && resourceDepositLookup[resourceDepositMap[active_position]].Type == depositType)
            {
                resource_found = true;
            }
            else
            {
                foreach (Vector3Int dir in Geometry.AllDirections)
                {
                    Vector3Int nbor = active_position + dir;
                    if (RegionBox.Contains(new Vector2Int(nbor.x, nbor.y)) && !visited.Contains(nbor))
                    {
                        fringe.Enqueue(nbor);
                        visited.Add(nbor);
                    }
                }
            }
        }
        if (!resource_found)
        {
            active_position = new Vector3Int(-1, -1, -1);
        }
        return active_position;
    }

    // SIMULATION
    /////////////

    public void SimulateOneStep ()
    {
        List<Action> actions = new List<Action>();
        foreach (PersonEntity person in personLookup.Values)
        {
            actions.Add(person.DecideNextAction(this));
        }
        foreach (Action action in actions)
        {
            action.ExecuteAction(this);
        }
        // In the future, this is where action conflicts (e.g., movement collisions)
        // should be resolved by evaluating the world state and ensuring it still complies
        // with the rules
    }
}