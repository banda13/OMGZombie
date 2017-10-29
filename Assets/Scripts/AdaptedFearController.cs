using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptedFearController : MonoBehaviour {


    [Range(0.0f, 100.0f)]
    public float fearLevel = 50f;

    [Range(0.0f, 100.0f)]
    public float heartRate = 50f;

    [Range(0.0f, 100.0f)]
    public float attantion = 50f;

    public delegate void fearLevelChange();
    public static event fearLevelChange lowFearLevel;
    public static event fearLevelChange normalFearLevel;
    public static event fearLevelChange highFearLevel;

    public delegate void heartRateChanged();
    public static event heartRateChanged lowHeartRate;
    public static event heartRateChanged normalHeartRate;
    public static event heartRateChanged highHeartRate;

    public delegate void attantionChanged();
    public static event attantionChanged lowAttention;
    public static event attantionChanged normalAttention;
    public static event attantionChanged highAttention;

    private int pointerClicks = 0;
    private bool detectingPointerClicks = false;
    public int pointerClicksDetectionDuration = 2; //seconds
    public int pointerClickLimit = 5;
    private IEnumerator pointerClickEnumerator;

    void Start()
    {
        AdaptedEventHandler.init(this);
    }

    public float FearLevel
    {
        get { return fearLevel; }
        set
        {

            //Debug.Log("Fear level change from " + fearLevel + " to " + value);
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

    public float HeartRate
    {
        get { return heartRate; }
        set
        {
            heartRate = value;
            if (value < 30)
            {
                lowHeartRate();
            }
            else if (value > 70)
            {
                highHeartRate();
            }
            else
            {
                normalHeartRate();
            }
        }
    }

    public float Attention
    {
        get { return attantion; }
        set
        {
            attantion = value;
            if (value < 30)
            {
                lowAttention();
            }
            else if (value > 70)
            {
                highAttention();
            }
            else
            {
                normalAttention();
            }
        }
    }

    public void pointerClick()
    {
        if (detectingPointerClicks)
        {
            pointerClicks++;
        }
        else
        {
            pointerClicks++;
            detectingPointerClicks = true;
            pointerClickEnumerator = detectingPlayerClicks();
            StartCoroutine(pointerClickEnumerator);
        }
        if (pointerClicks > pointerClickLimit)
        {
            AdaptedEventHandler.frequentPointerClick();
            if(pointerClickEnumerator != null)
            {
                StopCoroutine(pointerClickEnumerator);
                detectingPointerClicks = false;
                pointerClicks = 0;
            }
        }

    }

    private IEnumerator detectingPlayerClicks()
    {
        yield return new WaitForSeconds(pointerClicksDetectionDuration);
        if(pointerClicks > pointerClickLimit)
        {
            AdaptedEventHandler.frequentPointerClick();
        }
        detectingPointerClicks = false;
        pointerClicks = 0;
    }

}
