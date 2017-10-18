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
        factory.Active = true;
        factory.setUpZombieVillage();
        misson.Wait = true;


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            jumpTo();
        }
    }
}
