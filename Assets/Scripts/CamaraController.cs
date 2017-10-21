using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        AdaptedEventHandler.init();
    }

    void Update()
    {
        //transform.GetChild(0).GetComponent<Camera>().fieldOfView = 20; ->not working
        rotateTheWord();
        try
        {
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
                    StartCoroutine(delayedGo(2));
                }
            }

            if (Input.GetKeyDown("space"))
            {
                factory.killAll();
            }

            move();
        }
        catch(Exception e)
        {
            //find better solution to index out of range
        }
    }

    private void rotateTheWord()
    {
        Transform cameraPos = transform.GetChild(0);
        transform.GetChild(2).transform.position = new Vector3(cameraPos.position.x, cameraPos.position.y, cameraPos.position.z) + (cameraPos.forward) * 3.5f;
        transform.GetChild(2).transform.LookAt(cameraPos);

        transform.GetChild(4).transform.position = new Vector3(cameraPos.position.x, cameraPos.position.y, cameraPos.position.z) + (cameraPos.forward) * 0.1f;
        transform.GetChild(4).transform.LookAt(cameraPos);
    }

    private IEnumerator startFading()
    {
        float fadeTime = GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
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

    public IEnumerator fadingWithAction(CandleBase.actionBetweenFadeing f)
    {
        float fadeTime = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        f();
        fadeTime = GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
    }

    public IEnumerator fadingWithSniperAction(SnippingMission.fadingActions f)
    {
        float fadeTime = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        f();
        fadeTime = GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
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
        final.detonator.SetActive(true);
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
        StartCoroutine(fading(true, null));
    }
    
    public override void Go()
    {
        Wait = false;
        Debug.Log("Camera started in base path");
    }

    public IEnumerator delayedGo(float delay)
    {
        yield return new WaitForSeconds(delay);
        Wait = false;
    }
    

    public void jump(string destination, Transform target)
    {
        int index = 0;
        foreach(Transform t in waypoints)
        {
            if (t.name.Contains(destination))
            {
                currentWaypoint = index;
                target.position = waypoints[currentWaypoint - 1].position;
            }
            index++;
        }
    }


}
