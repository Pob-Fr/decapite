using UnityEngine;

public class EffectSpawnZombiMore : Effect {
    int nbZombiIncr = 1;

    public EffectSpawnZombiMore(int number) {
        this.nbZombiIncr = number;
    }

    public void ChangeNbZombiIncr(int number) {
        this.nbZombiIncr = number;
    }

    public bool isBonus() {
        return false;
    }

    public bool isMalus() {
        return true;
    }

    public void DoSomething() {
        GameDirector.singleton.IncreaseZombiSpawnCount(nbZombiIncr);
        GameDirector.singleton.Event("More Z !!");
    }
}
