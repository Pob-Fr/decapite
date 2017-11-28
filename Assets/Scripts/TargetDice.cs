using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDice : TargetAbstract {

    protected override GameObject GetCurrentTargetFunction() {
        if (CURRENT_TARGET == null) {
            CURRENT_TARGET = GameDirector.singleton.RequestDiceTarget();
        }
        return CURRENT_TARGET;
    }
}
