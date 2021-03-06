﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class AdaptedEventHandler : MonoBehaviour{

    static AndroidJavaClass jc;
    static AndroidJavaObject currentActivity;

    public static AdaptedController fear;
    public static Timer timer = new Timer();

    void Start()
    {
        fear = GameObject.Find("AdaptedEventHandler").GetComponent<AdaptedController>();
        //StartCoroutine(simulateAttentionLevel());
        //StartCoroutine(simulateFearLevel());
        //StartCoroutine(simulateHeartLevel());
        //StartCoroutine(simulateSensor());
    }

    public static void wayPointReached(Vector3 position, string waypointName)
    {
        Debug.Log("Sending AdaptedEvent"+ waypointName + " reached at position " + position);
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            currentActivity.Call("wayPointReached", new object[] { position.ToString(), waypointName});
        } catch(NullReferenceException e)
        {
            Debug.Log("Currentactivity null" + e);
        }
    }

    public static void jumpScareVoice(string jumpScareName)
    {
        Debug.Log("Sending AdaptedEvent: jumpscare: " + jumpScareName);
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            currentActivity.Call("jumpScareVoice", new object[] { jumpScareName});
        } catch(NullReferenceException e)
        {
            Debug.Log("Currentactivity null" + e);

        }
    }

    public static void playerDamaged(float healtLoss, float healtLeft)
    {
        Debug.Log("Sending AdaptedEvent: Player healt change from " + healtLeft + healtLoss + " to " + healtLeft);
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            currentActivity.Call("playerDamaged", new object[] { healtLoss, healtLeft});
        } catch(NullReferenceException e)
        {
            Debug.Log("Currentactivity null" + e);
        }
    }

    public static void playerDead(Vector3 position, string waypointName)
    {
        Debug.Log("Sending AdaptedEvent: player died at " + waypointName);
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            currentActivity.Call("playerDead", position, new object[] { position.ToString(), waypointName});
        } catch(NullReferenceException e)
        {
            Debug.Log("Currentactivity null" + e);

        }
    }

    public static void drawingFault(string direction, string shapeForm)
    {
        Debug.Log("Sending AdaptedEvent: drawing fault to " + direction + " in shape " + shapeForm);
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            currentActivity.Call("drawingFault", new object[] { direction, shapeForm});
        } catch(NullReferenceException e)
        {
            Debug.Log("Currentactivity null" + e);

        }
    }

    public static void missionStarted(string missionName)
    {
        Debug.Log("Sending AdaptedEvent: " + missionName + " started");
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            currentActivity.Call("missionStarted", new object[] { missionName});
        } catch(NullReferenceException e)
        {
            Debug.Log("Currentactivity null" + e);

        }
    }

    public static void missionCompleted(string missionName)
    {
        Debug.Log("Sending AdaptedEvent: " + missionName + " completed");
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            currentActivity.Call("missionCompleted", new object[] { missionName});
        } catch(NullReferenceException e)
        {
            Debug.Log("Currentactivity null" + e);

        }
    }

    public static void missionFailed(string missionName, int zombiesKilled)
    {
        Debug.Log("Sending AdaptedEvent: " + missionName + " failed, killed" + zombiesKilled + " zombies");
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            currentActivity.Call("missionFailed",new object[] { missionName, zombiesKilled});
        } catch(NullReferenceException e)
        {
            Debug.Log("Currentactivity null" + e);

        }
    }

    public static void frequentPointerClick(int limit, int duration)
    {
        Debug.Log("Sending AdaptedEvent: frequent pointer click");
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            currentActivity.Call("frequentPointerClick", new object[] {limit, duration });
        } catch(NullReferenceException e)
        {
            Debug.Log("Currentactivity null" + e);

        }
    }

    public static void uniqueEvent(string eventName)
    {
        Debug.Log("Sending AdaptedEvent: " + eventName);
        try
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            }
            currentActivity.Call("uniqueEvent", new object[] { eventName});
        } catch(NullReferenceException e)
        {
            Debug.Log("Currentactivity null" + e);

        }
    }

    public void setFearLevel(string fearLevel)
    {
        fear.FearLevel = float.Parse(fearLevel);
    }

    public void setHeartRate(string heartRate)
    {
        fear.HeartRate = float.Parse(heartRate);
    }

    public void setAttention(string attention)
    {
        fear.Attention = float.Parse(attention);
    }

    public void setSensorStatus(string status)
    {
        fear.SensorStatus = (AdaptedController.Status)Enum.Parse(typeof(AdaptedController.Status), status);
    }

    //for testing from Unity
    private IEnumerator simulateFearLevel()
    {
        System.Random r = new System.Random(System.DateTime.Now.Millisecond);
        setFearLevel(r.Next(0, 100).ToString());
        yield return new WaitForSeconds(r.Next(2, 20));
        StartCoroutine(simulateFearLevel());
    }

    private IEnumerator simulateAttentionLevel()
    {
        System.Random r = new System.Random(System.DateTime.Now.Millisecond);
        setAttention(r.Next(0, 100).ToString());
        yield return new WaitForSeconds(r.Next(2, 20));
        StartCoroutine(simulateAttentionLevel());
    }

    private IEnumerator simulateHeartLevel()
    {
        System.Random r = new System.Random(System.DateTime.Now.Millisecond);
        setHeartRate(r.Next(0, 100).ToString());
        yield return new WaitForSeconds(r.Next(2, 20));
        StartCoroutine(simulateHeartLevel());
    }

    private IEnumerator simulateSensor()
    {
        System.Random r = new System.Random(System.DateTime.Now.Millisecond);
        setSensorStatus(((AdaptedController.Status)r.Next(0, 5)).ToString());
        yield return new WaitForSeconds(r.Next(1, 5));
        StartCoroutine(simulateSensor());
    }
}
