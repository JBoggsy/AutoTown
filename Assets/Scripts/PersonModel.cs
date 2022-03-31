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
        Position += movement;
        return true;
    }

    public void Simulate ()
    {
        if (Position.x == 8)
        {
            if (Position.y < 12)
            {
                Move(Direction.North);
            } else
            {
                Move(Direction.East);
            }
        } else if (Position.x == 9)
        {
            if (Position.y > 8)
            {
                Move(Direction.South);
            } else
            {
                Move(Direction.West);
            }
        }
    }
}