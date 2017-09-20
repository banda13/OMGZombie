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
    
    void Start () {

        spawningPoints = new List<Transform>();

		foreach (Transform point in transform.GetChild(0))
        {
            spawningPoints.Add(point);
        }
        SetRandomTime();
        currentTime = 0;

        setUpZombieVillage();
	}

    void FixedUpdate()
    {
        currentTime += Time.deltaTime;
        
        if (currentTime >= spawnTime)
        {
            List<Transform> nearestPoints = getNearestSpawningpoints();
            if (nearestPoints.Count > 0)
            {
                int spawnIndex = Random.Range(0, nearestPoints.Count);
                nearestPoints[spawnIndex].GetComponent<SpawningPoint>().Spawn(selectOneBeautifulZombie(), player, transform.GetChild(1), zombieCounter);
                Debug.Log("Enemy Spawned at: " + nearestPoints[spawnIndex].name + " Index: " + zombieCounter);
                zombieCounter++;
            }
            currentTime = 0;
            SetRandomTime();
            
        }
    }

    private void setUpZombieVillage()
    {
        for(int i= 0; i<zombiesAtStart; i++)
        {
            int randomPlaceIndex = Random.Range(0, spawningPoints.Count);
            spawningPoints[randomPlaceIndex].GetComponent<SpawningPoint>().Spawn(selectOneBeautifulZombie(), player, transform.GetChild(1), zombieCounter);
            zombieCounter++;
        }
    }

    private GameObject selectOneBeautifulZombie()
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
}
