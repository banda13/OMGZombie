using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CandleToMine : CandleBase {

    public override void fadingActions()
    {
        AdaptedEventHandler.uniqueEvent("Jumped to Mine");
        SceneManager.LoadScene("mine", LoadSceneMode.Single);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            jumpTo();
        }
    }
}
