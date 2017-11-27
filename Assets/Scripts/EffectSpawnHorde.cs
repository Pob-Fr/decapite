using UnityEngine;

public class EffectSpawnHorde : Effect {

    public static AudioClip eventClip = null;

    int multiplier;

    public EffectSpawnHorde(int number) {
        this.multiplier = number;
    }

    public void ChangeNbZombi(int number) {
        this.multiplier = number;
    }

    public bool isBonus() {
        return false;
    }

    public bool isMalus() {
        return true;
    }

    public void DoSomething() {
        GameDirector.singleton.SpawnHorde(multiplier);
        GameDirector.singleton.Event("<color=#FF0000>Horde x" + multiplier + " !!!</color>", eventClip);
        GameDirector.singleton.ShakeCamera(10);
    }
}
