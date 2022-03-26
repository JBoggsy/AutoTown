using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMonobehaviour : WorldEntityMonoBehaviour
{
    public TreeModel Model { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetModel(TreeModel model)
    { 
        Model = model; 
        Model.Monobehaviour = this;
    }
}
