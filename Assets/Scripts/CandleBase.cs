using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CandleBase : MonoBehaviour {
    
    public CamaraController camara;
    public delegate void actionBetweenFadeing();

    public void jumpTo()
    {
        Debug.Log("Teleporting started");
        if(camara == null)
        {
            Debug.Log("cant set camera");
        }
        StartCoroutine(camara.fadingWithAction(fadingActions));
    }

    public abstract void fadingActions();
    
}
