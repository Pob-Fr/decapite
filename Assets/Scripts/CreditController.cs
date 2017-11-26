using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Attack") || Input.GetButton("Menu")) {
            SceneManager.LoadScene("Scenes/MainMenu");
        } else if (Input.GetButton("Quit")) {
            Application.Quit();
        }
	}
}
