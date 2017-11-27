using UnityEngine;

public class EffectZombieIncreaseSpawn : Effect {
    int zombieSpawnIncr = 1;

    public EffectZombieIncreaseSpawn(int incr) {
        this.zombieSpawnIncr = incr;
    }

    public void ChangeNbZombiIncr(int incr) {
        this.zombieSpawnIncr = incr;
    }

    public bool isBonus() {
        return false;
    }

    public bool isMalus() {
        return true;
    }

    public void DoSomething() {
        GameDirector.singleton.IncreaseZombieSpawnCount(zombieSpawnIncr);
        GameDirector.singleton.Event("<color=#FF0000>Endless !</color>");
    }
}
