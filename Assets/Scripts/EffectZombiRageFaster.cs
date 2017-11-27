using UnityEngine;

public class EffectZombiRageFaster : Effect {
    float zombiRageDelayDecr = 0.5f;

    public EffectZombiRageFaster(float decr) {
        this.zombiRageDelayDecr = decr;
    }

    public void ChangeZombiDelayDecr(float decr) {
        this.zombiRageDelayDecr = decr;
    }

    public bool isBonus() {
        return false;
    }

    public bool isMalus() {
        return true;
    }

    public void DoSomething() {
        GameDirector.singleton.DecreaseZombiRageDelay(zombiRageDelayDecr);
        GameDirector.singleton.Event("<color=#FF0000>Frenzy !</color>");
    }
}
