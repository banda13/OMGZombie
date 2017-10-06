using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour {

    private List<Transform> spawningPoints;

    public List<GameObject> zombies;

    public float maxTime = 15;
    public float minTime = 5;

    private float spawnTime;
    private float currentTime;

    public GameObject player;

    public int activeSpawningPlaces = 3;
    public int zombiesAtStart = 10;

    private int zombieCounter = 0;
    public int minZombiesAlive = 7;

    public bool Active = false;
    public bool zombiesAttackActivated = false;

    void Start () {

        spawningPoints = new List<Transform>();

		foreach (Transform point in transform.GetChild(0))
        {
            spawningPoints.Add(point);
        }
        SetRandomTime();
        currentTime = 0;

        //setUpZombieVillage();
	}

    void FixedUpdate()
    {
        currentTime += Time.deltaTime;
        if (Active)
        {
            if (currentTime >= spawnTime)
            {
                List<Transform> nearestPoints = getNearestSpawningpoints();
                if (nearestPoints.Count > 0)
                {
                    cleverZombieCreation(nearestPoints);
                }
                currentTime = 0;
                SetRandomTime();

            }

            //dont let to the number of the zombies under a specified number
            if (aliveZombies() < minZombiesAlive)
            {
                int i;
                for (i = 0; i < minZombiesAlive - aliveZombies(); i++)
                {
                    cleverZombieCreation(spawningPoints);
                }
                Debug.Log("Zombies number is under " + minZombiesAlive + " so i created " + i + "zombies");
            }
        }
    }

    public void setUpZombieVillage()
    {
        for(int i= 0; i<zombiesAtStart; i++)
        {
            cleverZombieCreation(spawningPoints);
        }
    }

    public int aliveZombies()
    {
        int count = 0;
        if(transform.GetChild(1).childCount == 0)
        {
            Debug.Log("There is no zombie in the game");
        }
        foreach(Transform zombie in transform.GetChild(1))
        {
            if (!zombie.GetComponent<ZombieController>().isDead())
            {
                count++;
            }
        }
        return count;
    }

    public ZombieController cleverZombieCreation(List<Transform> spawnPoints)
    {
        if (spawnPoints.Count > 0)
        {
            SpawningPoint emptyPoint = null;
            int iterCounter = 0;
            //We try to randomly select an empty point from the 3 nearest spawning points
            //we have a good chance to find it in this way
            while (iterCounter < spawningPoints.Count)
            {
                iterCounter++;
                int spawnIndex = Random.Range(0, spawnPoints.Count);
                if (!spawnPoints[spawnIndex].GetComponent<SpawningPoint>().taken)
                {
                    //we found and empty, yee!
                    emptyPoint = spawnPoints[spawnIndex].GetComponent<SpawningPoint>();
                    ZombieController created = createZombie(emptyPoint, selectOneBeautifulZombie());
                    Debug.Log("Enemy Spawned at: " + spawnPoints[spawnIndex].name + " Index: " + zombieCounter);
                    return created;
                }
            }
            Debug.Log("Zombie creation failed cause all spawning points are taken");
            return null;
        }
        Debug.Log("Zombie creation failed cause spawning points are empty");
        return null;
    }

    public ZombieController createZombie(SpawningPoint pos, GameObject zombie)
    {
        zombieCounter++;
        return pos.Spawn(zombie, player, transform.GetChild(1), zombieCounter, zombiesAttackActivated);
    }
    

    public GameObject selectOneBeautifulZombie()
    {
        if(zombies == null || zombies.Count == 0)
        {
            Debug.Log("I don't know any zombie..");
            return null;
        }
        int zombieIndex = Random.Range(0, zombies.Count);
        return zombies[zombieIndex];
    }

    private void SetRandomTime()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }

    private List<Transform> getNearestSpawningpoints()
    {
        List<Transform> near = new List<Transform>();
        List<Transform> copy = new List<Transform>(spawningPoints);
        for(int i = 0; i< activeSpawningPlaces; i++)
        {
            Transform nearest = getNearest(copy);
            if(nearest == null)
            {
                return near;
            }
            near.Add(nearest);
            copy.Remove(nearest);
        }
        return near;
    }

    private Transform getNearest(List<Transform> origin)
    {
        float dist = -1;
        Transform nearest = null;
        foreach(Transform t in origin)
        {
            if(dist == -1)
            {
                dist = Vector3.Distance(t.position, player.transform.position);
                nearest = t;
            }
            else
            {
                float newDistance = Vector3.Distance(t.position, player.transform.position);
                if (newDistance < dist)
                {
                    nearest = t;
                    dist = newDistance;
                }
            }
        }
        return nearest;
    }

    public void killAll()
    {
        int count = 0;
        foreach(Transform zombie in transform.GetChild(1))
        {
            zombie.GetComponent<ZombieController>().Die();
            count++;
        }
        Debug.Log("I killed " + count + " zombies");
    }

    public void activeZombiesAttack()
    {
        foreach(Transform zombie in transform.GetChild(1))
        {
            zombie.GetComponent<ZombieController>().enableAttack(true);
        }
        Debug.Log("Zombies attack activated!");
    }

    public void clearFactory()
    {
        int count = 0;
        foreach (Transform zombie in transform.GetChild(1))
        {
            Destroy(zombie.gameObject);
            count++;
        }
        Debug.Log("Factory cleard: I deleted " + count + " zombies");
    }
}
