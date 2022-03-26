using System.Collections.Generic;
using UnityEngine;

public class TownSceneManager : MonoBehaviour
{
    public static TownSceneManager Instance;
    public RegionModel Region { get; private set; }

    public GameObject TreePrefab;
    public GameObject RockPrefab;

    public TownUIManager TownUIManager;

    // PRIVATE VARS
    private Dictionary <int, GameObject> ResourceDepositGameObjectsMap = new Dictionary <int, GameObject>();

    // PUBLIC METHODS

    public void Awake()
    {
        Instance = this;
        Region = new RegionModel(20, 20);
    }

    public void SpawnResourceDeposit(IResourceDepositModel deposit, int ID)
    {
        GameObject newResourceDepositGameObject;
        switch (deposit.Type)
        {
            case ResourceDepositType.Tree:
                newResourceDepositGameObject = GameObject.Instantiate(TreePrefab);
                ResourceDepositGameObjectsMap.Add(ID, newResourceDepositGameObject);
                newResourceDepositGameObject.GetComponent<Transform>().SetPositionAndRotation(deposit.Position, new Quaternion(0, 0, 0, 0));
                newResourceDepositGameObject.GetComponent<TreeMonobehaviour>().SetModel((TreeModel)deposit);
                break;
            case ResourceDepositType.Rock:
                newResourceDepositGameObject = GameObject.Instantiate(RockPrefab);
                ResourceDepositGameObjectsMap.Add(ID, newResourceDepositGameObject);
                newResourceDepositGameObject.GetComponent<Transform>().SetPositionAndRotation(deposit.Position, new Quaternion(0, 0, 0, 0));
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
