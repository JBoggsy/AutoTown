using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMonobehaviour : WorldEntityMonoBehaviour
{
    public RockModel Model { get; private set; }

    private bool needsUpdate = true;

    void Update()
    {
        if (needsUpdate)
        {
            transform.position = Model.Position;
            needsUpdate = false;
        }
    }

    public void SetModel(RockModel model)
    { 
        Model = model; 
        Model.Monobehaviour = this;
    }
}
