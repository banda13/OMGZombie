using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleToWalking : CandleBase {

    public ChestController chest;
    public EnemyFactory factory;
    public SnippingMission misson;

    public override void fadingActions()
    {

        camara.jump("startFight", camara.transform);
        chest.missionCompleted = true;
        chest.missionStarted = true;
        if (factory.Active == false)
        {
            factory.Active = true;
            factory.setUpZombieVillage();
        }
        misson.Wait = true;
        StartCoroutine(misson.putDownTheGun());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            jumpTo();
        }
    }
}
