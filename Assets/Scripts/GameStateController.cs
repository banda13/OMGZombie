using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour {

    private GameObject player;
    private bool isPaused;
    
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {

        if (GvrControllerInput.AppButtonDown)
        {
            isPaused = !isPaused;
            Debug.Log("Pause: " + isPaused);
            
        }
	}

    private void updateGameState()
    {
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
