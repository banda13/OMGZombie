using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleToSnipping : CandleBase {

    public ChestController chest;

    public override void fadingActions()
    {
        camara.jump("startFight", camara.transform);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            jumpTo();
        }
    }
}
