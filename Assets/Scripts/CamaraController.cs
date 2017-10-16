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
#if UNITY_EDITOR
        shift = new Vector3(0, 0.5f, 0);
#endif
        init();
        final = GetComponent<FinalBattle>();
        snipping = GetComponent<SnippingMission>();
        factory = final.factory;
        Wait = true;
        StartCoroutine(startFading());

        AdaptedEventHandler.init();
    }

    void Update()
    {
        //transform.GetChild(0).GetComponent<Camera>().fieldOfView = 20; ->not working
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
        if (waypoints[currentWaypoint].name.Contains("finalBattle"))
        {
            if (!finalBattle)
            {
                startEpicFinalBattle();
            }
            else if (final.battleStarted && final.battleCompleted)
            {
                Debug.Log("Final battle completed");
                Go();
            }
        }

        if (Input.GetKeyDown("space"))
        {
            factory.killAll();
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
        Go();
    }
    

    private IEnumerator fading(bool start, PathFollower follow)
    {
        Wait = start;
        float fadeTime = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        if ( start && follow is FinalBattle) //TODO: need to find better solution
        {
            final.replaceCamera();
        }
        fadeTime = GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
        if(follow != null)
            follow.Go();
    }
    

    private IEnumerator sendTestData()
    {
        yield return new WaitForSeconds(20);
        Debug.Log("Test data send: " + System.DateTime.Now);
    }

    private void startEpicFinalBattle()
    {
        finalBattle = true;
        final.battleStarted = true;
        GetComponent<PlayerController>().hideWeapon(true);
        Debug.Log("Final battle started");
        StartCoroutine(fading(true, final));

    }

    public void stopEpicFinalBattle()
    {
        StartCoroutine(fading(true, null));
        StartCoroutine(final.zombiesDepart());
        GetComponent<PlayerController>().hideWeapon(false);
        Debug.Log("Lets fight!");
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
    
    public override void Go()
    {
        Wait = false;
        Debug.Log("Camera started in base path");
    }

    
}
