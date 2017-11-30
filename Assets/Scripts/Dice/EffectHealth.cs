using UnityEngine;

public class EffectHealth : Effect {
    int hp;

    public EffectHealth(Dice dice, int hp) : base(dice){
        this.hp = hp;
    }

    public void ChangeScore(int hp) {
        this.hp = hp;
    }

    public override bool isBonus() {
        return true;
    }

    public override void DoSomething(Player lastAttacker) {
        GameDirector.singleton.HealPlayer(lastAttacker, hp);
        GameDirector.singleton.Event("Life <color=#00FF00>+" + hp + "</color> !");
    }
}
