using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour {

    public static GameDirector singleton;
    public static int highScore = 0;

    private bool isPlaying = true;
    public int currentScore = 0;
    public float currentTime = 0;

    public float firstZombiSpawnDelay = 5;
    public float periodicZombiSpawnDelay = 10;
    public int periodicZombiSpawnCount = 3;
    public float zombiSpawnInterval = 0.2f;

    public float firstDiceSpawnDelay = 5;
    public float periodicDiceSpawnDelay = 5;


    public List<GameObject> zombiPrefabs;

    public GameObject zombiPrefab;
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
    public UnityEngine.UI.Text tryagainDisplayer;

    public AudioClip hordeJingle; // instant zombies
    public AudioClip scoreJingle; // extra score
    public AudioClip frenzyJingle; // faster zombies
    public AudioClip lifeJingle; // extra health
    public AudioClip floodJingle; // extra zombies

    // Use this for initialization
    void Start() {
        singleton = this;
        zombiPrefab = zombiPrefabs[0];
        StartCoroutine(PeriodicZombiSpawn());
        StartCoroutine(PeriodicDiceSpawn());
        EffectSpawnHorde.eventClip = hordeJingle;
    }

    void Update() {
        if (Input.GetButton("Menu"))
            SceneManager.LoadScene("Scenes/MainMenu");
        if (Input.GetButton("Quit"))
            Application.Quit();
                if (isPlaying) {
            currentTime += Time.deltaTime;
            timerDisplayer.text = "" + ((float)((int)(currentTime * 10))) / 10f;
        } else {
            if (tryagainDisplayer.enabled && Input.GetButton("Attack"))
                Restart();
        }
    }

    public IEnumerator PeriodicZombiSpawn() {
        yield return new WaitForSeconds(firstZombiSpawnDelay);
        while (true) {
            if (isPlaying) {
                StartCoroutine(SpawnZombisDelayed(periodicZombiSpawnCount));
                yield return new WaitForSeconds(periodicZombiSpawnDelay);
            }
        }
    }

    public void SpawnZombis(int count) {
        if (isPlaying) {
            StartCoroutine(SpawnZombisDelayed(count));
        }
    }

    public IEnumerator SpawnZombisDelayed(int count) {
        for (int i = 0; i < count; i++) {
            if (isPlaying) {
                SpawnZombi();
                yield return new WaitForSeconds(zombiSpawnInterval);
            }
        }
    }

    public void SpawnZombi() {
        if (isPlaying) {
            float x = Random.Range(-5, 5);
            float y = Random.Range(-5, 5);

            Zombi.Spawn(zombiPrefab, new Vector3(x, y, 0), player);
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
            float x = Random.Range(-5, 5);
            float y = Random.Range(-5, 5);
            Dice.Spawn(dicePrefab, new Vector3(x, y, 0));
        }
    }

    public void AddScore(int score) {
        if (isPlaying) {
            currentScore += score;
            if (!scoreDisplayer.enabled)
                scoreDisplayer.enabled = true;
            scoreDisplayer.text = "" + currentScore;
        }
    }

    public void IncreaseZombiSpawnCount(int incr) {
        if (isPlaying) {
            periodicZombiSpawnCount += incr;
        }
    }

    public void DecreaseZombiSpawnDelay(float decr) {
        if (isPlaying) {
            periodicZombiSpawnDelay -= decr;
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

    public void UpdatePlayerHealth(int health) {
        if (isPlaying) {
            lifeDisplayer.text = "LIFE : " + health;
        }
    }

    public void GameOver() {
        if (isPlaying) {
            StopAllCoroutines();
            StartCoroutine(ShowGameOver());
        }
    }

    public IEnumerator ShowGameOver() {
        isPlaying = false;
        audioPlayer.PlayOneShot(gameoverJingle);
        lifeDisplayer.enabled = false;
        gameoverDisplayer.enabled = true;
        yield return new WaitForSeconds(1);
        tryagainDisplayer.enabled = true;
    }

    public void Restart() {
        SceneManager.LoadScene("Scenes/Game");
    }

    public void Event(string text, AudioClip jingle = null) {
        if (isPlaying) {
            StartCoroutine(ShowEvent(text));
            if (jingle != null)
                audioPlayer.PlayOneShot(jingle, 1.25f);
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

}
