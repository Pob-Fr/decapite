using UnityEngine;

public class EffectScore : Effect {
    int score;

    public EffectScore(int score) {
        this.score = score;
    }

    public void ChangeScore(int score) {
        this.score = score;
    }

    public bool isBonus() {
        return true;
    }

    public bool isMalus() {
        return false;
    }

    public void DoSomething() {
        GameDirector.singleton.AddScore(score);
        GameDirector.singleton.Event("Score +" + score + " !");
    }
}
