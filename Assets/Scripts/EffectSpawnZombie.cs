using UnityEngine;

public class EffectSpawnZombie : Effect {

    public EffectSpawnZombie(Dice dice) : base(dice) {

    }

    public override bool isMalus() {
        return true;
    }

    public override void DoSomething() {
        GameDirector.singleton.SpawnZombies();
        GameDirector.singleton.Event("<color=#FF0000>Wave !</color>");
    }
}
