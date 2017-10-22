using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public static class AdaptedEventHandler {

    static AndroidJavaClass jc;
    static AndroidJavaObject currentActivity;

    public static AdaptedFearController fear;
    public static Timer timer = new Timer();

    public static void init(AdaptedFearController f) {

        if (Application.platform == RuntimePlatform.Android)
        {
            jc = new AndroidJavaClass("com.andy.omg.UnityPlayerActivity");
            currentActivity = jc.GetStatic<AndroidJavaObject>("UnityPlayerActivity");
        }

        fear = f;
        simulateFearFromUnity();
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

    public static void setFearLevel(float fearLevel)
    {
        fear.FearLevel = fearLevel;
    }

    private static void simulateFearFromUnity()
    {
        timer.Interval = 3000;
        timer.Elapsed += OnTimedEvent;
        timer.AutoReset = true;
        timer.Enabled = true;
        timer.Start();
    }

    private static void OnTimedEvent(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        Debug.Log("Tick");
        System.Random r = new System.Random();
        setFearLevel(r.Next(0, 100));
    }
}
