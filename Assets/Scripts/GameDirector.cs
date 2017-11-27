﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour {

    public static GameDirector singleton;
    public static int highScore = 0;

    private bool isPlaying = true;
    public int currentScore = 0;
    public double currentTime = 0;
    public int zombieKills = 0;
    //public int bestDiceStreak = 0;

    public float firstZombieSpawnDelay = 5f;
    public float periodicZombieSpawnDelay = 5f;
    public int periodicZombieSpawnCount = 3;
    public float zombieSpawnInterval = 0.2f;
    public float zombieRageDelay = 8f;

    public float firstDiceSpawnDelay = 5;
    public float periodicDiceSpawnDelay = 5;


    public GameObject zombiePrefab;
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
    public UnityEngine.UI.Text zombiekillsDisplayer;
    public UnityEngine.UI.Text tryagainDisplayer;

    public AudioClip hordeJingle; // instant zombies
    public AudioClip scoreJingle; // extra score
    public AudioClip frenzyJingle; // faster zombies
    public AudioClip lifeJingle; // extra health
    public AudioClip floodJingle; // extra zombies

    // Use this for initialization
    void Start() {
        singleton = this;
        StartCoroutine(PeriodicZombieSpawn());
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
            string h = "", m = "", s = "";
            int amount;
            if (currentTime > 3600) {
                amount = ((int)currentTime) / 3600;
                h = (amount < 10 ? "0" + amount : "" + amount) + ":";
            }
            if (currentTime > 60) {
                amount = (((int)currentTime) % 3600) / 60;
                m = (amount < 10 ? "0" + amount : "" + amount) + ":";
            }
            amount = (((int)currentTime) % 60);
            s = (amount < 10 ? "0" + amount : "" + amount);
            timerDisplayer.text = h + m + s;
        } else {
            if (tryagainDisplayer.enabled && (Input.GetButton("AttackJ") || Input.GetButton("AttackK")))
                Restart();
        }
    }

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
            float x = Random.Range(-16, 16);
            float y = Random.Range(-9, 3);

            Zombie z = Zombie.Spawn(zombiePrefab, new Vector3(x, y, y), player);
            z.StartCoroutine(z.Enrage(zombieRageDelay));
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
            float x = Random.Range(-16, 16);
            float y = Random.Range(-9, 3);
            Dice.Spawn(dicePrefab, new Vector3(x, y, y));
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

    public void ShakeCamera(int iterations) {
        StartCoroutine(DoShakeCamera(iterations));
    }

    private IEnumerator DoShakeCamera(int iterations) {
        for (int i = 0; i < iterations; ++i) {
            gameCamera.transform.position = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), -10);
            yield return null;
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
        if (currentScore > highScore) {
            highscoreDisplayer.text = "New high score : " + currentScore;
            highscoreDisplayer.enabled = true;
        } else if (highScore > 0) {
            highscoreDisplayer.text = "High score : " + highScore;
            highscoreDisplayer.enabled = true;
        } else {
            highscoreDisplayer.text = "No score";
            highscoreDisplayer.enabled = true;
        }
        yield return new WaitForSeconds(0.5f);
        zombiekillsDisplayer.text = "Zombies killed : " + zombieKills;
        zombiekillsDisplayer.enabled = true;
        yield return new WaitForSeconds(0.5f);
        tryagainDisplayer.enabled = true;
    }

    public void Restart() {
        SceneManager.LoadScene("Scenes/Game");
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

}
