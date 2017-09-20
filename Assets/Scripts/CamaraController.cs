using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamaraController : PathFollower  {

    void Start () {
        init();
    }

    void Update()
    {
        move();
        if(currentWaypoint == waypoints.Count)
        {
            //SceneManager.LoadScene("mine", LoadSceneMode.Single);
        }
    }
}
