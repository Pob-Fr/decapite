using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScoreHelper {

    public static int highScore = 0;

    public static int currentScore = 0;
    public static int totalZombieKills = 0;
    public static int bestDiceStreak = 0;
    public static float totalTimeSurvived = 0;


    public static void Reset() {
        currentScore = 0;
        totalZombieKills = 0;
        bestDiceStreak = 0;
        totalTimeSurvived = 0;
    }


}
