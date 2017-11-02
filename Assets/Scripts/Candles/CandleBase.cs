using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CandleBase : MonoBehaviour {
    
    public CamaraController camara;
    public delegate void actionBetweenFadeing();
    private bool isPaused = false;

    public void jumpTo()
    {
        if (!isPaused)
        {
            Debug.Log("Teleporting started");
            StartCoroutine(camara.fadingWithAction(fadingActions));
        }
    }

    public abstract void fadingActions();

    void OnPauseGame()
    {
        isPaused = true;
    }

    void OnResumeGame()
    {
        isPaused = false;
    }

}
