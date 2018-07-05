using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreViewController : MonoBehaviour {

    public UnityEngine.UI.Text scoreDisplayer;
    public UnityEngine.UI.Text lifeDisplayer;

    public UnityEngine.UI.Text endNameDisplayer;
    public UnityEngine.UI.Text endScoreDisplayer;
    public UnityEngine.UI.Text endKillsDisplayer;
    public UnityEngine.UI.Text endMultikillDisplayer;
    public UnityEngine.UI.Text endTimeDisplayer;

    public Player player;
    public PlayerScore score;

    public void UpdateView() {
        if (score != null) {
            scoreDisplayer.text = "" + score.currentScore;
        }
        if (player != null) {
            lifeDisplayer.text = "" + player.currentHealth;
        }
    }

    public void ShowNameEnd() {
        endNameDisplayer.enabled = true;
        endNameDisplayer.text = "" + player.GetPlayerName();
    }

    public void ShowScoreEnd() {
        endScoreDisplayer.enabled = true;
        endScoreDisplayer.text = "" + score.currentScore;
    }

    public void ShowTZKEnd() {
        endKillsDisplayer.enabled = true;
        endKillsDisplayer.text = "" + score.totalZombieKills;
    }

    public void ShowBDSEnd() {
        endMultikillDisplayer.enabled = true;
        endMultikillDisplayer.text = "" + score.bestDiceStreak;
    }

    public void ShowTSEnd() {
        endTimeDisplayer.enabled = true;
        string h = "", m = "", s = "";
        int amount;
        if (score.totalTimeSurvived > 3600) {
            amount = ((int)score.totalTimeSurvived) / 3600;
            h = (amount < 10 ? "0" + amount : "" + amount) + ":";
        }
        if (score.totalTimeSurvived > 60) {
            amount = (((int)score.totalTimeSurvived) % 3600) / 60;
            m = (amount < 10 ? "0" + amount : "" + amount) + ":";
        }
        amount = (((int)score.totalTimeSurvived) % 60);
        s = (amount < 10 ? "0" + amount : "" + amount);
        endTimeDisplayer.text = h + m + s;
    }

}
