using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour {

    public static GameDirector singleton;
    public static int highScore = 0;

    public static int currentScore = 0;
    public static float currentTime = 0;

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

    public AudioClip playMusic;
    public AudioClip gameoverMusic;
    public AudioClip gameoverJingle;
    //public AudioClip hordeJingle; // instant zombies
    //public AudioClip scoreJingle; // extra score
    //public AudioClip frenzyJingle; // faster zombies
    //public AudioClip lifeJingle; // extra health
    //public AudioClip floodJingle; // extra zombies

    // Use this for initialization
    void Start() {
        singleton = this;
        zombiPrefab = zombiPrefabs[0];
        StartCoroutine(PeriodicZombiSpawn());
        StartCoroutine(PeriodicDiceSpawn());
    }

    void Update() {
        currentTime += Time.deltaTime;
    }

    public IEnumerator PeriodicZombiSpawn() {
        yield return new WaitForSeconds(firstZombiSpawnDelay);
        while (true) {
            StartCoroutine(SpawnZombisDelayed(periodicZombiSpawnCount));
            yield return new WaitForSeconds(periodicZombiSpawnDelay);
        }
    }

    public void SpawnZombis(int count) {
        StartCoroutine(SpawnZombisDelayed(count));
    }

    public IEnumerator SpawnZombisDelayed(int count) {
        for (int i = 0; i < count; i++) {
            SpawnZombi();
            yield return new WaitForSeconds(zombiSpawnInterval);
        }
    }

    public void SpawnZombi() {
        float x = Random.Range(-5, 5);
        float y = Random.Range(-5, 5);

        Zombi.Spawn(zombiPrefab, new Vector3(x, y, 0), player);
    }

    public IEnumerator PeriodicDiceSpawn() {
        yield return new WaitForSeconds(firstDiceSpawnDelay);
        while (true) {
            SpawnDice();
            yield return new WaitForSeconds(periodicDiceSpawnDelay);
        }
    }

    public void SpawnDice() {
        /*float x = Random.Range(-5, 5);
        float y = Random.Range(-5, 5);
        Dice.Spawn(dicePrefab, new Vector3(x, y, 0));*/
        ShakeCamera(4);
    }

    public void AddScore(int score) {
        currentScore += score;
    }

    public void IncreaseZombiSpawnCount(int incr) {
        periodicZombiSpawnCount += incr;
    }

    public void DecreaseZombiSpawnDelay(float decr) {
        periodicZombiSpawnDelay -= decr;
    }

    public void ShakeCamera(int iterations) {
        StartCoroutine(DoShakeCamera(iterations));
    }

    private IEnumerator DoShakeCamera(int iterations) {
        for (int i = 0; i < iterations; ++i) {
            gameCamera.transform.position = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), -10);
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnGUI() {
        GUI.Label(new Rect(0, 10, 200, 20), "Timer : " + currentTime);
        GUI.Label(new Rect(0, 30, 200, 20), "Score : " + currentScore);
    }


}
