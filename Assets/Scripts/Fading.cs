using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour {
    
    public Image fadeingImage;
    public float fadeSpeed = 1.0f;
    private float alpha = 1.0f;
	

    private void fadeIn()
    {
        fadeingImage.CrossFadeAlpha(1.0f, fadeSpeed, true);
    }

    private void fadeOut()
    {
        fadeingImage.CrossFadeAlpha(0f, fadeSpeed, true);
    }

    public float BeginFade(int direction)
    {
        if(direction > 0)
        {
            fadeIn();
        }
        else
        {
            fadeOut();
        }
        return fadeSpeed;
    }



}
