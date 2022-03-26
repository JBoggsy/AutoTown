using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonMonobehaviour : MonoBehaviour
{
    public PersonModel Model { get; protected set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Model.Position;
    }

    public void SetModel(PersonModel model)
    {
        Model = model;
        Model.Monobehaviour = this;
    }
}
