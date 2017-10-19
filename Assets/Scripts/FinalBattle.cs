using System.Collections;
using System.Collections.Generic;
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

    void Start () {
        init();
        Wait = true;

        spawningPoints = new List<Transform>();

        foreach (Transform point in points.transform)
        {
            spawningPoints.Add(point);
        }
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
            if(p != null)
            {
                GameObject zombieObj = factory.selectOneBeautifulZombie();
                ZombieController zombie = factory.createZombie(p, zombieObj);
                zombie.setNextDestination(destination);
                //zombie.directionChange = 120; //if stucked
                zombie.speed_animationWalk = 0.0f;
                zombie.speed_animationRun = 0.0f;
                zombiesInBattle.Add(zombie);
            }
        }
        Debug.Log(spawningPoints.Count + " zombies spawed ");
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
