using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleRespawning : CandleToWalking {

    public PlayerController player;

    public override void fadingActions()
    {
        StartCoroutine(Undying());
        base.fadingActions();
        player.currentHealth = 100;
        player.isDead = false;
        factory.killAll();
        factory.clearFactory();
        factory.setUpZombieVillage();
        camara.Wait = false;
    }

    private IEnumerator Undying()
    {
        for (int i = 1; i < 90; i++)
        {
            player.transform.Rotate(new Vector3(1, 0, 0));
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 0.01f, player.transform.position.z);
            yield return null;
        }
    }

    public IEnumerator delayedJump(float wait)
    {
        yield return new WaitForSeconds(wait);
        jumpTo();
    }

}
