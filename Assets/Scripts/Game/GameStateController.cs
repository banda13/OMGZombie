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
            StartCoroutine(updateGameState());
        }
	}

    private IEnumerator updateGameState()
    {
        
        GameObject[] objects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in objects)
        {
            //foreach (Component go in obj.GetComponents<Component>())
            //{
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
                //}
            }
        }
        if (!isPaused)
        {
            Time.timeScale = 1;
        }
        yield return new WaitForSeconds(0.5f);

        if (isPaused)
        {
            Time.timeScale = 0.000001f;
        }
        
    }
}
