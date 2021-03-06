﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject optionsMenu;

    private void Start() {
        OpenGame();
    }

    public void OpenGame() {
        PlayerControls.LoadSettings();
        AudioOptionController.LoadSettings();
    }

    public void BackToMainMenu() {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        PlayerControls.SaveSettings();
        AudioOptionController.SaveSettings();
    }

    public void StartGameSolo() {
        SceneManager.LoadScene("Scenes/Game");
    }

    public void StartGameDuo() {
        SceneManager.LoadScene("Scenes/Game_Duo");
    }

    public void OpenOptionsMenu() {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CloseGame() {
        Application.Quit();
    }

}
