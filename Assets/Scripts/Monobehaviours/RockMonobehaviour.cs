using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockMonobehaviour : WorldEntityMonoBehaviour
{
    private bool needsUpdate = true;

    void Update()
    {
        if (needsUpdate)
        {
            transform.position = Model.Position;
            needsUpdate = false;
        }
    }

    public override string GetPopupText()
    {
        return string.Format("Rock ({0})", ((RockEntity)Model).AmountRemaining);
    }
}
