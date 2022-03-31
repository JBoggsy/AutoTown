using System;
using System.Collections.Generic;
using UnityEngine;

public class PersonModel : IWorldEntity
{
    public Vector3Int Position { get; protected set; }
    public PersonMonobehaviour Monobehaviour { protected get; set; }

    protected Inventory Inventory { get; set; }


    public PersonModel(int x, int y)
    {
        Position = new Vector3Int(x, y, 0);
        Inventory = new Inventory(20);
    }

    public bool Move(Vector3Int movement)
    {
        Position = Position + movement;
        return true;
    }
}