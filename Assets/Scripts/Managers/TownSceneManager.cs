using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownSceneManager : MonoBehaviour
{
    public static TownSceneManager Instance;
    public RegionModel Region { get; private set; }

    public GameObject TreePrefab;
    public GameObject RockPrefab;
    public GameObject PersonPrefab;
    public GameObject TownCenterPrefab;

    public TownUIManager TownUIManager;

    public float SimulationStepTime = 0.1f;

    private Dictionary<int, GameObject> ResourceDepositGameObjectsLookup = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> PersonGameObjectsLookup = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> BuildingGameObjectsLookup = new Dictionary<int, GameObject>();

    private IEnumerator simulationCoroutine;
    private bool runSimulation = true;

    private WorldEntityMonoBehaviour SelectedEntity;
    private bool HasSelectedEntity { get { return SelectedEntity != null; } }


    public void Awake()
    {
        Instance = this;
        Region = new RegionModel(200, 200);
        simulationCoroutine = Simulate();
    }

    public void Start ()
    {
        StartCoroutine(simulationCoroutine);
    }

    public void Update()
    {
        // LEFT CLICK
        if (Input.GetMouseButtonDown(0))
        {
            ClearSelection();

            WorldEntityMonoBehaviour entity = Util.GetObjectUnderCursor<WorldEntityMonoBehaviour>();
            if (entity != null)
            {
                Input_Select(entity);
            }
        }

        // RIGHT CLICK
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 input = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int coords = new Vector2Int(Mathf.FloorToInt(input.x - 0.5f), Mathf.FloorToInt(input.y - 0.5f));
            Input_Command(coords);
        }
    }

    // Handle a 'Select' type input from the user (left click)
    private void Input_Select(WorldEntityMonoBehaviour entity)
    {
        SelectedEntity = entity;
        TownUIManager.ShowPopup(SelectedEntity.GetPopupText(), SelectedEntity.transform);
    }

    // Handle a 'Command' type input from the user (right click)
    private void Input_Command(Vector2Int coords)
    {
        if (HasSelectedEntity
            && SelectedEntity is PersonMonobehaviour person
            && person.Model is PersonEntity entity
            && entity.AgentController is UserInputACI controller)
        {
            controller.HandleUserInput(coords);
        }
    }

    private void ClearSelection()
    {
        SelectedEntity = null;
        TownUIManager.ClearPopup();
    }

    public void SpawnPerson(PersonEntity person, int ID)
    {
        GameObject newPersonGameObject = GameObject.Instantiate(PersonPrefab);
        PersonGameObjectsLookup.Add(ID, newPersonGameObject);
        newPersonGameObject.GetComponent<PersonMonobehaviour>().Initialize(person);
    }

    public void SpawnResourceDeposit(IResourceDepositEntity deposit, int ID)
    {
        GameObject newResourceDepositGameObject;
        switch (deposit.Type)
        {
            case ResourceDepositType.Tree:
                newResourceDepositGameObject = GameObject.Instantiate(TreePrefab);
                ResourceDepositGameObjectsLookup.Add(ID, newResourceDepositGameObject);
                newResourceDepositGameObject.GetComponent<TreeMonobehaviour>().Initialize((TreeEntity)deposit);
                break;
            case ResourceDepositType.Rock:
                newResourceDepositGameObject = GameObject.Instantiate(RockPrefab);
                ResourceDepositGameObjectsLookup.Add(ID, newResourceDepositGameObject);
                newResourceDepositGameObject.GetComponent<RockMonobehaviour>().Initialize((RockEntity)deposit);
                break;
        }
    }

    public void SpawnBuilding(BuildingEntity building, int ID)
    {
        GameObject newBuildingGameObject;
        switch (building.buildingType)
        {
            case BuildingType.Town_Center:
                newBuildingGameObject = GameObject.Instantiate(TownCenterPrefab);
                BuildingGameObjectsLookup.Add(ID, newBuildingGameObject);
                newBuildingGameObject.GetComponent<TownCenterMonobehaviour>().SetModel((TownCenterEntity)building);
                break;
        }
    }

    private IEnumerator Simulate ()
    {
        while (true)
        {
            if (runSimulation)
            {
                Region.SimulateOneStep();
            }
            yield return new WaitForSeconds(SimulationStepTime);
        }
    }
}
