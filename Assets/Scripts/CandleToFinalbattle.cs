using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CandleToFinalbattle : CandleBase {
    

    public override void fadingActions()
    {
        camara.jump("beforeBattle", camara.transform);
        camara.Wait = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            jumpTo();
        }
    }
}
