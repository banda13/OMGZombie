using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WomenZombie : Zombie {

    private bool jumpedDown = false;

	void Start () {
        init();
	}
    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            jumpedDown = true;
            animator.runtimeAnimatorController = Resources.Load(Paths.normalBehaviorController) as RuntimeAnimatorController;
        }

        if (isPlayerNeerOrWeSawHim())
        {
            attackPlayer();
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && state != States.walking)
        {
            startWalking();
        }
        else if (state == States.walking)
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
