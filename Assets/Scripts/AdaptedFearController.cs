using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptedFearController : MonoBehaviour {


    [Range(0.0f, 100.0f)]
    public float fearLevel = 50f;

    public delegate void fearLevelChange();
    public static event fearLevelChange lowFearLevel;
    public static event fearLevelChange normalFearLevel;
    public static event fearLevelChange highFearLevel;
    
    public float FearLevel
    {
        get { return fearLevel; }
        set
        {

            Debug.Log("Fear level change from " + fearLevel + " to " + value);
            fearLevel = value;
            if (value < 30)
            {
                lowFearLevel();
            }
            else if (value > 70)
            {
                highFearLevel();
            }
            else
            {
                normalFearLevel();
            }

        }
    }
}
