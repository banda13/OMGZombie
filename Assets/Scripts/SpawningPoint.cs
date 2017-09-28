﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPoint : MonoBehaviour {
    
    public RuntimeAnimatorController spawningAnimations;
    public float extraYRotation = 0;
    public bool taken = false;

    private ZombieController zombie = null;

    public ZombieController Spawn(GameObject zombieObject, GameObject player, Transform parent, int index, bool activated)
    {
        taken = true;
        StartCoroutine(waitForLeaving());
        zombie = zombieObject.GetComponent<ZombieController>();
        zombie.GetComponent<Animator>().runtimeAnimatorController = spawningAnimations;
        if (zombie == null)
        {
            Debug.LogError("Wtf it is not zombie..?");
            return null;
        }
        var createdZomie = Instantiate(zombieObject, transform.position, Quaternion.Euler(new Vector3(0,  extraYRotation, 0)));
        createdZomie.transform.parent = parent;
        createdZomie.transform.name = "Zombie " + index;
        zombie = createdZomie.GetComponent<ZombieController>();
        zombie.player = player;
        zombie.zombieIndex = index;
        zombie.enableAttack(activated);
        //try to help the zombie, and add the first detination to him!
        if (transform.childCount > 0)
        {
            zombie.setNextDestination(transform.GetChild(0));
        }
        return zombie;
    }

    IEnumerator waitForLeaving()
    {
        yield return new WaitForSeconds(5.0f);
        taken = false;
    }

    public bool isTaken()
    {
        return taken;
    }

}
