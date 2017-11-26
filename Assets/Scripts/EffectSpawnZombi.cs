using UnityEngine;

public class EffectSpawnZombi : Effect {
    int nbZombi;

    public EffectSpawnZombi(int number) {
        this.nbZombi = number;
    }

    public void ChangeNbZombi(int number) {
        this.nbZombi = number;
    }

    public bool isBonus() {
        return false;
    }

    public bool isMalus() {
        return true;
    }

    public void DoSomething() {
        GameDirector.singleton.SpawnZombis(nbZombi + GameDirector.singleton.periodicZombiSpawnCount);
        GameDirector.singleton.Event("Zombies !!");
    }
}
