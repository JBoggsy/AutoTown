using System;
using System.Collections.Generic;
using UnityEngine;

public class PersonModel : IWorldEntityModel
{
    // PUBLIC VARS
    public Vector3Int Position { get; protected set; }
    public PersonMonobehaviour Monobehaviour { protected get; set; }
}