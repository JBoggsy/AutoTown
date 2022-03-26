using System.Collections.Generic;
using UnityEngine;

public class TownSceneManager : MonoBehaviour
{
    public static TownSceneManager Instance;
    public RegionModel Region { get; private set; }

    public GameObject TreePrefab;
    public GameObject RockPrefab;

    // PRIVATE VARS
    private Dictionary <int, GameObject> ResourceDepositGameObjectsMap = new Dictionary <int, GameObject>();

    // PUBLIC METHODS

    public void Awake()
    {
        Instance = this;
        Region = new RegionModel(20, 20);
    }

    public void SpawnResourceDeposit(IResourceDeposit deposit, int ID)
    {
        GameObject newResourceDepositGameObject;
        switch (deposit.Type)
        {
            case RegionModel.ResourceDepositType.Tree:
                newResourceDepositGameObject = GameObject.Instantiate(TreePrefab);
                ResourceDepositGameObjectsMap.Add(ID, newResourceDepositGameObject);
                newResourceDepositGameObject.GetComponent<Transform>().SetPositionAndRotation(deposit.Position, new Quaternion(0, 0, 0, 0));
                newResourceDepositGameObject.GetComponent<TreeMonobehaviour>().SetModel((TreeModel)deposit);
                break;
            case RegionModel.ResourceDepositType.Rock:
                newResourceDepositGameObject = GameObject.Instantiate(RockPrefab);
                ResourceDepositGameObjectsMap.Add(ID, newResourceDepositGameObject);
                newResourceDepositGameObject.GetComponent<Transform>().SetPositionAndRotation(deposit.Position, new Quaternion(0, 0, 0, 0));
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
