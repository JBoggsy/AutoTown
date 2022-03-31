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

    public TownUIManager TownUIManager;

    public float SimulationStepTime = 0.1f;

    private Dictionary<int, GameObject> ResourceDepositGameObjectsMap = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> PersonGameObjectsMap = new Dictionary<int, GameObject>();
    
    private IEnumerator simulationCoroutine;
    private bool runSimulation = true;


    public void Awake()
    {
        Instance = this;
        Region = new RegionModel(20, 20);
        simulationCoroutine = Simulate();
    }

    public void Start ()
    {
        StartCoroutine(simulationCoroutine);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bool clicked_on_object = false;

            GameObject game_object = Util.GetObjectUnderCursor();
            if (game_object != null)
            {
                MonoBehaviour mono_behaviour = game_object.GetComponent<MonoBehaviour>();
                if (mono_behaviour != null && mono_behaviour is WorldEntityMonoBehaviour)
                {
                    clicked_on_object = true;
                    string label = "";
                    if (mono_behaviour is TreeMonobehaviour tree)
                    {
                        label = "Tree (" + tree.Model.AmountRemaining + ")";
                    }
                    if (mono_behaviour is RockMonobehaviour rock)
                    {
                        label = "Rock (" + rock.Model.AmountRemaining + ")";
                    }
                    TownUIManager.ShowPopup(label, mono_behaviour.transform.position);
                }
            }

            if (!clicked_on_object)
            {
                TownUIManager.ClearPopup();
            }
        }
    }

    public void SpawnPerson (PersonModel person, int ID)
    {
        GameObject newPersonGameObject = GameObject.Instantiate(PersonPrefab);
        PersonGameObjectsMap.Add(ID, newPersonGameObject);
        newPersonGameObject.GetComponent<PersonMonobehaviour>().SetModel(person);
    }

    public void SpawnResourceDeposit (IResourceDeposit deposit, int ID)
    {
        GameObject newResourceDepositGameObject;
        switch (deposit.Type)
        {
            case ResourceDepositType.Tree:
                newResourceDepositGameObject = GameObject.Instantiate(TreePrefab);
                ResourceDepositGameObjectsMap.Add(ID, newResourceDepositGameObject);
                newResourceDepositGameObject.GetComponent<TreeMonobehaviour>().SetModel((TreeModel)deposit);
                break;
            case ResourceDepositType.Rock:
                newResourceDepositGameObject = GameObject.Instantiate(RockPrefab);
                ResourceDepositGameObjectsMap.Add(ID, newResourceDepositGameObject);
                newResourceDepositGameObject.GetComponent<RockMonobehaviour>().SetModel((RockModel)deposit);
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
