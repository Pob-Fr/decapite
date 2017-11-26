using UnityEngine;

public class EffectSpawnZombiFaster : Effect {
    float zombiDelayDecr = 0.5f;

    public EffectSpawnZombiFaster(float number) {
        this.zombiDelayDecr = number;
    }

    public void ChangeZombiDelayDecr(float number) {
        this.zombiDelayDecr = number;
    }

    public bool isBonus() {
        return false;
    }

    public bool isMalus() {
        return true;
    }

    public void DoSomething() {
        GameDirector.singleton.DecreaseZombiSpawnDelay(zombiDelayDecr);
        GameDirector.singleton.Event("Faster Z !!");
    }
}
