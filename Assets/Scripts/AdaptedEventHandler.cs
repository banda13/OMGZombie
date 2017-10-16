using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AdaptedEventHandler {

    static AndroidJavaClass jc;
    static AndroidJavaObject currentActivity;
    
    public static void init() {

        if (Application.platform == RuntimePlatform.Android)
        {
            jc = new AndroidJavaClass("com.andy.omg.UnityPlayerActivity");
            currentActivity = jc.GetStatic<AndroidJavaObject>("UnityPlayerActivity");
        }
        Debug.Log("Adapted event handler ready");
    }

    public static void wayPointReached(Vector3 position)
    {
        Debug.Log("Waypoint reached event send! Position " + position);
        try
        {
            currentActivity.Call("wayPointReached", position);
        }
        catch(NullReferenceException e)
        {
            //its ok in editor mode!
        }
    }


}
