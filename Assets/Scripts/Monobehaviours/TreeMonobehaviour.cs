using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeMonobehaviour : WorldEntityMonoBehaviour
{
    private bool needsUpdate = false;

    void Update()
    {
        if (needsUpdate)
        {
            transform.position = Model.Position;
            needsUpdate = false;
        }
    }

    public void Initialize(WorldEntity entity)
    {
        base.Initialize(entity);
        needsUpdate = true;
    }

    public override string GetPopupText()
    {
        return string.Format("Tree ({0})", ((TreeEntity)Model).AmountRemaining);
    }
}
