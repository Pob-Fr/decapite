using UnityEngine;

public class EffectPlayerIncreaseMovementSpeed : Effect {
    float movementSpeedBoost = 0.1f;

    public EffectPlayerIncreaseMovementSpeed(Dice dice, float boost) : base(dice) {
        this.movementSpeedBoost = boost;
    }

    public void ChangeMovementBoost(int boost) {
        this.movementSpeedBoost = boost;
    }

    public override bool isMalus() {
        return false;
    }

    public override void DoSomething(Player lastAttacker) {
        GameDirector.singleton.IncreasePlayerMovementSpeed(lastAttacker, movementSpeedBoost);
        GameDirector.singleton.Event("<color=#00FF00>Adrenaline !</color>");
    }
}
