using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PersonMonobehaviour : WorldEntityMonoBehaviour
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

    public void SetNeedsUpdate() { needsUpdate = true; }

    public void Initialize(PersonEntity model)
    {
        base.Initialize(model);
        model.Monobehaviour = this;
    }

    public override string GetPopupText()
    {
        return string.Format("Ted");
    }
}
