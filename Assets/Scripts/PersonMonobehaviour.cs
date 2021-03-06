using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PersonMonobehaviour : MonoBehaviour
{
    public PersonModel Model { get; protected set; }

    private bool needsUpdate = true;

    void Update()
    {
        if (needsUpdate)
        {
            transform.position = Model.Position;
            needsUpdate = false;
        }
    }

    public void SetNeedsUpdate() { needsUpdate = true; }

    public void SetModel(PersonModel model)
    {
        Model = model;
        Model.Monobehaviour = this;
    }
}
