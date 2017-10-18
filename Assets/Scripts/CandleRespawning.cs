using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleRespawning : CandleToWalking {

    public PlayerController player;

    public override void fadingActions()
    {
        StartCoroutine(Undying());
        Debug.Log("action child child");
        base.fadingActions();
        player.currentHealth = 100;
        player.isDead = false;
        factory.killAll();
        factory.clearFactory();
        factory.setUpZombieVillage();
       
    }

    private IEnumerator Undying()
    {
        for (int i = 1; i < 90; i++)
        {
            player.transform.Rotate(new Vector3(1, 0, 0));
            yield return null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6))
        {
            jumpTo();
        }
    }

    //nyehehe soma voltam XD bátya
}
