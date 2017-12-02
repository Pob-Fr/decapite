using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    public GameObject mainMenu;
    public GameObject optionsMenu;

    public void BackToMainMenu() {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
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
