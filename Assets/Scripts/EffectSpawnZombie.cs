using UnityEngine;

public class EffectSpawnZombie : Effect {

    public bool isBonus() {
        return false;
    }

    public bool isMalus() {
        return true;
    }

    public void DoSomething() {
        GameDirector.singleton.SpawnZombies();
        GameDirector.singleton.Event("<color=#FF0000>Wave !</color>");
    }
}
