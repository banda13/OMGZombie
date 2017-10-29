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

        if (Application.platform == RuntimePlatform.Android && (jc == null || currentActivity == null))
        {
            jc = new AndroidJavaClass("com.andy.omg.UnityPlayerActivity");
            currentActivity = jc.GetStatic<AndroidJavaObject>("UnityPlayerActivity");
        }

        fear = f;
        //simulateFearFromUnity();
        Debug.Log("Adapted event handler ready");

    }

    public static void wayPointReached(Vector3 position, string waypointName)
    {
        Debug.Log("Sending AdaptedEvent"+ waypointName + " reached at position " + position);
        try
        {
            currentActivity.Call("wayPointReached", position, waypointName);
        } catch(NullReferenceException e)
        {
            //its ok in editor
        }
    }

    public static void jumpScareVoice(string jumpScareName)
    {
        Debug.Log("Sending AdaptedEvent: jumpscare: " + jumpScareName);
        try
        {
            currentActivity.Call("jumpScareVoice", jumpScareName);
        } catch(NullReferenceException e)
        {

        }
    }

    public static void playerDamaged(float healtLoss, float healtLeft)
    {
        Debug.Log("Sending AdaptedEvent: Player healt change from " + healtLeft + healtLoss + " to " + healtLeft);
        try
        {
            currentActivity.Call("playerDamaged", healtLoss, healtLeft);
        } catch(NullReferenceException e)
        {

        }
    }

    public static void playerDead(Vector3 position, string waypointName)
    {
        Debug.Log("Sending AdaptedEvent: player died at " + waypointName);
        try
        {
            currentActivity.Call("playerDead", position, waypointName);
        } catch(NullReferenceException e)
        {

        }
    }

    public static void drawingFault(string direction, string shapeForm)
    {
        Debug.Log("Sending AdaptedEvent: drawing fault to " + direction + " in shape " + shapeForm);
        try
        {
            currentActivity.Call("drawingFault", direction, shapeForm);
        } catch(NullReferenceException e)
        {

        }
    }

    public static void missionStarted(string missionName)
    {
        Debug.Log("Sending AdaptedEvent: " + missionName + " started");
        try
        {
            currentActivity.Call("missionStarted", missionName);
        } catch(NullReferenceException e)
        {

        }
    }

    public static void missionCompleted(string missionName)
    {
        Debug.Log("Sending AdaptedEvent: " + missionName + " completed");
        try
        {
            currentActivity.Call("missionCompleted", missionName);
        } catch(NullReferenceException e)
        {

        }
    }

    public static void missionFailed(string missionName, int zombiesKilled)
    {
        Debug.Log("Sending AdaptedEvent: " + missionName + " failed, killed" + zombiesKilled + " zombies");
        try
        {
            currentActivity.Call("missionFailed", missionName, zombiesKilled);
        } catch(NullReferenceException e)
        {

        }
    }

    public static void frequentPointerClick()
    {
        Debug.Log("Sending AdaptedEvent: frequent pointer click");
        try
        {
            currentActivity.Call("frequentPointerClick");
        } catch(NullReferenceException e)
        {

        }
    }

    public static void uniqueEvent(string eventName)
    {
        Debug.Log("Sending AdaptedEvent: " + eventName);
        try
        {
            currentActivity.Call("uniqueEvent", eventName);
        } catch(NullReferenceException e)
        {

        }
    }

    public static void setFearLevel(float fearLevel)
    {
        fear.FearLevel = fearLevel;
    }

    public static void setHeartRate(float heartRate)
    {
        fear.HeartRate = heartRate;
    }

    public static void setAttention(float attention)
    {
        fear.Attention = attention;
    }

    private static void simulateFearFromUnity()
    {
        timer.Interval = 5000;
        timer.Elapsed += OnTimedEvent;
        timer.AutoReset = true;
        timer.Enabled = true;
        timer.Start();
    }

    private static void OnTimedEvent(System.Object source, System.Timers.ElapsedEventArgs e)
    {
        System.Random r = new System.Random();
        setFearLevel(r.Next(0, 100));
    }
}
