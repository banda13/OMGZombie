using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleToSnipping : CandleBase {

    public ChestController chest;

    public override void fadingActions()
    {
        chest.missionStarted = true;
        chest.missionProgress = true;
        camara.jump("startFight", camara.transform);
        chest.startMission();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            jumpTo();
        }
    }
}
