using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMonobehaviour : WorldEntityMonoBehaviour
{
    public RockModel Model { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Model.Position;
    }

    public void SetModel(RockModel model)
    { 
        Model = model; 
        Model.Monobehaviour = this;
    }
}
