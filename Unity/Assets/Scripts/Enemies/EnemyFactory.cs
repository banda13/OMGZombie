﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour {

    private List<Transform> spawningPoints;
    private List<Transform> fixedZombies;

    public List<GameObject> zombies;

    public float maxTime = 15;
    public float minTime = 5;

    private float spawnTime;
    private float currentTime;

    public GameObject player;

    public int activeSpawningPlaces = 3;
    public int zombiesAtStart = 10;

    private int zombieCounter = 0;
    public int minZombiesAliveAtLowFearLevel = 7;
    public int maxZombiesAliveAtLowFearLevel = 9;
    public int minZombiesAliveAtNormalFearLevel = 5;
    public int maxZombiesAliveAtNormalFearLevel = 7;
    public int minZombiesAliveAtHightFearLevel = 3;
    public int maxZombiesAliveAtHightFearLevel = 5;
    public int minZombiesAlive = 3;
    public int maxZombiesAlive = 5;

    public bool Active = false;
    public bool zombiesAttackActivated = false;
    private bool isPaused = false;

    void Start () {

        spawningPoints = new List<Transform>();
        fixedZombies = new List<Transform>();

		foreach (Transform point in transform.GetChild(0))
        {
            spawningPoints.Add(point);
        }
        foreach(Transform p in transform.GetChild(2))
        {
            fixedZombies.Add(p);
        }
        SetRandomTime();
        currentTime = 0;

        AdaptedController.lowFearLevel += syncZombiesToLowFearLevel;
        AdaptedController.normalFearLevel += syncZombiesToNormalFearLevel;
        AdaptedController.highFearLevel += syncZombiesToHighFearLevel;

        setUpZombiesEnviroment();
    }

    void FixedUpdate()
    {
        currentTime += Time.deltaTime;
        if (Active && !isPaused)
        {
            if (currentTime >= spawnTime && aliveZombies() < maxZombiesAlive)
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

    private void syncZombiesToLowFearLevel()
    {
        minZombiesAlive = minZombiesAliveAtLowFearLevel;
        maxZombiesAlive = maxZombiesAliveAtLowFearLevel;
    }

    private void syncZombiesToNormalFearLevel()
    {
        minZombiesAlive = minZombiesAliveAtNormalFearLevel;
        maxZombiesAlive = maxZombiesAliveAtNormalFearLevel;
    }

    private void syncZombiesToHighFearLevel()
    {
        minZombiesAlive = minZombiesAliveAtHightFearLevel;
        maxZombiesAlive = maxZombiesAliveAtHightFearLevel;
    }

    public void setUpZombieVillage()
    {
        for(int i= 0; i<zombiesAtStart; i++)
        {
            cleverZombieCreation(spawningPoints);
        }
    }

    private void setUpZombiesEnviroment()
    {
        foreach(Transform t in fixedZombies)
        {
            createZombie(t.GetComponent<SpawningPoint>(), selectOneBeautifulZombie(), instant: false);
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
            if (zombie.GetComponent<ZombieController>() != null && !zombie.GetComponent<ZombieController>().isDead())
            {
                count++;
            }
        }
        return count;
    }

    public ZombieController cleverZombieCreation(List<Transform> nearestPoints)
    {
        if (nearestPoints.Count > 0)
        {
            SpawningPoint emptyPoint = null;
            int iterCounter = 0;
            //We try to randomly select an empty point from the 3 nearest spawning points
            //we have a good chance to find it in this way
            while (iterCounter < spawningPoints.Count)
            {
                iterCounter++;
                int spawnIndex = Random.Range(0, nearestPoints.Count);
                if (!nearestPoints[spawnIndex].GetComponent<SpawningPoint>().taken)
                {
                    //we found and empty, yee!
                    emptyPoint = nearestPoints[spawnIndex].GetComponent<SpawningPoint>();
                    ZombieController created = createZombie(emptyPoint, selectOneBeautifulZombie());
                    Debug.Log("Enemy Spawned at: " + nearestPoints[spawnIndex].name + " Index: " + zombieCounter);
                    return created;
                }
            }
            Debug.Log("Zombie creation failed cause all spawning points are taken");
            return null;
        }
        Debug.Log("Zombie creation failed cause spawning points are empty");
        return null;
    }

    public ZombieController createZombie(SpawningPoint pos, GameObject zombie, bool instant = true)
    {
        zombieCounter++;
        Debug.Log("instant: " + instant);
        return pos.Spawn(zombie, player, transform.GetChild(1), zombieCounter, zombiesAttackActivated, instantSpawn:instant);
    }


    public GameObject selectOneBeautifulZombie()
    {
        if (zombies == null || zombies.Count == 0)
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
                dist = Vector3.Distance(t.position, player.GetComponent<CamaraController>().getNextDestination().position);
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
            if (zombie.GetComponent<ZombieController>() != null)
            {
                StartCoroutine(zombie.GetComponent<ZombieController>().Die());
                count++;
            }
        }
        Debug.Log("I killed " + count + " zombies");
    }

    public void activeZombiesAttack()
    {
        foreach(Transform zombie in transform.GetChild(1))
        {
            if (zombie.GetComponent<ZombieController>())
            {
                zombie.GetComponent<ZombieController>().enableAttack(true);
            }
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
