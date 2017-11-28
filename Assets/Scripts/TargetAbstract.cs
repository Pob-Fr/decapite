using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetAbstract {


    protected GameObject CURRENT_TARGET = null;

    protected abstract GameObject GetCurrentTargetFunction();

    public GameObject currentTarget {
        get { return GetCurrentTargetFunction(); }
        set { CURRENT_TARGET = value; }
    }

    public Hitable currentTartegHitable {
        get {
            if (currentTarget != null) {
                Hitable h = currentTarget.GetComponent<Entity>();
                if (h != null)
                    return h;
                h = currentTarget.GetComponent<Dice>();
                return h;
            }
            return null;
        }
    }


}
