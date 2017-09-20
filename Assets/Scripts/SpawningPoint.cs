using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPoint : MonoBehaviour {
    
    public RuntimeAnimatorController spawningAnimations;
    public Quaternion rotation = Quaternion.identity;

    private Zombie zombie = null;

    public void Spawn(GameObject zombieObject, GameObject player, Transform parent, int index)
    {
        zombie = zombieObject.GetComponent<Zombie>();
        if(zombie == null)
        {
            Debug.LogError("Wtf it is not zombie..?");
            return;
        }
        zombie.Player = player;
        zombie.GetComponent<Animator>().runtimeAnimatorController = spawningAnimations;
        zombie.number = index;
        var createdZomie = Instantiate(zombieObject, transform.position, rotation);
        createdZomie.transform.parent = parent;
        createdZomie.transform.name = "Zombie " + index;
    }

}
