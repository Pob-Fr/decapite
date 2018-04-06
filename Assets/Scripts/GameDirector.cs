using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour {

    public static GameDirector singleton;

    public float currentGameTime = 0;

    public bool isMultiplayer = false;
    private bool isPlaying = true;
    public int numberPlayers = 1;

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

    public List<GameObject> players;

    public Camera gameCamera;

    public AudioSource audioPlayer;

    public AudioClip gameMusic;
    public AudioClip gameoverMusic;
    public AudioClip gameoverJingle;

    public UnityEngine.UI.Text timerDisplayer;
    public UnityEngine.UI.Text eventDisplayer;
    public UnityEngine.UI.Image gameoverDisplayer;
    public UnityEngine.UI.Text statsTitle;
    public UnityEngine.UI.Text scoreTitle;
    public UnityEngine.UI.Text totalZombiekillsTitle;
    public UnityEngine.UI.Text bestDiceStreakTitle;
    public UnityEngine.UI.Text timeAliveTitle;
    public UnityEngine.UI.Text highscoreDisplayer;
    public UnityEngine.UI.Text tryagainDisplayer;

    public PlayerScoreViewController[] playerScoreViewControllers;

    public AudioClip hordeJingle; // instant zombies
    public AudioClip scoreJingle; // extra score
    public AudioClip frenzyJingle; // faster zombies
    public AudioClip lifeJingle; // extra health
    public AudioClip floodJingle; // extra zombies

    // Use this for initialization
    void Start() {
        singleton = this;
        audioPlayer.volume = AudioOptionController.GetMusicVolume();
        numberPlayers = players.Count;
        EffectSpawnHorde.eventClip = hordeJingle;
        StartCoroutine(PeriodicZombieSpawn());
        StartCoroutine(PeriodicDiceSpawn());
        foreach (PlayerScoreViewController psvc in playerScoreViewControllers)
            psvc.UpdateView();
    }

    void Update() {
        if (Input.GetButton("Menu"))
            SceneManager.LoadScene("Scenes/MainMenu");
        if (isPlaying) {
            float delta = Time.deltaTime;
            // update time alive
            foreach (PlayerScoreViewController vc in playerScoreViewControllers) {
                if (vc.player.isAlive)
                    vc.score.totalTimeSurvived += delta;
            }
            // update time displayer
            currentGameTime += delta;
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
            if (tryagainDisplayer.enabled && (PlayerControls.player1Control.GetAttack() || PlayerControls.player2Control.GetAttack()))
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

            GameObject playerToChase = GetRandomPlayerToChase();
            Zombie z = Zombie.Spawn(zombiePrefab, new Vector3(x, y, y), playerToChase);
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

            GameObject playerToChase = GetRandomPlayerToChase();
            Maggot m = Maggot.Spawn(maggotPrefab, new Vector3(position.x + x, position.y + y, position.y + y), playerToChase);
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

    public GameObject GetRandomPlayerToChase() {
        if (players.Count > 0)
            return players[((int)Random.Range(0, players.Count))];
        return null;
    }

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

    public void PlayerPunchLine(Player p) {
        if (isPlaying)
            p.PunchLine();
    }

    public void AddScore(Player p, int score) {
        if (isPlaying) {
            p.score.currentScore += score;
            UpdatePlayerScore(p);
        }
    }

    public void HealPlayer(Player p, int health) {
        if (isPlaying) {
            p.Heal(health);
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

    #endregion

    #region INTERFACE

    public void UpdatePlayerScore(Player p) {
        playerScoreViewControllers[(int)p.playerSlot].UpdateView();
    }

    public void UpdatePlayerHealth(Player p) {
        playerScoreViewControllers[(int)p.playerSlot].UpdateView();
    }

    public void ShakeCamera(int iterations) {
        StartCoroutine(DoShakeCamera(iterations));
    }

    private IEnumerator DoShakeCamera(int iterations) {
        for (int i = 0; i < iterations; ++i) {
            gameCamera.transform.position = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), -10);
            yield return null;
        }
        gameCamera.transform.position = new Vector3(0, 0, -10);
    }

    public void Event(string text, AudioClip jingle = null) {
        if (isPlaying) {
            StartCoroutine(ShowEvent(text));
            if (jingle != null)
                audioPlayer.PlayOneShot(jingle, AudioOptionController.GetJinglesVolume());
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

    public void OnePlayerDead(GameObject o) {
        --numberPlayers;
        players.Remove(o);
        if (numberPlayers == 0) GameOver();
    }

    public void Restart() {
        if (isMultiplayer)
            SceneManager.LoadScene("Scenes/Game_duo");
        else
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
        yield return new WaitForSeconds(1f);
        audioPlayer.PlayOneShot(gameoverJingle, AudioOptionController.GetJinglesVolume());
        DisplayGameOver();
        DisplayStats();
        yield return new WaitForSeconds(0.3f);
        DisplayScore();
        yield return new WaitForSeconds(0.3f);
        DisplayTZK();
        yield return new WaitForSeconds(0.3f);
        DisplayBDS();
        yield return new WaitForSeconds(0.3f);
        DisplayTS();
        yield return new WaitForSeconds(0.3f);
        DisplayHighScore();
        yield return new WaitForSeconds(0.3f);
        DisplayTryAgain();
    }

    public void DisplayGameOver() {
        gameoverDisplayer.enabled = true;
    }

    public void DisplayStats() {
        statsTitle.enabled = true;
        foreach (PlayerScoreViewController vc in playerScoreViewControllers)
            vc.ShowNameEnd();
    }

    public void DisplayScore() {
        scoreTitle.enabled = true;
        foreach (PlayerScoreViewController vc in playerScoreViewControllers)
            vc.ShowScoreEnd();
    }

    public void DisplayTZK() {
        totalZombiekillsTitle.enabled = true;
        foreach (PlayerScoreViewController vc in playerScoreViewControllers)
            vc.ShowTZKEnd();
    }

    public void DisplayBDS() {
        bestDiceStreakTitle.enabled = true;
        foreach (PlayerScoreViewController vc in playerScoreViewControllers)
            vc.ShowBDSEnd();
    }

    public void DisplayTS() {
        timeAliveTitle.enabled = true;
        foreach (PlayerScoreViewController vc in playerScoreViewControllers)
            vc.ShowTSEnd();

    }

    public void DisplayHighScore() {
        int newHighScore = 0;
        foreach (PlayerScoreViewController p in playerScoreViewControllers) {
            if (p.score.currentScore > PlayerScore.highScore)
                newHighScore = p.score.currentScore;
        }
        if (newHighScore > PlayerScore.highScore) {
            highscoreDisplayer.text = "New high score : " + (PlayerScore.highScore = newHighScore);
        } else if (PlayerScore.highScore > 0) {
            highscoreDisplayer.text = "High score : " + PlayerScore.highScore;
        } else {
            highscoreDisplayer.text = "No score";
        }
        highscoreDisplayer.enabled = true;
    }

    public void DisplayTryAgain() {
        tryagainDisplayer.enabled = true;
    }

    #endregion

}
