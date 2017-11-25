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

    public float firstDiceSpawnDelay = 15;
    public float periodicDiceSpawnDelay = 20;


    public List<GameObject> zombiPrefabs;

    public GameObject zombiPrefab;

    public GameObject player;

    // Use this for initialization
    void Start() {
        singleton = this;
        zombiPrefab = zombiPrefabs[0];
        StartCoroutine(PeriodicZombiSpawn());
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
        for (int i = 0; i < count; i++) {
            SpawnZombi();
        }
    }

    public IEnumerator SpawnZombisDelayed(int count) {
        for (int i = 0; i < count; i++) {
            SpawnZombi();
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void SpawnZombi() {
        float x = Random.Range(-5, 5);
        float y = Random.Range(-5, 5);

        Zombi.Spawn(zombiPrefab, new Vector3(x, y, 0), player);
    }

    public IEnumerator PeriodicDiceSpawn() {
        while (true) {
            SpawnDice();
            yield return new WaitForSeconds(periodicDiceSpawnDelay);
        }
    }

    public void SpawnDice() {

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

    void OnGUI() {
        GUI.Label(new Rect(0, 10, 200, 20), "Timer : " + currentTime);
        GUI.Label(new Rect(0, 30, 200, 20), "Score : " + currentScore);
    }


}
