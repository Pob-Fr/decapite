using UnityEngine;

public class EffectScore : Effect {
    int score;

    public EffectScore(Dice dice, int score) : base(dice) {
        this.score = score;
    }

    public void ChangeScore(int score) {
        this.score = score;
    }

    public override bool isBonus() {
        return true;
    }

    public override void DoSomething(Player lastAttacker) {
        GameDirector.singleton.AddScore(lastAttacker, score);
        GameDirector.singleton.Event("Score <color=#FFFF00>+" + score + "</color> !");
    }
}
