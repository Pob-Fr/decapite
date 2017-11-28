using UnityEngine;

public class EffectZombieRageFaster : Effect {
    float zombieRageDelayDecr = 0.5f;

    public EffectZombieRageFaster(Dice dice, float decr) : base(dice) {
        this.zombieRageDelayDecr = decr;
    }

    public void ChangeZombiDelayDecr(float decr) {
        this.zombieRageDelayDecr = decr;
    }

    public override bool isMalus() {
        return true;
    }

    public override void DoSomething() {
        GameDirector.singleton.DecreaseZombieRageDelay(zombieRageDelayDecr);
        GameDirector.singleton.Event("<color=#FF0000>Frenzy !</color>");
    }
}
