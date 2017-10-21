using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FinalBattle : PathFollower {

    private CamaraController controller;
    public EnemyFactory factory;
    public Transform points;
    public GameObject epicPortal;
    public Door door;

    private List<Transform> spawningPoints;

    private List<ZombieController> zombiesInBattle;
    public bool battleStarted = false;
    public bool battleCompleted = false;

    public Transform destination;
    public GameObject detonator;

    void Start () {
        init();
        Wait = true;

        spawningPoints = new List<Transform>();
        
        foreach(Transform t in points.transform)
        {
            spawningPoints.Add(t);
        }
        var rnd = new System.Random();
        spawningPoints = new List<Transform>(spawningPoints.OrderBy(item => rnd.Next()));
    }
	
	void Update () {
        move();
        if(battleStarted && !battleCompleted)
        {
            checkBattleEnded();
        }
        if(currentWaypoint == waypoints.Count && !Wait)
        {
            Wait = true;
            controller.stopEpicFinalBattle();
            
        }
	}

    public override void Go()
    {
        
        Wait = false;
        controller = transform.root.GetComponent<CamaraController>();
        factory.killAll();
        factory.Active = false;
        factory.clearFactory();
        zombiesInBattle = new List<ZombieController>();
        foreach (Transform t in spawningPoints)
        {
            SpawningPoint p = t.GetComponent<SpawningPoint>();
            if (p != null)
            {
                GameObject zombieObj = factory.selectOneBeautifulZombie();
                ZombieController zombie = factory.createZombie(p, zombieObj, instant:false);
                zombie.setNextDestination(destination);
                zombie.speed_animationWalk = 0.0f;
                zombie.speed_animationRun = 0.0f;
                zombiesInBattle.Add(zombie);
            }
        }
        Debug.Log(spawningPoints.Count + " zombies spawed ");
        StartCoroutine(delayedActivation());
    }

    private IEnumerator delayedActivation()
    {
        foreach(ZombieController zombie in zombiesInBattle)
        {
            zombie.GetComponent<Animator>().SetBool("Block", false);
            yield return new WaitForSeconds(0.3f);
        }
    }     

    public IEnumerator zombiesDepart()
    {
        yield return new WaitForSeconds(3);
        Debug.Log("Zombies starting");

        foreach(ZombieController zombie in zombiesInBattle)
        {
            //zombie.playerDetectionRange = 50;
            zombie.speed_animationWalk = 0.4f;
            zombie.speed_animationRun = 0.4f;
            zombie.speed = 0.4f;
            yield return new WaitForSeconds(0.2f);
        }
        //start 30sec timer
        StartCoroutine(countDonw());
    }
    private IEnumerator countDonw()
    {
       yield  return new WaitForSeconds(30);
       //battleCompleted = true;
    }

    private void checkBattleEnded()
    {
        if (zombiesInBattle != null && zombiesInBattle.Count != 0 && factory.aliveZombies() == 0)
        {
            battleCompleted = true;
            StartCoroutine(door.Move());
            epicPortal.SetActive(true);
        }
    }

    public override void replaceCamera()
    {
        transform.position = waypoints[currentWaypoint].position;
    }
}
