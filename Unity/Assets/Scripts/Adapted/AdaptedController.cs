using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdaptedController : MonoBehaviour {


    [Range(0.0f, 100.0f)]
    public float fearLevel = 50f;

    [Range(0.0f, 100.0f)]
    public float heartRate = 50f;

    [Range(0.0f, 100.0f)]
    public float attantion = 50f;

    private float avgFearLevel = 50f;
    private float avgHeartRate = 50f;
    private float avgAttention = 50f;

    private int fearEventCounter = 0;
    private int heartEventCounter = 0;
    private int attentionEventCounter = 0;
    
    public Slider fearSlider;
    public Slider healtSlider;
    public Slider attentionSlider;
    public GameObject panel;
    public GameObject camera;

    public enum Status
    {
        OFFLINE, UNRESPONSIVE, LOW_BATTERY, BAD_SIGNAL, CONNECTING, ONLINE
    }

    private Status sensorStatus = Status.OFFLINE;
    public Text sensorText;

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

    private bool isPaused = false;

    public float FearLevel
    {
        get { return fearLevel; }
        set
        {
            if (!isPaused)
            {
                fearLevel = value;
                fearEventCounter++;
                avgFearLevel = (avgFearLevel + fearLevel) / fearEventCounter;
                fearSlider.value = (int) Math.Round(avgFearLevel);
                if (value < 30  && lowFearLevel != null)
                {
                    lowFearLevel();
                }
                else if (value > 70  && highFearLevel != null)
                {
                    highFearLevel();
                }
                else if(normalFearLevel != null)
                {
                    normalFearLevel();
                }
            }
        }
    }

    public float HeartRate
    {
        get { return heartRate; }
        set
        {
            if (!isPaused)
            {
                heartRate = value;
                heartEventCounter++;
                avgHeartRate = (avgHeartRate + heartRate) / heartEventCounter;
                healtSlider.value = (int)Math.Round(avgHeartRate);
                if (value < 30 && lowHeartRate != null)
                {
                    lowHeartRate();
                }
                else if (value > 70 && highHeartRate != null)
                {
                    highHeartRate();
                }
                else if(normalHeartRate != null)
                {
                    normalHeartRate();
                }
            }
        }
    }

    public float Attention
    {
        get { return attantion; }
        set
        {
            if (!isPaused)
            {
                attantion = value;
                attentionEventCounter++;
                avgAttention = (avgAttention + attantion) / attentionEventCounter;
                attentionSlider.value = (int) Math.Round(avgAttention);
                if (value < 30 && lowAttention != null)
                {
                    lowAttention();
                }
                else if (value > 70 && highAttention != null)
                {
                    highAttention();
                }
                else if(normalAttention != null)
                {
                    normalAttention();
                }
            }
        }
    }

    public Status SensorStatus
    {
        get { return sensorStatus; }
        set {
            sensorStatus = value;
            sensorText.text = sensorStatus.ToString();
        }
    }

    public void pointerClick()
    {
        if (!isPaused)
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
                AdaptedEventHandler.frequentPointerClick(pointerClickLimit, pointerClicksDetectionDuration);
                if (pointerClickEnumerator != null)
                {
                    StopCoroutine(pointerClickEnumerator);
                    detectingPointerClicks = false;
                    pointerClicks = 0;
                }
            }
        }
    }

    private IEnumerator detectingPlayerClicks()
    {
        yield return new WaitForSeconds(pointerClicksDetectionDuration);
        if(pointerClicks > pointerClickLimit)
        {
            AdaptedEventHandler.frequentPointerClick(pointerClickLimit, pointerClicksDetectionDuration);
        }
        detectingPointerClicks = false;
        pointerClicks = 0;
    }

    void OnPauseGame()
    {
        isPaused = true;
        panel.SetActive(true);
        panel.transform.position = camera.transform.position + camera.transform.forward * 0.5f;
        panel.transform.LookAt(camera.transform.position);
        fearSlider.value = avgFearLevel;
        healtSlider.value = avgHeartRate;
        attentionSlider.value = avgAttention;
    }

    void OnResumeGame()
    {
        isPaused = false;
        panel.SetActive(false);
    }

}
