using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CamaraController : PathFollower  {

    public bool finalBattle = false;
    private bool sniperMission = false;
    private FinalBattle final;
    private SnippingMission snipping;

    private EnemyFactory factory;

    void Start () {
        shift = new Vector3(0, 0.5f, 0);
        init();
        final = GetComponent<FinalBattle>();
        snipping = GetComponent<SnippingMission>();
        factory = final.factory;
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
        sniperMission = true;
        StartCoroutine(fading(true, snipping));
    }

    public void stopSnipeMission()
    {
        sniperMission = false;
        StartCoroutine(fading(true, null));
    }

    //dont need to override this , because it isn't a mission 
    public override void Go()
    {
        throw new NotImplementedException();
    }
}
