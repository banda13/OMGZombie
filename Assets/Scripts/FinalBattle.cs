using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBattle : PathFollower {

    private CamaraController controller;
    public EnemyFactory factory;
    public Transform points;
    public GameObject epicPortal;

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
            epicPortal.SetActive(true);
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
                //create deaf and blind zombies
                ZombieController zombie = factory.createZombie(p, zombieObj);
                zombie.eyeShot = 0;
                zombie.playerDetectionRange = 0;
                zombie.setNextDestination(destination);
                zombie.directionChange = 120; //if stucked
                zombie.speed_animationWalk = 0;
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
            zombie.speed_animationWalk = 0.6f;
            zombie.speed = 0.6f;
        }
    }

    private void checkBattleEnded()
    {
        if (zombiesInBattle != null && zombiesInBattle.Count != 0 && factory.aliveZombies() == 0)
        {
            battleCompleted = true;
        }
    }

    public override void replaceCamera()
    {
        transform.position = waypoints[currentWaypoint].position;
    }
}
