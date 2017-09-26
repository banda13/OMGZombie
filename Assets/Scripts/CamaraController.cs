using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CamaraController : PathFollower  {

    public bool finalBattle = false;
    private FinalBattle final;


    void Start () {
        init();
        final = GetComponent<FinalBattle>();
    }

    void Update()
    {
        move();
        if(currentWaypoint == waypoints.Count)
        {
            SceneManager.LoadScene("mine", LoadSceneMode.Single);
        }
        if (waypoints[currentWaypoint].name.Contains("finalBattle") && !finalBattle){
            startEpicFinalBattle();
        }
    }
    private IEnumerator fading()
    {
        float fadeTime = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        fadeTime = GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }

    private void startEpicFinalBattle()
    {
        finalBattle = true;
        Debug.Log("Final battle started");
        StartCoroutine(fading());
        Wait = true;
        final.Go(this);

    }

    public void stopEpicFinalBattle()
    {
        StartCoroutine(fading());
        Wait = false;
        Debug.Log("Final battler ended");
    }
}
