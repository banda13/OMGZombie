using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicZombie : Zombie {

    public bool spawned = false;

    void Start()
    {
        init();
    }

    void Update()
    {
        /*if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.runtimeAnimatorController = Resources.Load(Paths.normalBehaviorController) as RuntimeAnimatorController;
            spawned = true;
        }*/
        //dont do anthing while the zombie don't start the basic behavior and movement
        if (!spawned)
        {
            return;
        }


        if (isPlayerNeerOrWeSawHim())
        {
            attackPlayer();
        }
        else if (state != States.walking)
        {
            startWalking();
        }

        if(state == States.walking)
        {
                if (targetReached)
                {
                    chooseDestination(null);
                }
                else
                {
                    if (!rotateToDirection(100))
                    {
                        goToDestination();
                    }
                }
        }
    }
}
