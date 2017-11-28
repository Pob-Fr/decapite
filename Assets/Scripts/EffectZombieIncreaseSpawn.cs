using UnityEngine;

public class EffectZombieIncreaseSpawn : Effect {
    int zombieSpawnIncr = 1;

    public EffectZombieIncreaseSpawn(Dice dice, int incr) : base(dice) {
        this.zombieSpawnIncr = incr;
    }

    public void ChangeNbZombiIncr(int incr) {
        this.zombieSpawnIncr = incr;
    }

    public override bool isMalus() {
        return true;
    }

    public override void DoSomething() {
        GameDirector.singleton.IncreaseZombieSpawnCount(zombieSpawnIncr);
        GameDirector.singleton.Event("<color=#FF0000>Infestation !</color>");
    }
}
