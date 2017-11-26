using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateController : MonoBehaviour {

    private GameObject player;
    private bool isPaused;
    private float gamePauseDelay = 0.5f;
    private float pauseTimeScale = 0.0000001f;
    private float normalTimeScale = 1;
    
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {

        if (GvrControllerInput.AppButtonDown)
        {
            isPaused = !isPaused;
            Debug.Log("Pause: " + isPaused);
            StartCoroutine(updateGameState());
        }
	}

    private IEnumerator updateGameState()
    {
        
        GameObject[] objects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in objects)
        {
            if (go && go.transform.parent == null)
            {
                if (isPaused)
                {
                    go.BroadcastMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
                }
                else
                {
                    go.BroadcastMessage("OnResumeGame", SendMessageOptions.DontRequireReceiver);
                }
            }
        }
        if (!isPaused)
        {
            Time.timeScale = normalTimeScale;
        }
        yield return new WaitForSeconds(gamePauseDelay);

        if (isPaused)
        {
            Time.timeScale = pauseTimeScale;
        }
    }
}
