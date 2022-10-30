using UnityEngine;


public class TownCenterMonobehaviour : MonoBehaviour
{
    public TownCenterEntity Model { get; protected set; }

    private bool needsUpdate = true;

    void Update()
    {
        if (needsUpdate)
        {
            transform.position = Model.Position;
            needsUpdate = false;
        }
    }

    public void SetModel(TownCenterEntity model)
    {
        Model = model;
        Model.Monobehaviour = this;
    }
}
