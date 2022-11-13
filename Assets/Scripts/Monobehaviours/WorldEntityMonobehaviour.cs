using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;

public abstract class WorldEntityMonoBehaviour : MonoBehaviour
{
    public WorldEntity Model;

    public void Initialize(WorldEntity entity)
    {
        Model = entity;
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }
    public virtual string GetPopupText()
    {
        return "This is a WorldEntityMonoBehaviour";
    }
}
