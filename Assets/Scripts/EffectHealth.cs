﻿using UnityEngine;

public class EffectHealth : Effect {
    int hp;

    public EffectHealth(int hp) {
        this.hp = hp;
    }

    public void ChangeScore(int hp) {
        this.hp = hp;
    }

    public bool isBonus() {
        return true;
    }

    public bool isMalus() {
        return false;
    }

    public void DoSomething() {
        GameDirector.singleton.UpdatePlayerHealth(GameDirector.singleton.player.GetComponent<Player>().currentHealth += hp);
        GameDirector.singleton.Event("Life +" + hp + " !");
    }
}
