using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPlayer : TargetAbstract {

    protected override GameObject GetCurrentTargetFunction() {
        return CURRENT_TARGET;
    }

}
