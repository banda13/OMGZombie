﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CandleToFinalbattle : CandleBase {

    public EnemyFactory factory;

    public override void fadingActions()
    {
        camara.jump("beforeBattle", camara.transform);
        camara.Wait = false;
        factory.Active = true;
        factory.zombiesAttackActivated = true;
        AdaptedEventHandler.uniqueEvent("Jumped to final battle");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            jumpTo();
        }
    }
}
