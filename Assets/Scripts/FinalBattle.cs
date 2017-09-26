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

    public void Go(CamaraController camera)
    {
        transform.position = waypoints[currentWaypoint].position;
        Wait = false;
        controller = camera;
        factory.killAll();
        factory.Active = false;
        foreach(Transform t in spawningPoints)
        {
            SpawningPoint p = t.GetComponent<SpawningPoint>();
            if(p != null)
            {
                GameObject zombieObj = factory.selectOneBeautifulZombie();
                ZombieController zombie = zombieObj.GetComponent<ZombieController>();
                if(zombie == null)
                {
                    Debug.LogError("Can't create final battle zombie because no zombie controller attached!");
                    continue;
                }
                //create deaf and blind zombies
                zombie.eyeShot = 0;
                zombie.playerDetectionRange = 0;
                factory.createZombie(p, zombieObj);
            }
        }
        Debug.Log(spawningPoints.Count + " zombies spawed ");
    }
}
