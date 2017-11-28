using UnityEngine;

public class EffectSpawnMaggot : Effect {

    public EffectSpawnMaggot(Dice dice) : base(dice) {
    }

    public override bool isMalus() {
        return true;
    }

    public override void DoSomething() {
        GameDirector.singleton.SpawnMaggots(dice.transform.position, 5);
        GameDirector.singleton.Event("<color=#FF0000>Rotten !</color>");
    }
}
