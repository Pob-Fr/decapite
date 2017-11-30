using UnityEngine;

public class EffectSpawnHorde : Effect {

    public static AudioClip eventClip = null;

    int multiplier;

    public EffectSpawnHorde(Dice dice, int number) : base(dice) {
        this.multiplier = number;
    }

    public void ChangeNbZombi(int number) {
        this.multiplier = number;
    }

    public override bool isMalus() {
        return true;
    }

    public override void DoSomething(Player lastAttacker) {
        GameDirector.singleton.SpawnHorde(multiplier);
        GameDirector.singleton.Event("<color=#FF0000>Horde x" + multiplier + " !!!</color>", eventClip);
        GameDirector.singleton.ShakeCamera(10);
    }
}
