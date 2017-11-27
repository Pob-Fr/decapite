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
        if (Input.GetButton("AttackJ")|| Input.GetButton("AttackK"))
            SceneManager.LoadScene("Scenes/Game");
        if (Input.GetButton("Quit"))
            Application.Quit();
    }
}
