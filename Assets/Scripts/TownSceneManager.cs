using System.Collections.Generic;
using UnityEngine;

public class TownSceneManager : MonoBehaviour
{
    public static TownSceneManager Instance;
    public RegionModel Region { get; private set; }

    public GameObject TreePrefab;
    public GameObject RockPrefab;
    public GameObject PersonPrefab;

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

    public void OnMouseDown(WorldEntityMonoBehaviour entity)
    {
        if (entity is RockMonobehaviour rock)
        {
            Debug.Log("You clicked on a rock");
        }
        else if (entity is TreeMonobehaviour tree)
        {
            Debug.Log("You clicked on a tree");
        }
    }
}
