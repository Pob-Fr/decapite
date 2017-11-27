using UnityEngine;

public class EffectSpawnZombi : Effect {

    public bool isBonus() {
        return false;
    }

    public bool isMalus() {
        return true;
    }

    public void DoSomething() {
        GameDirector.singleton.SpawnZombis();
        GameDirector.singleton.Event("<color=#FF0000>Wave !</color>");
    }
}
