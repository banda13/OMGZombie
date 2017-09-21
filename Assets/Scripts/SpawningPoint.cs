using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPoint : MonoBehaviour {
    
    public RuntimeAnimatorController spawningAnimations;
    public Quaternion rotation = Quaternion.identity;

    private ZombieController zombie = null;

    public void Spawn(GameObject zombieObject, GameObject player, Transform parent, int index)
    {
        zombie = zombieObject.GetComponent<ZombieController>();
        if(zombie == null)
        {
            Debug.LogError("Wtf it is not zombie..?");
            return;
        }
        zombie.player = player;
        zombie.GetComponent<Animator>().runtimeAnimatorController = spawningAnimations;
        zombie.Spawn(index);
        var createdZomie = Instantiate(zombieObject, transform.position, rotation);
        createdZomie.transform.parent = parent;
        createdZomie.transform.name = "Zombie " + index;
    }

}
