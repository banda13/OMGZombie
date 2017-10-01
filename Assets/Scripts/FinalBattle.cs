using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBattle : PathFollower {

    private CamaraController controller;
    public EnemyFactory factory;
    public Transform points;

    private List<Transform> spawningPoints;

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
        if(currentWaypoint == waypoints.Count && !Wait)
        {
            Wait = true;
            controller.stopEpicFinalBattle();
        }
	}

    public override void Go()
    {
        transform.position = waypoints[currentWaypoint].position;
        Wait = false;
        controller = transform.root.GetComponent<CamaraController>();
        factory.killAll();
        factory.Active = false;
        foreach(Transform t in spawningPoints)
        {
            SpawningPoint p = t.GetComponent<SpawningPoint>();
            if(p != null)
            {
                GameObject zombieObj = factory.selectOneBeautifulZombie();
                //create deaf and blind zombies
                ZombieController zombie = factory.createZombie(p, zombieObj);
                zombie.eyeShot = 0;
                zombie.playerDetectionRange = 0;
            }
        }
        Debug.Log(spawningPoints.Count + " zombies spawed ");
    }
}
