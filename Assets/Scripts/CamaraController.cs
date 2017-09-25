using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamaraController : PathFollower  {

    private bool finalBattle = false;

    void Start () {
        init();
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


    private void startEpicFinalBattle()
    {
        finalBattle = true;
        Wait = true;
    }

    private void stopEpicFinalBattle()
    {
        finalBattle = false;
        Wait = false;
    }
}
