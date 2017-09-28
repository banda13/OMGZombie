using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPoint : MonoBehaviour {
    
    public RuntimeAnimatorController spawningAnimations;
    public float extraYRotation = 0;
    public bool taken = false;

    private ZombieController zombie = null;

    public void Spawn(GameObject zombieObject, GameObject player, Transform parent, int index)
    {
        taken = true;
        StartCoroutine(waitForLeaving());
        zombie = zombieObject.GetComponent<ZombieController>();
        zombie.GetComponent<Animator>().runtimeAnimatorController = spawningAnimations;
        if (zombie == null)
        {
            Debug.LogError("Wtf it is not zombie..?");
            return;
        }
        Vector3 look = transform.GetChild(0).position;
        var createdZomie = Instantiate(zombieObject, transform.position, Quaternion.LookRotation(new Vector3(look.x, look.y + extraYRotation, 0)));
        createdZomie.transform.parent = parent;
        createdZomie.transform.name = "Zombie " + index;
        zombie = createdZomie.GetComponent<ZombieController>();
        zombie.player = player;
        zombie.zombieIndex = index;
        //try to help the zombie, and add the first detination to him!
        if (transform.childCount > 0)
        {
            zombie.setNextDestination(transform.GetChild(0));
        }
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
