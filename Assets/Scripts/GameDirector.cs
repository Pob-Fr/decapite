using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour {

    public static GameDirector singleton;

    private bool isPlaying = true;

    public float firstZombieSpawnDelay = 5f;
    public float periodicZombieSpawnDelay = 5f;
    public int periodicZombieSpawnCount = 3;
    public float zombieSpawnInterval = 0.2f;
    public float zombieRageDelay = 10f;

    public float firstDiceSpawnDelay = 5;
    public float periodicDiceSpawnDelay = 5;


    public GameObject zombiePrefab;
    public GameObject maggotPrefab;
    public GameObject dicePrefab;

    public GameObject player;

    public Camera gameCamera;

    public AudioSource audioPlayer;

    public AudioClip gameMusic;
    public AudioClip gameoverMusic;
    public AudioClip gameoverJingle;

    public UnityEngine.UI.Text scoreDisplayer;
    public UnityEngine.UI.Text timerDisplayer;
    public UnityEngine.UI.Text eventDisplayer;
    public UnityEngine.UI.Text lifeDisplayer;
    public UnityEngine.UI.Image gameoverDisplayer;
    public UnityEngine.UI.Text highscoreDisplayer;
    public UnityEngine.UI.Text totalZombiekillsDisplayer;
    public UnityEngine.UI.Text bestDiceStreakDisplayer;
    public UnityEngine.UI.Text tryagainDisplayer;

    public AudioClip hordeJingle; // instant zombies
    public AudioClip scoreJingle; // extra score
    public AudioClip frenzyJingle; // faster zombies
    public AudioClip lifeJingle; // extra health
    public AudioClip floodJingle; // extra zombies

    // Use this for initialization
    void Start() {
        singleton = this;
        ScoreHelper.Reset();
        EffectSpawnHorde.eventClip = hordeJingle;
        StartCoroutine(PeriodicZombieSpawn());
        StartCoroutine(PeriodicDiceSpawn());
    }

    void Update() {
        if (Input.GetButton("Menu"))
            SceneManager.LoadScene("Scenes/MainMenu");
        if (Input.GetButton("Quit"))
            Application.Quit();
        if (isPlaying) {
            float currentGameTime = ScoreHelper.totalTimeSurvived += Time.deltaTime;
            string h = "", m = "", s = "";
            int amount;
            if (currentGameTime > 3600) {
                amount = ((int)currentGameTime) / 3600;
                h = (amount < 10 ? "0" + amount : "" + amount) + ":";
            }
            if (currentGameTime > 60) {
                amount = (((int)currentGameTime) % 3600) / 60;
                m = (amount < 10 ? "0" + amount : "" + amount) + ":";
            }
            amount = (((int)currentGameTime) % 60);
            s = (amount < 10 ? "0" + amount : "" + amount);
            timerDisplayer.text = h + m + s;
        } else {
            if (tryagainDisplayer.enabled && (Input.GetButton("AttackJ") || Input.GetButton("AttackK")))
                Restart();
        }
    }

    #region SPAWNS

    public IEnumerator PeriodicZombieSpawn() {
        yield return new WaitForSeconds(firstZombieSpawnDelay);
        while (true) {
            if (isPlaying) {
                StartCoroutine(SpawnZombiesDelayed(periodicZombieSpawnCount));
                yield return new WaitForSeconds(periodicZombieSpawnDelay);
            }
        }
    }

    public void SpawnZombies() {
        if (isPlaying) {
            StartCoroutine(SpawnZombiesDelayed(periodicZombieSpawnCount));
        }
    }

    public void SpawnHorde(int multiplier) {
        if (isPlaying) {
            StartCoroutine(SpawnZombiesDelayed(periodicZombieSpawnCount * multiplier));
        }
    }

    public IEnumerator SpawnZombiesDelayed(int count) {
        for (int i = 0; i < count; i++) {
            if (isPlaying) {
                SpawnZombie();
                yield return new WaitForSeconds(zombieSpawnInterval);
            }
        }
    }

    public void SpawnZombie() {
        if (isPlaying) {
            float x = Random.Range(-16f, 16f);
            float y = Random.Range(-9f, 3f);

            Zombie z = Zombie.Spawn(zombiePrefab, new Vector3(x, y, y), player);
            z.StartCoroutine(z.Enrage(zombieRageDelay));
        }
    }

    public void SpawnMaggots(Vector2 position, int count) {
        for (int i = 0; i < count; i++) {
            if (isPlaying) {
                SpawnMaggot(position);
            }
        }
    }

    public void SpawnMaggot(Vector2 position) {
        if (isPlaying) {
            float x = Random.Range(-2f, 2f);
            float y = Random.Range(-2f, 2f);

            Maggot m = Maggot.Spawn(maggotPrefab, new Vector3(position.x + x, position.y + y, position.y + y), player);
            if (x < 0)
                m.isLookinkRight = false;
        }
    }

    public IEnumerator PeriodicDiceSpawn() {
        yield return new WaitForSeconds(firstDiceSpawnDelay);
        while (true) {
            if (isPlaying) {
                SpawnDice();
                yield return new WaitForSeconds(periodicDiceSpawnDelay);
            }
        }
    }

    public void SpawnDice() {
        if (isPlaying) {
            float x = Random.Range(-16f, 16f);
            float y = Random.Range(-9f, 3f);
            Dice.Spawn(dicePrefab, new Vector3(x, y, y));
        }
    }

    #endregion

    #region TARGETS

    private List<Player> playerTracker = new List<Player>();

    public void StartTrackingPlayer(Player player) {
        playerTracker.Add(player);
    }

    public void StopTrackingPlayere(Player player) {
        playerTracker.Remove(player);
    }

    public GameObject RequestPlayerTarget() {
        if (playerTracker.Count() > 0)
            return playerTracker.ElementAt(Random.Range(0, diceTracker.Count())).gameObject;
        return null;
    }



    private List<Dice> diceTracker = new List<Dice>();

    public void StartTrackingDice(Dice dice) {
        diceTracker.Add(dice);
    }

    public void StopTrackingDice(Dice dice) {
        diceTracker.Remove(dice);
    }

    public GameObject RequestDiceTarget() {
        if (diceTracker.Count() > 0)
            return diceTracker.ElementAt(Random.Range(0, diceTracker.Count())).gameObject;
        return null;
    }

    #endregion

    #region DICE_EVENTS

    public void AddScore(int score) {
        if (isPlaying) {
            scoreDisplayer.text = "" + (ScoreHelper.currentScore += score);
            if (!scoreDisplayer.enabled)
                scoreDisplayer.enabled = true;
        }
    }

    public void IncreaseZombieSpawnCount(int incr) {
        if (isPlaying) {
            periodicZombieSpawnCount += incr;
        }
    }

    public void DecreaseZombieRageDelay(float decr) {
        if (isPlaying) {
            zombieRageDelay -= decr;
        }
    }

    public void PlayerPunchLine() {
        if (isPlaying)
            player.GetComponent<Player>().PunchLine();
    }

    public void HealPlayer(int health) {
        if (isPlaying) {
            player.GetComponent<Player>().Heal(health);
        }
    }

    public void UpdatePlayerHealth(int health) {
        if (isPlaying) {
            lifeDisplayer.text = "LIFE : " + health;
        }
    }

    public void ShakeCamera(int iterations) {
        StartCoroutine(DoShakeCamera(iterations));
    }

    private IEnumerator DoShakeCamera(int iterations) {
        for (int i = 0; i < iterations; ++i) {
            gameCamera.transform.position = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), -10);
            yield return null;
        }
    }

    public void Event(string text, AudioClip jingle = null) {
        if (isPlaying) {
            StartCoroutine(ShowEvent(text));
            if (jingle != null)
                audioPlayer.PlayOneShot(jingle, 1f);
        }
    }

    private IEnumerator ShowEvent(string text) {
        if (isPlaying) {
            eventDisplayer.text = text;
            eventDisplayer.enabled = true;
            yield return new WaitForSeconds(3);
        }
        if (isPlaying) {
            eventDisplayer.enabled = false;
        }
    }

    #endregion

    #region META

    public void Restart() {
        SceneManager.LoadScene("Scenes/Game");
    }

    public void GameOver() {
        if (isPlaying) {
            StopAllCoroutines();
            StartCoroutine(ShowGameOver());
        }
    }

    public IEnumerator ShowGameOver() {
        isPlaying = false;
        audioPlayer.clip = gameoverMusic;
        audioPlayer.Play();
        lifeDisplayer.enabled = false;
        yield return new WaitForSeconds(1f);
        //gameoverDisplayer.enabled = true;
        audioPlayer.PlayOneShot(gameoverJingle, 1f);
        yield return new WaitForSeconds(0.5f);
        if (ScoreHelper.currentScore > ScoreHelper.highScore) {
            highscoreDisplayer.text = "New high score : " + (ScoreHelper.highScore = ScoreHelper.currentScore);
            highscoreDisplayer.enabled = true;
        } else if (ScoreHelper.highScore > 0) {
            highscoreDisplayer.text = "High score : " + ScoreHelper.highScore;
            highscoreDisplayer.enabled = true;
        } else {
            highscoreDisplayer.text = "No score";
            highscoreDisplayer.enabled = true;
        }
        yield return new WaitForSeconds(0.5f);
        totalZombiekillsDisplayer.text = "Total Zombie kills : " + ScoreHelper.totalZombieKills;
        totalZombiekillsDisplayer.enabled = true;
        yield return new WaitForSeconds(0.5f);
        bestDiceStreakDisplayer.text = "Best dice streak : " + ScoreHelper.bestDiceStreak;
        bestDiceStreakDisplayer.enabled = true;
        yield return new WaitForSeconds(0.5f);
        tryagainDisplayer.enabled = true;
    }

    #endregion

}
