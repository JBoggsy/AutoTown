using UnityEngine;


public class TownCenterMonobehaviour : MonoBehaviour
{
    public TownCenterModel Model { get; protected set; }

    private bool needsUpdate = true;

    void Update()
    {
        if (needsUpdate)
        {
            transform.position = Model.Position;
            needsUpdate = false;
        }
    }

    public void SetModel(TownCenterModel model)
    {
        Model = model;
        Model.Monobehaviour = this;
    }
}
