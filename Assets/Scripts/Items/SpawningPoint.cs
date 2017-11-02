using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPoint : MonoBehaviour {
    
    public RuntimeAnimatorController spawningAnimations;
    public float extraYRotation = 0;
    public bool taken = false;
    public bool mineMap = false;

    private ZombieController zombie = null;

    public ZombieController Spawn(GameObject zombieObject, GameObject player, Transform parent, int index, bool activated, bool instantSpawn = true)
    {
        taken = true;
        StartCoroutine(waitForLeaving());
        zombie = zombieObject.GetComponent<ZombieController>();

        //set the spawning animator which specify how the zombie spawns
        //the zombie will change it after he spawned
        if (zombie != null) { 
            zombie.GetComponent<Animator>().runtimeAnimatorController = spawningAnimations;
        }
        var createdZomie = Instantiate(zombieObject, transform.position, Quaternion.Euler(new Vector3(0, extraYRotation, 0)));
        createdZomie.transform.parent = parent;
        createdZomie.transform.name = "Zombie " + index;
        if (standingZombies(createdZomie))
        {
            ZombieStandingBehavior fixedZombie = createdZomie.GetComponent<ZombieStandingBehavior>();
            fixedZombie.player = player;
            fixedZombie.animatorTriggers = new List<string>();
            fixedZombie.animatorTriggers.Add("scream");
            fixedZombie.animatorTriggers.Add("attack");
            if (mineMap)
            {
                fixedZombie.activationDistence = 20;
                fixedZombie.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
        }
        else
        {
            zombie = createdZomie.GetComponent<ZombieController>();
            zombie.player = player;
            zombie.zombieIndex = index;
            zombie.enableAttack(activated);

            //try to help the zombie, and add the first detination to him!
            if (transform.childCount > 0)
            {
                zombie.setNextDestination(transform.GetChild(0));
            }
        }
        createdZomie.GetComponent<Animator>().SetBool("Block", !instantSpawn);
        
        return zombie;
    }
    IEnumerator waitForLeaving()
    {
        yield return new WaitForSeconds(10.0f); //10sec spawning time
        taken = false;
    }

    public bool isTaken()
    {
        return taken;
    }

    private bool standingZombies(GameObject zombie)
    {
        try
        {
            if (transform.name.Contains("fixedPoint"))
            {
                DestroyImmediate(zombie.GetComponent<ZombieController>(), true);
                zombie.AddComponent<ZombieStandingBehavior>();
                return true;
            }
        } catch (Exception e)
        {
            Debug.Log("Cant change the zombie controller script, because it isnt exist");
            return false;
        }
        return false;
    }

}
