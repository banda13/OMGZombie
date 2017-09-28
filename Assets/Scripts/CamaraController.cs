using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CamaraController : PathFollower  {

    public bool finalBattle = false;
    private FinalBattle final;

    private EnemyFactory factory;

    void Start () {
        shift = new Vector3(0, 0.5f, 0);
        init();
        final = GetComponent<FinalBattle>();
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
    private IEnumerator fading(bool start)
    {
        float fadeTime = GetComponent<Fading>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        fadeTime = GetComponent<Fading>().BeginFade(-1);
        yield return new WaitForSeconds(fadeTime);
        if (start)
        {
            final.Go(this);
        }
        Wait = start;
    }

    private void startEpicFinalBattle()
    {
        finalBattle = true;
        Debug.Log("Final battle started");
        StartCoroutine(fading(true));

    }

    public void stopEpicFinalBattle()
    {
        StartCoroutine(fading(false));
        Debug.Log("Final battler ended");
    }

    public void activateZombies()
    {
        factory.zombiesAttackActivated = true;
        factory.activeZombiesAttack();
    }
}
