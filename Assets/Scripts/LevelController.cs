using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    public GameObject blue;
    public GameObject green;
    public GameObject red;


    public void jumpToSnipperMission()
    {

    }

    public void jumpToFinalBattle()
    {

    }

    public void jumpToMine()
    {
        SceneManager.LoadScene("mine", LoadSceneMode.Single);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            jumpToSnipperMission();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            jumpToFinalBattle();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            jumpToMine();
        }
    }
}
