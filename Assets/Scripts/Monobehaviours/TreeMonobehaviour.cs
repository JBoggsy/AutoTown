using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMonobehaviour : WorldEntityMonoBehaviour
{
    public TreeEntity Model { get; private set; }

    private bool needsUpdate = true;

    void Update()
    {
        if (needsUpdate)
        {
            transform.position = Model.Position;
            needsUpdate = false;
        }
    }

    public void SetModel(TreeEntity model)
    { 
        Model = model; 
        Model.Monobehaviour = this;
    }
}
