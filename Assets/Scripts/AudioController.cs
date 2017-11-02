using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

    public AudioSource snipperAudio;
    public AudioSource finalAudio;
    public AudioSource lowAudio;
    public AudioSource midAudio;
    public AudioSource fastAudio;

    public AudioSource currentAudio;

    public List<AudioSource> jumpScarAudios;

    public float minScaringDuration = 5;
    public float maxScaringDuration = 20;
    public float minScareAtLowAttention = 5;
    public float maxScareAtLowAttention = 20;
    public float minScareAtNormalAttention = 8;
    public float maxScareAtNormalAttention = 30;
    public float minScareAtHighAttention = 10;
    public float maxScareAtHighAttention = 40;

    public enum fearLevel { low, normal, high};
    public fearLevel fear;

    public bool isSnippingMission = false;
    public bool isFinalBattle = false;
    private bool dead = false;

    public float volume = 0.08f;

	void Start () {
        StartCoroutine(jumpScare());

        AdaptedFearController.lowFearLevel += lowFearLevel;
        AdaptedFearController.normalFearLevel += midFearLevel;
        AdaptedFearController.highFearLevel += highFearLevel;

        AdaptedFearController.lowAttention += lowAttention;
        AdaptedFearController.normalAttention += normalAttention;
        AdaptedFearController.highAttention += highAttention;

        fear = fearLevel.normal; //default
        currentAudio = midAudio;
        stepAudio(midAudio);
	}
	
	void Update () {
        if (!dead)
        {
            if (isSnippingMission && currentAudio != snipperAudio)
            {
                stepAudio(snipperAudio);
            }
            else if (isFinalBattle && currentAudio != finalAudio)
            {
                stepAudio(finalAudio);
            }
            else
            {
                if (fear == fearLevel.low && currentAudio != lowAudio)
                {
                    stepAudio(lowAudio);
                }
                else if (fear == fearLevel.normal && currentAudio != midAudio)
                {
                    stepAudio(midAudio);
                }
                else if (fear == fearLevel.high && currentAudio != fastAudio)
                {
                    stepAudio(fastAudio);
                }
            }
        }
    }
    
    private void stepAudio(AudioSource next)
    {
        if (currentAudio.isPlaying)
            currentAudio.Stop();
        currentAudio = next;
        currentAudio.volume = volume;
        currentAudio.Play();
    }

    private IEnumerator jumpScare()
    {
        yield return new WaitForSeconds(Random.Range(minScaringDuration, maxScaringDuration));
        AudioSource audio = jumpScarAudios[Random.Range(0, jumpScarAudios.Count)];
        audio.Play();
        AdaptedEventHandler.jumpScareVoice(audio.clip.name);
        StartCoroutine(jumpScare());
    }

    private void lowFearLevel()
    {
        fear = fearLevel.low;
    }

    private void midFearLevel()
    {
        fear = fearLevel.normal;
    }

    private void highFearLevel()
    {
        fear = fearLevel.high;
    }

    private void lowAttention()
    {
        minScaringDuration = minScareAtLowAttention;
        maxScaringDuration = maxScareAtLowAttention;
    }

    private void normalAttention()
    {
        minScaringDuration = minScareAtNormalAttention;
        maxScaringDuration = maxScareAtNormalAttention;
    }

    private void highAttention()
    {
        minScaringDuration = minScareAtHighAttention;
        maxScaringDuration = maxScareAtHighAttention;
    }

    public IEnumerator turnDownMusic()
    {
        dead = true;
        for(float i = currentAudio.volume; i > 0; i -= 0.001f)
        {
            currentAudio.volume = i;
            yield return null;
        }
    }

    public IEnumerator turnUpMusic()
    {
        dead = false;
        for (float i = currentAudio.volume; i <  volume; i += 0.001f)
        {
            currentAudio.volume = i;
            yield return null;
        }
    }
}
// 2 3 4 a szintek mindegyik legyen loopolva
// 1 10 a mission soundok
//+ szívritmus + random scary noises