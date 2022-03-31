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

    // PRIVATE VARS
    private Dictionary<int, GameObject> ResourceDepositGameObjectsMap = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> PersonGameObjectsMap = new Dictionary<int, GameObject>();

    // PUBLIC METHODS

    public void Awake()
    {
        Instance = this;
        Region = new RegionModel(20, 20);
    }

    public void SpawnPerson(PersonModel person, int ID)
    {
        GameObject newPersonGameObject = GameObject.Instantiate(PersonPrefab);
        PersonGameObjectsMap.Add(ID, newPersonGameObject);
        newPersonGameObject.GetComponent<PersonMonobehaviour>().SetModel(person);
    }

    public void SpawnResourceDeposit(IResourceDepositModel deposit, int ID)
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
}
