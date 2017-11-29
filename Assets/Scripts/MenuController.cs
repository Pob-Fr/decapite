using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButton("AttackC")|| Input.GetButton("AttackK"))
            SceneManager.LoadScene("Scenes/Game");
        if (Input.GetButton("AttackC_2") || Input.GetButton("AttackK_2"))
            SceneManager.LoadScene("Scenes/Game_Duo");
        if (Input.GetButton("Quit"))
            Application.Quit();
    }
}
