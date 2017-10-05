using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CamaraController : PathFollower {

    public bool finalBattle = false;
    private bool sniperMissionCompleted = false;
    private FinalBattle final;
    private SnippingMission snipping;

    private EnemyFactory factory;

    void Start() {
        shift = new Vector3(0, 0.5f, 0);
        init();
        final = GetComponent<FinalBattle>();
        snipping = GetComponent<SnippingMission>();
        factory = final.factory;
        Wait = true;
        StartCoroutine(startFading());
    }

    void Update()
    {
        rotateTheWord();
        move();
        if (currentWaypoint == waypoints.Count)
        {
            SceneManager.LoadScene("mine", LoadSceneMode.Single);
        }
        if (waypoints[currentWaypoint].name.Contains("startFight") && !sniperMissionCompleted)
        {
            Wait = true;
        }
        if (waypoints[currentWaypoint].name.Contains("finalBattle") && !finalBattle) {
            startEpicFinalBattle();
        }
    }

    private void rotateTheWord()
    {
        Transform cameraPos = transform.GetChild(0);
        transform.GetChild(2).transform.position = new Vector3(cameraPos.position.x, cameraPos.position.y, cameraPos.position.z) + (cameraPos.forward) * 3.5f;
        transform.GetChild(2).transform.LookAt(cameraPos);
    }

    private IEnumerator startFading()
    {
        float fadeTime = GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
        Wait = false;
    }


    private IEnumerator fading(bool start, PathFollower follow)
    {
        float fadeTime = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        fadeTime = GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
        Wait = start;
        if(follow != null)
            follow.Go();
       
    }

    private void startEpicFinalBattle()
    {
        finalBattle = true;
        Debug.Log("Final battle started");
        StartCoroutine(fading(true, final));

    }

    public void stopEpicFinalBattle()
    {
        StartCoroutine(fading(false, null));
        Debug.Log("Final battler ended");
    }

    public void activateZombies()
    {
        factory.zombiesAttackActivated = true;
        factory.activeZombiesAttack();
    }

    public void startSnipeMission()
    {
        StartCoroutine(fading(true, snipping));
    }

    public void stopSnipeMission()
    {
        Debug.Log("Snipe mission stoped ");
        sniperMissionCompleted = true;
        StartCoroutine(fading(true, this));
    }

    //dont need to override this , because it isn't a mission 
    public override void Go()
    {
        Wait = false;
        Debug.Log("ggogog");
    }
}
