using UnityEngine;

public class EffectZombieRageFaster : Effect {
    float zombieRageDelayDecr = 0.5f;

    public EffectZombieRageFaster(float decr) {
        this.zombieRageDelayDecr = decr;
    }

    public void ChangeZombiDelayDecr(float decr) {
        this.zombieRageDelayDecr = decr;
    }

    public bool isBonus() {
        return false;
    }

    public bool isMalus() {
        return true;
    }

    public void DoSomething() {
        GameDirector.singleton.DecreaseZombieRageDelay(zombieRageDelayDecr);
        GameDirector.singleton.Event("<color=#FF0000>Frenzy !</color>");
    }
}
