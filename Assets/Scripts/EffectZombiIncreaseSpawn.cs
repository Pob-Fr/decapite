using UnityEngine;

public class EffectZombiIncreaseSpawn : Effect {
    int zombiSpawnIncr = 1;

    public EffectZombiIncreaseSpawn(int incr) {
        this.zombiSpawnIncr = incr;
    }

    public void ChangeNbZombiIncr(int incr) {
        this.zombiSpawnIncr = incr;
    }

    public bool isBonus() {
        return false;
    }

    public bool isMalus() {
        return true;
    }

    public void DoSomething() {
        GameDirector.singleton.IncreaseZombiSpawnCount(zombiSpawnIncr);
        GameDirector.singleton.Event("<color=#FF0000>Endless !</color>");
    }
}
